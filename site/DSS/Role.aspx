<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="DSS.DSS.Role" %>

<script type="text/C#" runat="server">
    string Try2Str(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Редактор ролей
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Редактор ролей
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>




    <asp:UpdatePanel ID="_UP_Editor" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="_PNL_Editor" CssClass="parent">
	        <table style="height: 100%; width: 100%;" cellpadding="0" cellspacing="0" >
	        <tr align="center"><td>
            <div class="window">
            <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="3" class="text_header1">
                        Параметры роли
                    </td>
                </tr>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label10" Text="Наименование" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_ID" Visible="false" />
                            <asp:TextBox runat="server" ID="_TB_Name" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td align="right" valign="middle">
                        <asp:Button runat="server" ID="_BTN_Add" Text="Добавить" CssClass="_AM_Label" ForeColor="Black" />
                        <asp:Button runat="server" ID="_BTN_Update" Text="Сохранить" CssClass="_AM_Label" ForeColor="Black" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td align="left" valign="middle">
                        <asp:Button runat="server" ID="_BTN_Cancel" Text="Отмена" CssClass="_AM_Label" ForeColor="Black" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
            </table>
            </div>
	        </td></tr>
	        </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>




    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td style="width:50px;">
            </td>
            <td class="text_header1">
                Роли
            </td>
            <td style="width:50px;">
                <asp:ImageButton ID="_IMGBTN_Add" runat="server" ImageUrl="Images/add.gif" AlternateText="Добавить роль" CssClass="imgInGrid" Visible="false" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:UpdatePanel ID="_UP_GridView" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:SqlDataSource ID="_DS_DataSource" runat="server" ConnectionString="<%$ ConnectionStrings:DSSConnectionString %>" SelectCommand="dbo.issdss_role_Read_All" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:CookieParameter CookieName="TaskID" Name="TaskID" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:GridView ID="_GV_MainGridView" runat="server" DataSourceID="_DS_DataSource" CssClass="contentItem"
                            CellPadding="3" GridLines="None" AlternatingRowStyle-CssClass="contentTableCellAlt"
                            HeaderStyle-CssClass="contentTableTitle" HeaderStyle-Font-Names="Tahoma" HeaderStyle-Font-Size="11px"
                            RowStyle-CssClass="contentTableCell" RowStyle-Font-Names="Tahoma" RowStyle-Font-Size="11px"
                            AllowSorting="true" AutoGenerateColumns="false" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Наименование" SortExpression="Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <label style="font-weight:bold;"><%# Try2Str(Eval("name")) %></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Наименование" SortExpression="Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="_LB_Name" CommandName="EditItem" CommandArgument='<%# Try2Str(Eval("id")) %>'><%# Try2Str(Eval("name")) %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Пользователи" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="_IMGBTN_Person" ImageUrl="Images/edit.png" CommandName="Person" CommandArgument='<%# Try2Str(Eval("id")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Разрешения" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="_IMGBTN_Permission" ImageUrl="Images/edit.png" CommandName="Permission" CommandArgument='<%# Try2Str(Eval("id")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Удалить" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div onclick="return confirm('Удалить роль?');" style="width:1px; margin:auto 0px;">
                                            <asp:ImageButton runat="server" ID="_IMGBTN_Del" ImageUrl="Images/del.gif" CommandName="DeleteItem" CommandArgument='<%# Try2Str(Eval("id")) %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
