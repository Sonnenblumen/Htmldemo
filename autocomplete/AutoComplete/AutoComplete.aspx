<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="AutoComplete.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AutoComplete實例</title>
    <link rel="stylesheet" href="css/jquery.autocomplete.css" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.6.js"></script>
    <script type="text/javascript" src="js/jquery.autocomplete.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
        $("#<%=txtUserName.ClientID %>").autocomplete("Ajax/AutoComplete.ashx", {
            width: 155,
            selectFirst: true,
            autoFill: true,
            minChars: 0,
            scroll: true,
            mustMatch: true,
            extraParams: { a: "1", b: "2", c: "3"} //此處實際請求的URL為"Ajax/AutoComplete.ashx?q='[你在txtUserName中輸入的值]'&a=1&b=2&c=3"

            //BUG   ---begin
            //此處報錯,灰常的無語,日後有時間再驗證.
            //formatItem: function (row, i, max) {
            //    return "<td align='left'>{0}</td><td align='right'>{1}</td>".format(row[0], row[1]);
            //}
            //BUG   ---end

        }
            );
        });

        String.prototype.format = function() { //String 格式化
            var arg = arguments;
            return this.replace(/\{(\d+)\}/g, function(i, m) {
                return arg[m];
            });
        }
      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
    </div>
    </form>
</body>
</html>
