using System;
using System.Data;
using System.Data.OleDb;
using System.Web;

/// <summary>
/// Common 的摘要描述
/// </summary>


public static class Common 
{
    private static string msgSuccess = "成功!";
    private static string msgFail="失敗，請聯系管理員!";
    private static string msgError = "出錯，請聯系管理員!";

    public static string MsgSuccess
    {
        get
        {
            return msgSuccess;
        }
    }

    public static string MsgFail
    {
        get 
        {
            return msgFail;
        }
    }

    public static string MsgError
    {
        get
        {
            return msgError;
        }
    }

    public static void AlertMsg(System.Web.UI.Page page, string msg)
    {
        string scriptMsg = string.Format("<script language='javascript'>alert('{0}')</script>", msg);
        page.ClientScript.RegisterStartupScript(page.GetType(), "", scriptMsg);
    }

    /// <summary>
    /// 彈出信息並且返回來源頁,防止事件刷新
    /// </summary>
    /// <param name="page">頁面</param>
    /// <param name="msg">信息</param>
    public static void AlertMsgBack(System.Web.UI.Page page, string msg)
    {
        string scriptMsg = string.Format("<script language='javascript'>alert('{0}');location.href='{1}';</script>", msg, page.Request.Url.ToString());
        page.Response.Write(scriptMsg);
    }

    public static void RunScript(System.Web.UI.Page page, string script)
    {
        string scriptMsg = string.Format("<script language='javascript'>{0}</script>", script);
        page.ClientScript.RegisterStartupScript(page.GetType(), "", scriptMsg);
    }

    public static void ShowModalDialog(System.Web.UI.Page page, string url,string top,string left,string width,string height)
    {
        string script = "<script language='javascript'>";
        script=script+string.Format("window.showModalDialog('../{0}', window, 'dialogTop:{1}px;dialogLeft:{2}px;dialogWidth:{3}px;dialogHeight:{4}px');",
                                            url,top,left,width,height
                            );
        script=script+"</script>";
        page.ClientScript.RegisterStartupScript(page.GetType(), "", script);
    }

    public static void RedirectLogON(System.Web.UI.Page page)
    {
        if ( page.Session["uid"] == null)
        {
            page.Response.Redirect("../default.aspx");
        }
    }

    public static void IsNullSession(System.Web.UI.Page page)
    {
        if ( page.Session["uid"] == null)
        {
            page.Response.Redirect("../default.aspx");
        }
    }

    public static void RedirectDefault(System.Web.UI.Page page)
    {
        page.Response.Redirect("../default.aspx");
    }

    public static string EncodePassword(string userNO, string password, string passwordformate)
    {
        string Encodepassword;
        if (passwordformate == "sha1")
            Encodepassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
        else if (passwordformate == "md5 ")
            Encodepassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile( password, "md5");
        else
            Encodepassword = " ";
        return Encodepassword;
    }

    /// <summary>
    /// 返回COOKIE信息
    /// </summary>
    /// <param name="page"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static HttpCookie GetCookieUser(this System.Web.UI.Page page, string key)
    {
        HttpCookie CookieUser = page.Request.Cookies["opUser"];
        if (CookieUser == null)
        {
            page.Response.Redirect("Default.aspx");
        }
        return CookieUser;
    }

    public static void SetCookieUser(this System.Web.UI.Page page, string loginId, string contract, string database)
    {
        HttpCookie CookieUser = page.Request.Cookies["opUser"];
        if (CookieUser == null)
        {
            CookieUser = new HttpCookie("opUser");
            CookieUser.Values["opLoginId"] = HttpUtility.UrlEncode(loginId);
            CookieUser.Values["opContract"] = HttpUtility.UrlEncode(contract);
            page.Response.AppendCookie(CookieUser);
        }

        HttpCookie cookieDb = page.Request.Cookies["cookieDb"];
        if (cookieDb == null)
        {
            cookieDb = new HttpCookie("cookieDb");
            database = database.ToUpper();
            cookieDb.Values["database"] = HttpUtility.UrlEncode(database);            
            page.Response.AppendCookie(cookieDb);
        }
    }

    /// <summary>
    /// 從Cookie裡取用戶登錄的User id
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static string GetUserId(this System.Web.UI.Page page)
    {
        string sUserId;
        HttpCookie CookieUser = page.Request.Cookies["opUser"];
        sUserId = CookieUser["opLoginId"].ToString();
        return sUserId;
    }

    /// <summary>
    /// 從Cookie裡取用戶登錄的廠區
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static string GetUserLoginContract(this System.Web.UI.Page page)
    {
        string sContract;
        HttpCookie CookieUser = page.Request.Cookies["opUser"];
        sContract = CookieUser["opContract"].ToString();
        return sContract;
    }

    /// <summary>
    /// 從Cookie裡取用戶登錄的Database 
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static string GetDatabaseName(this System.Web.UI.Page page)
    {
        string sDbName;
        HttpCookie CookieUser = page.Request.Cookies["cookieDb"];
        sDbName = CookieUser["database"].ToString();
        return sDbName;
    }
}

