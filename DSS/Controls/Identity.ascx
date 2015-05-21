<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Identity.ascx.cs" Inherits="DSS.DSS.Controls.Identity" %>

<table class="userMenu" cellpadding="0" cellspacing="0" border="0">
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
        <td style="background-color: #CCE0F2; border-left: 1px solid #bbc9d7; border-right: 1px solid #bbc9d7;">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="menuLabel" style="text-align:right; vertical-align:top; padding-right:7px; padding-top:12px; padding-bottom:7px;">
                                    Задача:
                                </td>
                                <td class="menuLabel" style="text-align:left; vertical-align:top; font-size:11px; padding-right:12px; padding-top:12px; padding-bottom:7px;">
                                    <asp:Label runat="server" ID="_LBL_Task"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="menuLabel" style="text-align:right; vertical-align:top; padding-left:12px; padding-right:7px; padding-bottom:12px;">
                                    Пользователь:
                                </td>
                                <td class="menuLabel" style="text-align:left; vertical-align:top; font-size:11px; padding-right:12px; padding-bottom:12px;">
                                    <asp:Label runat="server" ID="_LBL_Name"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="menuSep" colspan="2" style="border:0px; cursor:default;">
                    </td>
                </tr>
                <tr>
                    <td class="menuLink" style="width:50%;">
                        <div onclick="window.location.href = 'Options.aspx';" style="margin:0 auto; padding:5px;">
                            Настройки
                        </div>
                    </td>
                    <td class="menuLink">
                        <div onclick="document.getElementById('ctl00_Identity1__BTN_Exit').click();" style="margin:0 auto; padding:5px;">
                            Выход
                            <div style="display:none;">
                                <asp:Button runat="server" ID="_BTN_Exit" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
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