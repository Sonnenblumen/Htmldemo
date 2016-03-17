<%@ WebHandler Language="C#" Class="AutoComplete" %>

using System;
using System.Web;
using System.Data;

public class AutoComplete : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        GetAutoComplete(context);
    }
    
    private void GetAutoComplete(HttpContext context)
    {
        OraDbHelper helper = new OraDbHelper();     //實例化數據操作類
        string a = context.Request.QueryString["a"].ToString();
        string b = context.Request.QueryString["b"].ToString();
        string c = context.Request.QueryString["c"].ToString();
        string q = context.Request.QueryString["q"].ToString();

        string sql = "Select * From test Where userName like '" + q + "%'";
        DataSet ds = helper.getDS(sql);
        int i, j;
        j = ds.Tables[0].Rows.Count;
        for (i = 0; i < j; i++)
        {
            DataRow dr = ds.Tables[0].Rows[i];
            context.Response.Write(string.Format("{0}\n", dr["userName"]));
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}