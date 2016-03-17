using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.OleDb;

#region DbHelper
/// <summary>
/// 数据访问Helper。
/// </summary>
public class DbHelper
{
    #region properties

    private DbProviderFactory dbFactory;

    private string connectionString;

    public ExecutePages ExecutePager { get; set; }

    public ExecuteTop ExecuteToper { get; set; }
    #endregion

    #region construct
    /// <summary>
    /// 始初化 DbHelper 类的新实例。
    /// </summary>
    /// <param name="connectionName">连接字符串配置文件节中的单个命名连接字符串名。</param>
    //public DbHelper(string connectionName)
    public DbHelper()
    {
        string connectionName = "SqlConnectionString";
        if (string.IsNullOrEmpty(connectionName) || ConfigurationManager.ConnectionStrings[connectionName] == null)
            throw new ArgumentNullException("connectionName");

        ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[connectionName];
        dbFactory = DbProviderFactories.GetFactory(setting.ProviderName);
        connectionString = setting.ConnectionString;
    }
    #endregion

    #region methods
    /// <summary>
    /// 返回实现 DbCommand 类的提供程序的类的一个新实例。
    /// </summary>
    /// <returns></returns>
    public DbCommand CreateCommand()
    {
        return dbFactory.CreateCommand();
    }
    public OleDbConnection openConn(string conStr)
    {
        try
        {
            OleDbConnection Conn = new OleDbConnection();

            //Get DB conncection string from web.config file.
            System.Configuration.Configuration WebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            string strConn = WebConfig.AppSettings.Settings[conStr].Value;

            Conn.ConnectionString = strConn;
            Conn.Open();

            return Conn;
        }
        catch
        {
            return null;
        }
    }

    public OleDbConnection GetConn()
    {
        string strConn;

        try
        {
            OleDbConnection Conn = new OleDbConnection();

            //Get DB conncection string from web.config file.
            System.Configuration.Configuration WebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
            strConn = WebConfig.AppSettings.Settings["SqlConnectionString"].Value;

            Conn.ConnectionString = strConn;
            Conn.Open();

            return Conn;
        }
        catch
        {

            return null;
        }

    }



    //public Boolean doCmd(String strSQL)
    //{
        
    //    //cm.Transaction = cn.BeginTransaction();
    //    try
    //    {
    //        OleDbConnection cn = openConn("OraDbConnString");
    //        OleDbCommand cm = new OleDbCommand(strSQL, cn);
    //        cm.ExecuteNonQuery();
    //        //cm.Transaction.Commit();
    //        return true;
    //    }
    //    catch(Exception ex)
    //    {
    //       //cm.Transaction.Rollback();
    //        return false;
    //    }
    //}


   



    /// <summary>
    /// 返回实现 DbCommandBuilder 类的提供程序的类的一个新实例。
    /// </summary>
    /// <returns></returns>
    public DbCommandBuilder CreateCommandBuilder()
    {
        return dbFactory.CreateCommandBuilder();
    }

    /// <summary>
    /// 返回实现 DbConnection 类的提供程序的类的一个新实例。
    /// </summary>
    /// <returns></returns>
    public DbConnection CreateConnection()
    {
        DbConnection conn = dbFactory.CreateConnection();
        conn.ConnectionString = connectionString;
        return conn;
        
    }

    /// <summary>
    /// 返回实现 DbConnectionStringBuilder 类的提供程序的类的一个新实例。
    /// </summary>
    /// <returns></returns>
    public DbConnectionStringBuilder CreateConnectionStringBuilder()
    {
        return dbFactory.CreateConnectionStringBuilder();
    }

    /// <summary>
    /// 返回实现 DbDataAdapter 类的提供程序的类的一个新实例。
    /// </summary>
    /// <returns></returns>
    public DbDataAdapter CreateDataAdapter()
    {
        return dbFactory.CreateDataAdapter();
    }

    /// <summary>
    /// 返回实现 DbDataSourceEnumerator 类的提供程序的类的一个新实例。
    /// </summary>
    /// <returns></returns>
    public DbDataSourceEnumerator CreateDataSourceEnumerator()
    {
        return dbFactory.CreateDataSourceEnumerator();
    }

    /// <summary>
    /// 返回实现 DbParameter 类的提供程序的类的一个新实例。
    /// </summary>
    /// <returns></returns>
    public DbParameter CreateParameter()
    {
        return dbFactory.CreateParameter();
    }

