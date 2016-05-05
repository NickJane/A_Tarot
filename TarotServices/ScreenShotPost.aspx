<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScreenShotPost.aspx.cs" Inherits="TarotServices.ScreenShotPost" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= TarotServices.Common.ValidIp(Request.UserHostAddress) %></title>
    <style type="text/css">
        
        .divTarotCard{ border:1px solid #55A5D8;}
    </style>
</head>
<body style="margin:0 auto;">
    <form id="form1" runat="server">
    <span id="spanFormationName" runat="server" style="font-weight: bold; height:20px;"></span>
    <div id="div_formation" runat="server" style="display: block; border: 0px solid blue;">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <div style="width: 15px; position: absolute;font-size:10px;left: <%# Convert.ToInt32(Eval("X"))-15 %>px; top: <%#  Eval("Y") %>px;">
                            <%# Eval("SortIndex")%>,<%# Eval("CardName")%>--
                            <%# Convert.ToBoolean( Eval("IsHandStand"))?"逆":"正" %>
                        </div>
                <div style="left: <%# Eval("X") %>px; top: <%# Eval("Y") %>px; width: <%# Eval("W") %>px; height: <%# Eval("H") %>px; position: absolute;"
                    class="divTarotCard" index='${SortIndex}' describe='${Describe}'>
                    <div style="position: relative;">
                        
                        <image style="width:98%" src="<%= "http://"+ Lib.Utils.ConfigHelper.GetAppSetting("taluolaile.com") %><%# Eval("Imgurl") %>"></image>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <i>http://www.taluolaile.com</i>塔罗来了@copyright
    </form>
</body>
</html>
