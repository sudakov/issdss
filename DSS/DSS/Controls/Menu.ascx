<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="DSS.DSS.Controls.Menu" %>

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
        <td class="menuSep">
        </td>
    </tr>
    <asp:Panel runat="server" ID="_PNL_DSS">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Default.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Система поддержки принятия решений</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" ID="_PNL_Task">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Task.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Задачи</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" ID="_PNL_Alternative">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Alternative.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Альтернативы</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" ID="_PNL_Criteria">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Criteria.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Критерии</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" ID="_PNL_Resource">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Resource.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Ресурсы</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" id="_PNL_Value">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Value.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Экспертные оценки</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" id="_PNL_Ranking">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Ranking.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Ранжирование</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" id="_PNL_Plan">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Plan.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Планирование</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>
    
    <asp:Panel runat="server" id="_PNL_Fuzzy">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'FuzzyRank.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Нечеткое ранжирование</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" id="_PNL_Person">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Person.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Пользователи</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>

    <asp:Panel runat="server" id="_PNL_Role">
    <tr>
        <td class="menuItem">
            <div class="menuLabel" onclick="window.location.href = 'Role.aspx';">
                <table class="userMenuItem" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="spacing"></td>
                        <td class="point"></td>
                        <td class="content">Роли</td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="menuSep">
        </td>
    </tr>
    </asp:Panel>
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