<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonDLL.ascx.cs" Inherits="DSS.DSS.Controls.PersonDLL" %>

<asp:Panel runat="server" ID="_PNL_PersonDDL" Visible="false">
<table class="userMenu" cellpadding="0" cellspacing="0">
    <tr>
        <td class="menuSpacing">
        </td>
    </tr>
    <tr>
        <td>
            <table class="userMenuItem" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="headerCorner"></td>
                    <td class="header"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="menuSep" style="cursor:default;">
        </td>
    </tr>
    <tr>
        <td class="menuLabel" style="border-left:1px solid #bbc9d7; border-right:1px solid #bbc9d7; padding-top:3px; text-align:center;">
            Пользователь
        </td>
    </tr>
    <tr>
        <td style="padding:5px; background-color:#CCE0F2; border-left:1px solid #bbc9d7; border-right:1px solid #bbc9d7;">
            <asp:DropDownList runat="server" ID="_DDL_Person" Width="100%" DataTextField="name" DataValueField="id" AutoPostBack="true" />
        </td>
    </tr>
    <tr>
        <td class="menuSep" style="cursor:default;">
        </td>
    </tr>
    <tr>
        <td>
            <table class="userMenuItem" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="footer"></td>
                    <td class="footerCorner"></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>