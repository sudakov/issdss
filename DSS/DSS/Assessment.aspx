<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Assessment.aspx.cs" Inherits="DSS.DSS.Assessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Экспертиза КЭ
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <asp:Label runat="server" ID="_TB_Header"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td colspan="3" class="text_header1">
                Критерии оценки
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
                    <asp:Panel runat="server" ID="_PNL_Group">
                        <td colspan="3" class="contentItemTitle50 contentItem" style="background-color:#ADC2CC; text-align:left; height:30px;">
                            <div style='padding-left: <%# Eval("lev") %>px; cursor:default;'>
                                <asp:Label runat="server" ID="_LBL_GroupName" Text='<%# Eval("c_name") %>' ToolTip='<%# Eval("description") %>' />
                            </div>
                        </td>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="_PNL_Criteria">
                        <td class="contentItemTitle50 contentItem" style="text-align: left;">
                            <div style='padding-left: <%# Eval("lev") %>px; cursor:default;'>
                                <asp:Label runat="server" ID="_LBL_ID" Text='<%# Eval("c_id") %>' Visible="false" />
                                <asp:Label runat="server" ID="_LBL_IsResourse" Text='<%# Eval("is_resourse") %>' Visible="false" />
                                <asp:Label runat="server" ID="_LBL_Name" Text='<%# Eval("c_name") %>' ToolTip='<%# Eval("description") %>' />
                                <asp:Label runat="server" ID="_LBL_IsParent" Text='<%# Eval("is_parent") %>' Visible="false" />
                            </div>
                        </td>
                        <td class="itemSpacingHoriz"></td>
                        <td class="controls">
                            <div id="Div1" runat="server" style="margin: 5px 5px 5px 5px;">
                                <asp:Label runat="server" ID="_LBL_DDL_SelectedID" Text='<%# Eval("current_scale_id") %>' Visible="false" />
                                <asp:DropDownList runat="server" ID="_DDL_Value" Width="100%"
                                    DataSource='<%# Eval("ddl_data_source") %>'
                                    DataValueField='<%# Eval("ddl_data_source","id") %>'
                                    DataTextField='<%# Eval("ddl_data_source","name") %>' />
                            </div>
                            <div style="margin: 5px 9px 5px 5px;">
                                <asp:TextBox runat="server" ID="_TB_Value" Width="100%" Text='<%# Eval("value_for_view") %>' />
                            </div>
                        </td>
                    </asp:Panel>
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
                <asp:Table runat="server" ID="_TBL_Main" CellPadding="0" CellSpacing="0" BorderWidth="0" CssClass="contentItems"></asp:Table>
</asp:Content>