    /// <summary>
    /// 准备一个数据操作命令实例。
    /// </summary>
    /// <param name="conn">数据库连接实例。</param>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <param name="cmdType">CommandType 值之一。</param>
    /// <param name="cmdParams">SQL 语句或存储过程的参数。</param>
    /// <returns>DbCommand 实例。</returns>
    protected DbCommand PrepareCommand(DbConnection conn, string cmdText, CommandType cmdType, params IDataParameter[] cmdParams)
    {
        DbCommand cmd = CreateCommand();
        cmd.Connection = conn;
        cmd.CommandText = cmdText;
        cmd.CommandType = cmdType;

        if (cmdParams != null)
        {
            foreach (IDataParameter param in cmdParams)
            {
                cmd.Parameters.Add(param);
            }
        }

        return cmd;
    }

    /// <summary>
    /// 执行数据操作，返回首行首列值。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <param name="cmdType">CommandType 值之一。</param>
    /// <param name="cmdParams">SQL 语句或存储过程的参数。</param>
    /// <returns>首行首列值，未查询到数据时返回null。</returns>
    public object ExecuteScalar(string cmdText, CommandType cmdType, params IDataParameter[] cmdParams)
    {
        object val;

        using (DbConnection conn = CreateConnection())
        {
            using (DbCommand cmd = PrepareCommand(conn, cmdText, cmdType, cmdParams))
            {
                conn.Open();
                val = cmd.ExecuteScalar();
                conn.Close();
                cmd.Parameters.Clear();
            }
        }

        return val;
    }

    /// <summary>
    /// 执行数据操作，返回首行首列值。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <param name="cmdParams">SQL 语句或存储过程的参数。</param>
    /// <returns>首行首列值，未查询到数据时返回null。</returns>
    public object ExecuteScalar(string cmdText, params IDataParameter[] cmdParams)
    {
        return ExecuteScalar(cmdText, CommandType.Text, cmdParams);
    }

    /// <summary>
    /// 执行数据操作，返回首行首列值。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <returns>首行首列值，未查询到数据时返回null。</returns>
    public object ExecuteScalar(string cmdText)
    {
        return ExecuteScalar(cmdText, CommandType.Text, null);
    }

    /// <summary>
    /// 执行数据操作，返回受影响行数。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句。</param>
    /// <param name="cmdType">CommandType 值之一。</param>
    /// <param name="cmdParams">SQL 语句的参数。</param>
    /// <returns>受影响行数。</returns>
    public int ExecuteNonQuery(string cmdText, CommandType cmdType, params IDataParameter[] cmdParams)
    {
        using (DbConnection conn = CreateConnection())
        {
            using (DbCommand cmd = PrepareCommand(conn, cmdText, cmdType, cmdParams))
            {
                conn.Open();
                int val = cmd.ExecuteNonQuery();
                conn.Close();
                cmd.Parameters.Clear();
                return val;
            }
        }
    }

    /// <summary>
    /// 执行数据操作，返回受影响行数。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句。</param>
    /// <param name="cmdType">CommandType 值之一。</param>
    /// <returns>受影响行数。</returns>
    public int ExecuteNonQuery(string cmdText, CommandType cmdType)
    {
        return ExecuteNonQuery(cmdText, cmdType, null);
    }

    /// <summary>
    /// 执行数据操作，返回受影响行数。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句。</param>
    /// <param name="cmdParams">SQL 语句的参数。</param>
    /// <returns>受影响行数。</returns>
    public int ExecuteNonQuery(string cmdText, params IDataParameter[] cmdParams)
    {
        return ExecuteNonQuery(cmdText, CommandType.Text, cmdParams);
    }

    /// <summary>
    /// 执行数据操作，返回受影响行数。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句。</param>
    /// <returns>受影响行数。</returns>
    public int ExecuteNonQuery(string cmdText)
    {
        return ExecuteNonQuery(cmdText, CommandType.Text, null);
    }

