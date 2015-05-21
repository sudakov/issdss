<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Person.aspx.cs" Inherits="DSS.DSS.Person" %>

<script type="text/C#" runat="server">
    string Try2Str(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Информация о пользователях
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Информация о пользователях
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td colspan="3" class="text_header1">
                Пользователи
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:UpdatePanel ID="_UP_GridView" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:SqlDataSource ID="_DS_DataSource" runat="server" ConnectionString="<%$ ConnectionStrings:DSSConnectionString %>" SelectCommand="dbo.issdss_person_Read_All" SelectCommandType="StoredProcedure">
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
                                <asp:TemplateField HeaderText="Имя пользователя" SortExpression="Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <label style="font-weight:bold;"><%# Try2Str(Eval("name")) %></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Имя пользователя" SortExpression="Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="_LB_Name" CommandName="EditItem" CommandArgument='<%# Try2Str(Eval("id")) %>'><%# Try2Str(Eval("name")) %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Логин" SortExpression="rank" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Rank" Text='<%# Try2Str(Eval("login")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Задачи" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a class='link_into_grid' href='PersonTask.aspx?id=<%# Try2Str(Eval("id")) %>'><%# Try2Str(Eval("tasks")) %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Альтернативы" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="_IMGBTN_Alternative" ImageUrl="Images/edit.png" CommandName="Alterntive" CommandArgument='<%# Try2Str(Eval("id")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Роли" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a class='link_into_grid' href='PersonRole.aspx?id=<%# Try2Str(Eval("id")) %>'><%# Try2Str(Eval("roles")) %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Удалить" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div onclick="return confirm('Удалить пользователя?');" style="width:1px; margin:auto 0px;">
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
