<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true"
    CodeBehind="Registration.aspx.cs" Inherits="DSS.DSS.Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Регистрация пользователя
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Регистрация пользователя
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table class="contentItems" style="width: 100%;" cellpadding="0" cellspacing="0"
        border="0">
        <tr>
            <td class="contentItemTitle50 contentItem">
                <asp:Label runat="server" ID="Label1" Text="Имя пользователя" />
            </td>
            <td class="itemSpacingHoriz">
            </td>
            <td class="controls">
                <div style="margin: 5px 9px 5px 5px;">
                    <asp:TextBox runat="server" ID="_TB_Name" Width="100%" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_TB_Name"
                        Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="RegisterUserValidationGroup"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing">
            </td>
        </tr>
        <tr>
            <td class="contentItemTitle50 contentItem">
                <asp:Label runat="server" ID="Label2" Text="Логин" />
            </td>
            <td class="itemSpacingHoriz">
            </td>
            <td class="controls">
                <div style="margin: 5px 9px 5px 5px;">
                    <asp:TextBox runat="server" ID="_TB_Login" Width="100%" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_TB_Login"
                        Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="RegisterUserValidationGroup"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                        runat="server" ControlToValidate="_TB_Login" ErrorMessage="<br>Логин должен быть длиной 4-14 символов и может состоять из латинских символов и цифр"
                        SetFocusOnError="True" ValidationExpression="\w{4}(\w)?(\w)?(\w)?(\w)?(\w)?(\w)?"
                        Display="Dynamic" ValidationGroup="RegisterUserValidationGroup"></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing">
            </td>
        </tr>
        <tr>
            <td class="contentItemTitle50 contentItem">
                <asp:Label runat="server" ID="Label10" Text="Пароль" />
            </td>
            <td class="itemSpacingHoriz">
            </td>
            <td class="controls">
                <div style="margin: 5px 9px 5px 5px;">
                    <asp:TextBox runat="server" ID="_TB_Password" TextMode="Password" Width="100%" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="_TB_Password"
                        Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="RegisterUserValidationGroup"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing">
            </td>
        </tr>
        <tr>
            <td class="contentItemTitle50 contentItem">
                <asp:Label runat="server" ID="Label3" Text="Повтор пароля" />
            </td>
            <td class="itemSpacingHoriz">
            </td>
            <td class="controls">
                <div style="margin: 5px 9px 5px 5px;">
                    <asp:TextBox runat="server" ID="_TB_Password1" TextMode="Password" Width="100%" />
                    <asp:RequiredFieldValidator ControlToValidate="_TB_Password1" CssClass="failureNotification"
                        Display="Dynamic" ErrorMessage="Поле ''Подтвердите пароль'' является обязательным."
                        ID="ConfirmPasswordRequired" runat="server" ToolTip="Поле ''Подтвердите пароль'' является обязательным."
                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="_TB_Password"
                        ControlToValidate="_TB_Password1" CssClass="failureNotification" Display="Dynamic"
                        ErrorMessage="Значения ''Пароль'' и ''Подтвердите пароль'' должны совпадать."
                        ValidationGroup="RegisterUserValidationGroup">Пароли должны совпадать</asp:CompareValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing">
            </td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing">
            </td>
        </tr>
        <tr>
            <td colspan="3" class="registration_table" align="center" valign="middle">
                <asp:Button runat="server" ID="_BTN_Submit" Text="Зарегистрироваться" CssClass="_AM_Label"
                    ForeColor="Black" ValidationGroup="RegisterUserValidationGroup"/>
            </td>
        </tr>
    </table>
</asp:Content>