    /// <summary>
    /// 执行数据操作，返回DbDataReader。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <param name="cmdType">CommandType 值之一。</param>
    /// <param name="cmdParams">SQL 语句或存储过程的参数。</param>
    /// <returns>DbDataReader实例。</returns>
    public DbDataReader ExecuteReader(string cmdText, CommandType cmdType, params IDataParameter[] cmdParams)
    {
        DbConnection conn = CreateConnection();

        using (DbCommand cmd = PrepareCommand(conn, cmdText, cmdType, cmdParams))
        {
            conn.Open();
            DbDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return dr;
        }
    }

    /// <summary>
    /// 执行数据操作，返回DbDataReader。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <param name="cmdParams">SQL 语句或存储过程的参数。</param>
    /// <returns>DbDataReader实例。</returns>
    public DbDataReader ExecuteReader(string cmdText, params IDataParameter[] cmdParams)
    {
        return ExecuteReader(cmdText, CommandType.Text, cmdParams);
    }

    /// <summary>
    /// 执行数据操作，返回DbDataReader。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <param name="cmdType">CommandType 值之一。</param>
    /// <returns>DbDataReader实例。</returns>
    public DbDataReader ExecuteReader(string cmdText, CommandType cmdType)
    {
        return ExecuteReader(cmdText, cmdType, null);
    }

    /// <summary>
    /// 执行数据操作，返回DbDataReader。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <returns>DbDataReader实例。</returns>
    public DbDataReader ExecuteReader(string cmdText)
    {
        return ExecuteReader(cmdText, CommandType.Text, null);
    }

    /// <summary>
    /// 执行数据操作，返回DataSet。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <param name="cmdType">CommandType 值之一。</param>
    /// <param name="cmdParams">SQL 语句或存储过程的参数。</param>
    /// <returns>DataSet实例。</returns>
    public DataSet ExecuteDataSet(string cmdText, CommandType cmdType, params IDataParameter[] cmdParams)
    {
        DataSet ds = new DataSet();

        using (DbConnection conn = CreateConnection())
        {
            using (DbDataAdapter da = CreateDataAdapter())
            {
                using (DbCommand cmd = PrepareCommand(conn, cmdText, cmdType, cmdParams))
                {
                    cmd.CommandTimeout = int.MaxValue;
                    conn.Open();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    conn.Close();
                    cmd.Parameters.Clear();
                }
            }
        }

        return ds;
    }


    /// <summary>
    /// 执行数据操作，返回DataSet。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <returns>DataSet实例。</returns>
    public DataSet ExecuteDataSet(string cmdText)
    {
        return ExecuteDataSet(cmdText, CommandType.Text, null);
    }   

    /// <summary>
    /// 执行数据操作，返回DataSet。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>
    /// <param name="cmdType">CommandType 值之一。</param>
    /// <returns>DataSet实例。</returns>
    public DataSet ExecuteDataSet(string cmdText, CommandType cmdType)
    {
        return ExecuteDataSet(cmdText, cmdType, null);
    }

    /// <summary>
    /// 执行数据操作，返回DataSet。
    /// </summary>
    /// <param name="cmdText">要执行的 SQL 语句或存储过程。</param>       
    /// <param name="cmdParams">SQL 语句或存储过程的参数。</param>
    /// <returns>DataSet实例。</returns>
    public DataSet ExecuteDataSet(string cmdText, params IDataParameter[] cmdParams)
    {
        return ExecuteDataSet(cmdText, CommandType.Text, cmdParams);
    }


 

    /// <summary>
    /// 实现数据查询分页（支持参数化查询）。
    /// </summary>
    /// <param name="tables">查询表名（支持多表）。</param>
    /// <param name="fields">查询需返回结果集的字段。</param>
    /// <param name="where">条件字串，需要加WHERE，可使用INNER JOIN等联合查询（请用参数化查询）。</param>
    /// <param name="order">排序字串（不需加ORDER BY）。</param>
    /// <param name="pageIndex">当前页。</param>
    /// <param name="pageSize">页大小。</param>
    /// <param name="recordCount">out 返回记录数。</param>
    /// <param name="parameters">对应查询化查询的参数值。</param>
    /// <returns>DbDataReader实例。</returns>
    public DbDataReader ExecutePages(string tables, string fields, string where, string order, int pageIndex, int pageSize, out int recordCount, params IDataParameter[] parameters)
    {
        if (ExecutePager == null)
            throw new NotSupportedException();

        return ExecutePager.Execute(tables, fields, where, order, pageIndex, pageSize, out recordCount, parameters);
    }

