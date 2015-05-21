<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Resource.aspx.cs" Inherits="DSS.DSS.Resource" %>

<script type="text/C#" runat="server">
    string Try2Str(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
    }

    string MyMathRound(string s)
    {
        if ((s.Contains(".") || s.Contains(",")) && (s[s.Length - 1].Equals('0') || s[s.Length - 1].Equals('.') || s[s.Length - 1].Equals(',')))
        {
            s = s.Remove(s.Length - 1);
            return MyMathRound(s);
        }
        else
            return s;
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Редактор ресурсов
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Редактор ресурсов
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
                        Параметры ресурса
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
                    <td class="contentItemTitle50 contentItem" style="vertical-align:top; padding-top:10px;">
                        <asp:Label runat="server" ID="Label2" Text="Описание" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_Description" Width="100%" TextMode="MultiLine" Rows="5" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label4" Text="Доступная величина" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_Value" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label5" Text="Период возобновления" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_Period" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label3" Text="Единица изменения периода" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 5px 5px 5px;">
                            <asp:DropDownList runat="server" ID="_DDL_Measure" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label1" Text="Критерий" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 5px 5px 5px;">
                            <asp:DropDownList runat="server" ID="_DDL_Criteria" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
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
                Ресурсы
            </td>
            <td style="width:50px;">
                <asp:ImageButton ID="_IMGBTN_Add" runat="server" ImageUrl="Images/add.gif" AlternateText="Добавить ресурс" CssClass="imgInGrid" Visible="false" ToolTip="Добавить ресурс" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:UpdatePanel ID="_UP_GridView" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:SqlDataSource ID="_DS_DataSource" runat="server" ConnectionString="<%$ ConnectionStrings:DSSConnectionString %>" SelectCommand="dbo.issdss_resource_Read_All" SelectCommandType="StoredProcedure">
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
                                <asp:TemplateField HeaderText="Описание" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Description" Text='<%# Try2Str(Eval("description")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Значение" SortExpression="value" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Value" Text='<%# MyMathRound(Try2Str(Eval("value"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Период" SortExpression="period" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Period" Text='<%# Try2Str(Eval("period")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Единица изменения периода" SortExpression="m_name" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Measure" Text='<%# Try2Str(Eval("m_name")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Критерий" SortExpression="c_name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Criteria" Text='<%# Try2Str(Eval("c_name")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Удалить" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div onclick="return confirm('Удалить ресурс?');" style="width:1px; margin:auto 0px;">
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