<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditJob.aspx.cs" Inherits="DSS.DSS.EditJob" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/Site.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="main_admin">
    <asp:Panel runat="server" ID="_PNL_MainContent" style="border:1px solid #bbc9d7; background-color:White; padding:15px;">
    <div id="clientArea">
    <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="contentItemTitle50 contentItem" style="background-color:#ADC2CC; text-align:center; height:30px;">
                Дата начала
            </td>
            <td class="itemSpacingHoriz">&nbsp;</td>
            <td class="contentItemTitle50 contentItem" style="background-color:#ADC2CC; text-align:center;">
                Дата окончания
            </td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing"></td>
        </tr>
        <tr>
            <td class="controls">
                <div style="margin:  5px 5px 5px 5px;">
                <asp:Calendar runat="server" ID="_CAL_StartDate" Width="160">
                    <DayStyle BackColor="White"></DayStyle>
                    <OtherMonthDayStyle BackColor="White" ForeColor="LightGray"></OtherMonthDayStyle>
                    <SelectedDayStyle BackColor="Blue"></SelectedDayStyle>
                </asp:Calendar>
                </div>
            </td>
            <td class="itemSpacingHoriz">
            </td>
            <td class="controls">
                <div style="margin:  5px 5px 5px 5px;">
                <asp:Calendar runat="server" ID="_CAL_EndDate" Width="160">
                    <DayStyle BackColor="White"></DayStyle>
                    <OtherMonthDayStyle BackColor="White" ForeColor="LightGray"></OtherMonthDayStyle>
                    <SelectedDayStyle BackColor="Blue"></SelectedDayStyle>
                </asp:Calendar>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing"></td>
        </tr>
        <tr>
            <td align="right" valign="middle">
                <asp:Button runat="server" ID="_BTN_Save" Text="Сохранить" CssClass="_AM_Label" ForeColor="Black" />
                <asp:Button runat="server" ID="_BTN_Delete" Text="Удалить даты" CssClass="_AM_Label" ForeColor="Black" />
                <asp:TextBox runat="server" ID="_TB_Duration" Visible="false" />
                <asp:TextBox runat="server" ID="_TB_MeasureID" Visible="false" />
            </td>
            <td class="itemSpacingHoriz"></td>
            <td class="registration_table" align="left" valign="middle">
                <div onclick="window.close();" style="width:1px; margin:auto 0px;">
                    <asp:Button runat="server" ID="_BTN_Cancel" Text="Закрыть окно" CssClass="_AM_Label" ForeColor="Black" />
                </div>
            </td>
        </tr>
    </table>
    </div>
    </asp:Panel>
    </div>
    </form>
</body>
</html>