    /// <summary>
    ///SQL2005分页扩展方法，请谨慎使用
    /// </summary>
    /// <param name="tables">查询表名（支持多表）。</param>
    /// <param name="fields">查询需返回结果集的字段。</param>
    /// <param name="where">条件字串，不需要加WHERE，（请用参数化查询,参数名请使用表列名）。</param>
    /// <param name="order">排序字串（不需加ORDER BY）。</param>
    /// <param name="pageIndex">当前页。</param>
    /// <param name="pageSize">页大小。</param>
    /// <param name="recordCount">out 返回记录数。</param>
    /// <param name="parameters">对应查询化查询的参数值,参数名请使用表列名</param>
    /// <returns>DbDataReader实例。</returns>
    public DbDataReader ExecutePagesExpansion(string tables, string fields, string where, string order, int pageIndex, int pageSize, out int recordCount, params IDataParameter[] parameters)
    {
        if (ExecutePager == null)
            throw new NotSupportedException();

        return ExecutePager.ExecutePage(tables, fields, where, order, pageIndex, pageSize, out recordCount, parameters);
    }
    public DbDataReader ExecuteTop(string tables, string fields, string where, string order, int top, params IDataParameter[] parameters)
    {
        if (ExecuteToper == null)
            throw new NotSupportedException();

        return ExecuteToper.Execute(tables, fields, where, order, top, parameters);
    }

    /// <summary>
    /// 返回 DbParameter 类的一个新实例。
    /// </summary>
    /// <param name="name">要映射的参数的名称。</param>
    /// <param name="type">DbType 值之一。</param>
    /// <param name="size">列中数据的最大大小（以字节为单位）</param>
    /// <param name="pd">ParameterDirection 值之一。默认为 Input。</param>
    /// <returns>DbParameter实例。</returns>
    public DbParameter NewParam(string name, DbType type, int size, ParameterDirection pd)
    {
        DbParameter param = CreateParameter();
        param.ParameterName = name;
        param.DbType = type;
        param.Size = size;
        param.Direction = pd;
        return param;
    }

    /// <summary>
    /// 返回 DbParameter 类的一个新实例。
    /// </summary>
    /// <param name="name">要映射的参数的名称。</param>
    /// <param name="type">DbType 值之一。</param>
    /// <param name="size">列中数据的最大大小（以字节为单位）。</param>
    /// <param name="value">新 DbParameter 实例的值。</param>
    /// <returns>DbParameter实例。</returns>
    public DbParameter NewParam(string name, DbType type, int size, object value)
    {
        DbParameter param = CreateParameter();
        param.ParameterName = name;
        param.DbType = type;
        param.Size = size;
        if (value != null)
            param.Value = value;
        else
            param.Value = DBNull.Value;
        return param;
    }

    /// <summary>
    /// 返回 DbParameter 类的一个新实例。
    /// </summary>
    /// <param name="name">要映射的参数的名称。</param>
    /// <param name="type">DbType 值之一。</param>
    /// <param name="value">新 DbParameter 实例的值。</param>
    /// <returns>DbParameter实例。</returns>
    public DbParameter NewParam(string name, DbType type, object value)
    {
        DbParameter param = CreateParameter();
        param.ParameterName = name;
        param.DbType = type;
        if (value != null)
            param.Value = value;
        else
            param.Value = DBNull.Value;
        return param;
    }

    /// <summary>
    /// 返回 DbParameter 类的一个新实例。
    /// </summary>
    /// <param name="name">要映射的参数的名称。</param>
    /// <param name="value">新 DbParameter 实例的值。</param>
    /// <returns>DbParameter实例。</returns>
    public DbParameter NewParam(string name, object value)
    {
        DbParameter param = CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        return param;
    }
    #endregion
}
#endregion

#region ExecutePages
/// <summary>
/// 数据分页抽象基类。
/// </summary>
public abstract class ExecutePages
{
    public DbHelper Helper { get; set; }

    public ExecutePages(DbHelper helper)
    {
        Helper = helper;
    }

    public abstract DbDataReader Execute(string tables, string fields, string where, string order, int pageIndex, int pageSize, out int recordCount, params IDataParameter[] parameters);

    public abstract DbDataReader ExecutePage(string tables, string fields, string where, string order, int pageIndex, int pageSize, out int recordCount, params IDataParameter[] parameters);
}

