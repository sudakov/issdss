<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Authority.ascx.cs" Inherits="DSS.DSS.Controls.Authority" %>

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
    <asp:Panel ID="Panel1" runat="server" DefaultButton="_BTN_Enter">
    <tr>
        <td style="background-color:#CCE0F2; border-left:1px solid #bbc9d7; border-right:1px solid #bbc9d7;">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2" style="height:5px;">
                    </td>
                </tr>
                <tr>
                    <td class="menuLabel" style="padding:5px; text-align:right;">
                        Логин:
                    </td>
                    <td>
                        <div style="margin: 0px 9px 0px 0px;">
                            <asp:TextBox runat="server" ID="_TB_Login" Width="90%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="_TB_Login"
                                    CssClass="failureNotification" ErrorMessage="Поле является обязательным."
                                    ToolTip="Поле является обязательным." ValidationGroup="AutorizationGroup">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height:5px;">
                    </td>
                </tr>
                <tr>
                    <td class="menuLabel" style="padding:5px; text-align:right;">
                        Пароль:
                    </td>
                    <td>
                        <div style="margin: 0px 9px 0px 0px;">
                            <asp:TextBox runat="server" ID="_TB_Password" Width="90%" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="_TB_Password"
                                    CssClass="failureNotification" ErrorMessage="Поле является обязательным."
                                    ToolTip="Поле является обязательным." ValidationGroup="AutorizationGroup">*</asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height:5px;">
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
        <td style="background-color: #CCE0F2; border-left: 1px solid #bbc9d7; border-right: 1px solid #bbc9d7;">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="menuLink" style="font-weight:normal; font-size:11px; width:34%;">
                        <div onclick="window.location.href = 'Registration.aspx';" style="margin:0 auto; padding:5px;">
                            Регистрация
                        </div>
                    </td>
                    <td class="menuLink" style="font-weight:normal; font-size:11px; width:34%; white-space:nowrap;">
                        <div onclick="window.location.href = 'PasswordRecovery.aspx';" style="margin:0 auto; padding:5px;">
                            Забыли пароль?
                        </div>
                    </td>
                    <td class="menuLink">
                        <div onclick="document.getElementById('ctl00_Authority1__BTN_Enter').click();" style="margin:0 auto; padding:5px;">
                            Войти
                            <div style="display:none;">
                                <asp:Button runat="server" ID="_BTN_Enter" ValidationGroup="AutorizationGroup" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td class="menuSep" style="cursor:default;">
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table class="userMenuItem" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="footer"></td>
                    <td class="footerCorner"></td>
                </tr>
            </table>
        </td>
    </tr>
</table>