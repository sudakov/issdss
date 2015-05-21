<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Plan.aspx.cs" Inherits="DSS.DSS.Plan" %>

<script type="text/C#" runat="server">
    string Try2Str(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
    }

    string Try2YesNo(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "") || (Obj.ToString().Trim() == "0")) ? "Нет" : "Да";
    }

    string Try2Date(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : ((DateTime)Convert.ToDateTime(Obj)).ToShortDateString();
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
    Планирование программы
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Планирование программы
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
                        Параметры плана
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
                        <asp:Label runat="server" ID="Label2" Text="Метод планирования" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 5px 5px 5px;">
                            <asp:DropDownList runat="server" ID="_DDL_Method" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label4" Text="Дата начала" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_BeginDate" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label5" Text="Дата окончания" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_EndDate" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label3" Text="Единица измерения точности алгоритма планирования" />
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
                        <asp:Label runat="server" ID="Label1" Text="Готов" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px;">
                            <asp:CheckBox runat="server" ID="_CHBX_IsReady" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label6" Text="Приоритет" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_Alfa" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>

                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label8" Text="Резерв ресурсов" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_ReservPercent" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="contentItemTitle30 contentItem" style="width:100%; text-align:center; padding:9px;">
                        <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="width:20px;">
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label7" Text="Предшественники" />
                                </td>
                                <td style="width:20px;">
                                    <asp:ImageButton runat="server" ID="_IMGBTN_ParentAdd" ImageUrl="Images/add.png" AlternateText="Добавить предшественника" CssClass="imgInGrid" ToolTip="Добавить предшественника" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>

                <asp:Panel runat="server" ID="_PNL_ParentIsEmpty" Visible="false">
                <tr>
                    <td colspan="3" class="controls" style="width:100%; text-align:center; padding:9px;">
                        <asp:Label runat="server" ID="Label9" Text="Планы, предшествующие текущему, отсутствуют"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                </asp:Panel>
                            
                <asp:Panel runat="server" ID="_PNL_ParentIsNotEmpty" Visible="false">
                <tr>
                    <td colspan="3" style="width:100%; text-align:center; padding:0px;">

                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="_GV_ParentGridView" runat="server" CssClass="contentItem"
                                    CellPadding="3" GridLines="None" AlternatingRowStyle-CssClass="contentTableCellAlt"
                                    HeaderStyle-CssClass="contentTableTitle" HeaderStyle-Font-Names="Tahoma" HeaderStyle-Font-Size="11px"
                                    RowStyle-CssClass="contentTableCell" RowStyle-Font-Names="Tahoma" RowStyle-Font-Size="11px"
                                    AllowSorting="true" AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Наименование" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label><%# Try2Str(Eval("name")) %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Удалить" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div onclick="return confirm('Удалить предшествующий план?');" style="width:1px; margin:auto 0px;">
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
                </asp:Panel>

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




    <asp:UpdatePanel ID="_UP_ParentEditor" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="_PNL_ParentEditor" CssClass="parent">
	        <table style="height: 100%; width: 100%;" cellpadding="0" cellspacing="0" >
	        <tr align="center"><td>
            <div class="window" style="border-color:Black; width:400px;">
            <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="3" class="text_header1">
                        Предшествующий план
                    </td>
                </tr>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label11" Text="Наименование" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 5px 5px 5px;">
                            <asp:DropDownList runat="server" ID="_DDL_Parent" Width="100%" />
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
                        <asp:Button runat="server" ID="_BTN_ParentAdd" Text="Добавить" CssClass="_AM_Label" ForeColor="Black" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td align="left" valign="middle">
                        <asp:Button runat="server" ID="_BTN_ParentCancel" Text="Отмена" CssClass="_AM_Label" ForeColor="Black" />
                    </td>
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
                Планы
            </td>
            <td style="width:50px;">
                <asp:ImageButton ID="_IMGBTN_Add" runat="server" ImageUrl="Images/add.gif" AlternateText="Добавить план" CssClass="imgInGrid" Visible="false" ToolTip="Добавить план" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:UpdatePanel ID="_UP_GridView" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:SqlDataSource ID="_DS_DataSource" runat="server" ConnectionString="<%$ ConnectionStrings:DSSConnectionString %>" SelectCommand="dbo.issdss_plandss_Read_All" SelectCommandType="StoredProcedure">
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
                                <asp:TemplateField HeaderText="Расчет" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div onclick="return confirm('Произвести расчет плана? Процедура может занять некоторое время.');" style="width:1px; margin:auto 0px;">
                                            <asp:ImageButton runat="server" ID="_IMGBTN_Planing" ImageUrl="Images/play.png" CommandName="Plan" CommandArgument='<%# Try2Str(Eval("id")) %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Диаграмма Ганта" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div onclick="window.open('Gantt.aspx?id=<%# Try2Str(Eval("id")) %>');" style="width:1px; margin:auto 0px;">
                                            <asp:ImageButton runat="server" ID="_IMGBTN_Gantt" ImageUrl="Images/gantt.png" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Метод планирования" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Method" Text='<%# Try2Str(Eval("method")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Дата начала" SortExpression="begin_date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_BeginDate" Text='<%# Try2Date(Try2Str(Eval("begin_date"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Дата окончания" SortExpression="end_date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_EndDate" Text='<%# Try2Date(Try2Str(Eval("end_date"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Единица измерения точности" SortExpression="measure" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Measure" Text='<%# Try2Str(Eval("measure")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Готов" SortExpression="isready" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_IsReady" Text='<%# Try2YesNo(Eval("isready")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Приоритет" SortExpression="alfa" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Alfa" Text='<%# MyMathRound(Try2Str(Eval("alfa"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Значение целевой функции" SortExpression="func_value" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_FuncValue" Text='<%# MyMathRound(Try2Str(Eval("func_value"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Резерв ресурсов" SortExpression="reserv_percent" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_ReservPercent" Text='<%# MyMathRound(Try2Str(Eval("reserv_percent"))) %>' />
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
