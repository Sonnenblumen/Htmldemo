using System;
using System.Data;
using System.Configuration;
//using System.Web.Security;
using System.Data.OleDb;


/// <summary>
/// OraDbHelper ,以System.Data.OleDb類來操作數據庫
/// </summary>
public class OraDbHelper
{
    public OraDbHelper()
    {
    }




    public string GetConnectionString()
    {

        string connectionString = "";
        string connectionName = "SqlConnectionString";
        if (string.IsNullOrEmpty(connectionName) || ConfigurationManager.ConnectionStrings[connectionName] == null)
            throw new ArgumentNullException("connectionName");

        ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[connectionName];
        connectionString = setting.ConnectionString;


        return connectionString;
    }


    /// <summary>
    /// 取得Oracle数据库存连接,需在app.config中的appSetting处设定连接字符串OraDbConnString
    /// </summary>
    /// <returns></returns>
    public OleDbConnection GetConn()
    {
        string strConn;

        OleDbConnection Conn = new OleDbConnection();
        strConn = GetConnectionString();
        Conn.ConnectionString = strConn;
        Conn.Open();

        return Conn;

    }




    public OleDbConnection GetConn(string dataSource, string username, string password)
    {
        string connString = "Provider=MSDAORA;Data Source=" + dataSource + ";Persist Security Info=True;Password=" + password + ";User ID=" + username;

        OleDbConnection Conn = new OleDbConnection();

        try
        {
            Conn.ConnectionString = connString;
            Conn.Open();
        }
        catch (Exception ex)
        {
            return null;
        }

        return Conn;
    }

    /// <summary>
    /// 取得数据集
    /// </summary>
    /// <param name="strSQL"></param>
    /// <param name="Conn"></param>
    /// <returns></returns>
    public DataSet getDS(string strSQL, OleDbConnection Conn)
    {
        //string strSQL;

        OleDbDataAdapter Adpter;
        DataSet ds;
        Adpter = new OleDbDataAdapter(strSQL, Conn);

        ds = new DataSet();
        Adpter.Fill(ds, "table1");

        return ds;
    }


    /// <summary>
    /// 取得数据集,默認連接的數據庫存是在app.config中的appSetting处设定连接字符串OraDbConnString
    /// </summary>
    /// <param name="strSQL"></param>
    /// <returns></returns>

    public DataSet getDS(string strSQL)
     {
        //string strSQL;

        OleDbDataAdapter Adpter;
        DataSet ds;
        OleDbConnection Conn;

        Conn = GetConn();
        Adpter = new OleDbDataAdapter(strSQL, Conn);

        ds = new DataSet();
        Adpter.Fill(ds, "table1");

        return ds;
    }






    public DataTable getDT(string strSQL)
    {
        DataSet ds;

        ds = getDS(strSQL);

        return ds.Tables[0];
    }


    /// <summary>
    /// 执行 sql command
    /// </summary>
    /// <param name="strSQL"></param>
    /// <param name="Conn"></param>
    /// <returns></returns>
    public string ExecuteNonQuery(string strSQL, OleDbConnection Conn)
    {

        OleDbCommand Cmd = new OleDbCommand();
        Cmd.CommandType = CommandType.Text;
        Cmd.CommandText = strSQL;
        Cmd.Connection = Conn;

        Cmd.Transaction = Conn.BeginTransaction();
        try
        {
            Cmd.ExecuteNonQuery();
            Cmd.Transaction.Commit();
            return "";
        }
        catch (Exception Err)
        {
            Cmd.Transaction.Rollback();
            return Err.Message;
        }
        finally
        {
            //if (Conn != null) Conn.Dispose();
            if (Cmd != null) Cmd.Dispose();
            //if (Adpter != null) Adpter.Dispose();
        }

    }

    /// <summary>
    /// 执行 sql command,默認連接的數據庫存是在app.config中的appSetting处设定连接字符串OraDbConnString
    /// </summary>
    /// <param name="strSQL"></param>
    /// <returns></returns>
    public string ExecuteNonQuery(string strSQL)
    {
        OleDbConnection Conn;

        Conn = GetConn();

        OleDbCommand Cmd = new OleDbCommand();
        Cmd.CommandType = CommandType.Text;
        Cmd.CommandText = strSQL;
        Cmd.Connection = Conn;

        if (Conn != null)
        {
            Cmd.Transaction = Conn.BeginTransaction();

            try
            {
                Cmd.ExecuteNonQuery();
                Cmd.Transaction.Commit();
                return "";
            }
            catch (Exception Err)
            {
                Cmd.Transaction.Rollback();
                return Err.Message;
            }
            finally
            {
                if (Conn != null) Conn.Dispose();
                if (Cmd != null) Cmd.Dispose();
                //if (Adpter != null) Adpter.Dispose();
            }
        }
        else
        {
            return "Connection is null!";
        }

    }

    /// <summary>
    /// 取得DataReader,默認連接的數據庫存是在web.config中的appSetting处设定连接字符串SqlDbConnString
    /// </summary>
    /// <param name="strSQL"></param>
    /// <returns></returns>
    public OleDbDataReader GetReader(string strSQL)
    {
        //OleDbDataAdapter Adpter;
        OleDbDataReader oReader;
        OleDbConnection Conn = new OleDbConnection();
        OleDbCommand oComm = new OleDbCommand();

        Conn = GetConn();
        oComm.Connection = Conn;
        oComm.CommandText = strSQL;

        try
        {
            oReader = oComm.ExecuteReader();

        }

        catch
        {
            return null;
        }

        return oReader;
    }

    /// <summary>
    /// 取得DataReader
    /// </summary>
    /// <param name="strSQL"></param>
    /// <param name="oConn"></param>
    /// <returns></returns>
    public OleDbDataReader GetReader(string strSQL, OleDbConnection oConn)
    {
        //OleDbDataAdapter Adpter;
        OleDbDataReader oReader;
        OleDbCommand oComm = new OleDbCommand();

        oComm.Connection = oConn;
        oComm.CommandText = strSQL;

        try
        {
            oReader = oComm.ExecuteReader();

        }
        catch
        {
            return null;
        }

        return oReader;
    }


}

