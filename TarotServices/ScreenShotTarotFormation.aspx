<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScreenShotTarotFormation.aspx.cs"
    Inherits="TarotServices.ScreenShotTarotFormation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        
        .divTarotCard{ border:1px solid #55A5D8;}
    </style>
</head>
<body style="margin:0 auto;">
    <form id="form1" runat="server">
    <div id="div_formation" runat="server" style="display: block; border: 1px solid blue;">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <div style="left: <%# Eval("X") %>px; top: <%# Eval("Y") %>px; width: <%# Eval("W") %>px; height: <%# Eval("H") %>px; position: absolute;"
                    class="divTarotCard" index='${SortIndex}' describe='${Describe}'>
                    <div style="position: relative;">
                        <div style="width: 15px; height: 15px; position: absolute; z-index: 40; background-color: white;
                            line-height: 15px; font-size:12px;">
                            <%# Eval("SortIndex")%>
                        </div>
                        <image style="width:95%" src="<%= "http://"+ Lib.Utils.ConfigHelper.GetAppSetting("taluolaile.com")+"/content/Images/TarotCard/bg.jpg" %>"></image>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