/// <summary>
/// 基于 Sql Server 2005 的分页实现。
/// </summary>
public class SqlServer9ExecutePages : ExecutePages
{
    public SqlServer9ExecutePages(DbHelper helper)
        : base(helper)
    {
    }

    public override DbDataReader Execute(string tables, string fields, string where, string order, int pageIndex, int pageSize, out int recordCount, params IDataParameter[] parameters)
    {
        string count = string.Format("SELECT COUNT(0) FROM {0}  {1}", tables, where);
        recordCount = Convert.ToInt32(Helper.ExecuteScalar(count, parameters));
        string sql = string.Format("SELECT {0} FROM(SELECT {0},ROW_NUMBER() OVER(ORDER BY {1}) AS row FROM {2} {3}) tmp WHERE tmp.row BETWEEN {4} AND {5}", fields, order, tables, where, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
        return Helper.ExecuteReader(sql, CommandType.Text, parameters);
    }






    public override DbDataReader ExecutePage(string tables, string fields, string where, string order, int pageIndex, int pageSize, out int recordCount, params IDataParameter[] parameters)
    {

        string count = string.Format("SELECT COUNT(0) FROM {0} WHERE {1}", tables, where);
        recordCount = Convert.ToInt32(Helper.ExecuteScalar(count, parameters));


        //string sql = string.Format("with query as (SELECT {0},ROW_NUMBER() OVER(ORDER BY {1}) AS row FROM {2} WHERE {3})select * from query where row BETWEEN {4} AND {5}", fields, order, tables, where, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
        string sql = string.Format("SELECT {0} FROM {1} WHERE {2} ", fields, tables, where);
        return Helper.ExecuteReader(sql, CommandType.Text, parameters);
    }


}

/// <summary>
/// 基于 SQLite 的分页实现。
/// </summary>
public class SQLiteExecutePages : ExecutePages
{
    public SQLiteExecutePages(DbHelper helper)
        : base(helper)
    {
    }

    public override DbDataReader Execute(string tables, string fields, string where, string order, int pageIndex, int pageSize, out int recordCount, params IDataParameter[] parameters)
    {
        string count = string.Format("SELECT COUNT(0) FROM {0} {1}", tables, where);
        recordCount = Convert.ToInt32(Helper.ExecuteScalar(count, parameters));
        string sql = string.Format("SELECT {0} FROM {1} {2} ORDER BY {3} LIMIT {4},{5}", fields, tables, where, order, (pageIndex - 1) * pageSize, pageSize);
        return Helper.ExecuteReader(sql, CommandType.Text, parameters);
    }

    public override DbDataReader ExecutePage(string tables, string fields, string where, string order, int pageIndex, int pageSize, out int recordCount, params IDataParameter[] parameters)
    {
        throw new NotImplementedException();
    }
}
#endregion

#region ExecuteTop
public abstract class ExecuteTop
{
    public DbHelper Helper { get; set; }

    public ExecuteTop(DbHelper helper)
    {
        Helper = helper;
    }

    public abstract DbDataReader Execute(string tables, string fields, string where, string order, int top, params IDataParameter[] parameters);
}

public class SqlServerExecuteTop : ExecuteTop
{
    public SqlServerExecuteTop(DbHelper helper)
        : base(helper)
    {
    }

    public override DbDataReader Execute(string tables, string fields, string where, string order, int top, params IDataParameter[] parameters)
    {
        string sql;

        if (top < 0)
        {
            sql = string.Format("SELECT {0} FROM {1} {2} ORDER BY {3}", fields, tables, where, order);
        }
        else
        {
            sql = string.Format("SELECT TOP {0} {1} FROM {2} {3} ORDER BY {4}", top, fields, tables, where, order);
        }
        return Helper.ExecuteReader(sql, CommandType.Text, parameters);
    }
}

public class SQLiteExecuteTop : ExecuteTop
{
    public SQLiteExecuteTop(DbHelper helper)
        : base(helper)
    {
    }

    public override DbDataReader Execute(string tables, string fields, string where, string order, int top, params IDataParameter[] parameters)
    {
        string sql = string.Format("SELECT {0} FROM {1} {2} ORDER BY {3} LIMIT {4}", fields, tables, where, order, top);
        return Helper.ExecuteReader(sql, CommandType.Text, parameters);
    }
}
#endregion
