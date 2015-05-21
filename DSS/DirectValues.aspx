<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="DirectValues.aspx.cs" Inherits="DSS.DSS.DirectValues" %>

<script type="text/C#" runat="server">
    double Try2Int(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? 0.0 : Convert.ToDouble(Obj);
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Взвешенная сумма
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Взвешенная сумма
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td colspan="3" class="text_header1">
                Ввод непосредственных значений
            </td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing"></td>
        </tr>
        <tr>
            <td colspan="3" class="itemSpacing"></td>
        </tr>
        <asp:Repeater runat="server" ID="_RP_Main">
            <ItemTemplate>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label ID="_LBL_ID" runat="server" Text='<%# Eval("id") %>' Visible="false" />
                        <asp:Label ID="_LBL_Name" runat="server" Text='<%# Eval("name") %>' />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox ID="_TB_" runat="server" Width="100%" Text='<%# Try2Int(Eval("rank")) %>' />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td colspan="3" class="itemSpacing"></td>
        </tr>
        <tr>
            <td align="right" valign="middle">
                <asp:Button runat="server" ID="_BTN_Save" Text="Сохранить" CssClass="_AM_Label" ForeColor="Black" />
            </td>
            <td class="itemSpacingHoriz"></td>
            <td class="registration_table" align="left" valign="middle">
                <asp:Button runat="server" ID="_BTN_Cancel" Text="Отмена" CssClass="_AM_Label" ForeColor="Black" />
            </td>
        </tr>
    </table>
</asp:Content>
