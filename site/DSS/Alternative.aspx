<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Alternative.aspx.cs" Inherits="DSS.DSS.Alternative" %>

<script type="text/C#" runat="server">
    bool Try2Bool(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "") || (Obj.ToString().Trim() == "0")) ? false : true;
    }

    string Try2Str(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
    }

    string Try2Int(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "0" : Obj.ToString();
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
    Редактор альтернатив
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Редактор альтернатив
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>




    <asp:UpdatePanel ID="_UP_Editor" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="_PNL_Editor" CssClass="parent">
	        <table style="height: 100%; width: 100%;" cellpadding="0" cellspacing="0" >
	        <tr align="center"><td>
            <div class="window" style="width:800px;">
            <table class="contentItems" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="3" class="text_header1">
                        Параметры альтернативы
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
                    <td colspan="3" class="contentItemTitle30 contentItem" style="width:100%; text-align:center; padding:9px;">
                        <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="width:20px;">
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label4" Text="Работы" />
                                </td>
                                <td style="width:20px;">
                                    <asp:ImageButton runat="server" ID="_IMGBTN_JobAdd" ImageUrl="Images/add.png" AlternateText="Добавить работу" CssClass="imgInGrid" ToolTip="Добавить работу" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>

                <asp:Panel runat="server" ID="_PNL_JobIsEmpty" Visible="false">
                <tr>
                    <td colspan="3" class="controls" style="width:100%; text-align:center; padding:9px;">
                        <asp:Label runat="server" ID="_LBL_JobIsEmpty" Text="Не добавлено ни одной работы"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                </asp:Panel>
                            
                <asp:Panel runat="server" ID="_PNL_JobIsNotEmpty" Visible="false">
                <tr>
                    <td colspan="3" style="width:100%; text-align:center; padding:0px;">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="_GV_JobGridView" runat="server" CssClass="contentItem"
                                    CellPadding="3" GridLines="None" AlternatingRowStyle-CssClass="contentTableCellAlt"
                                    HeaderStyle-CssClass="contentTableTitle" HeaderStyle-Font-Names="Tahoma" HeaderStyle-Font-Size="11px"
                                    RowStyle-CssClass="contentTableCell" RowStyle-Font-Names="Tahoma" RowStyle-Font-Size="11px"
                                    AllowSorting="true" AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="№" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="_LB_Ord" Text='<%# Try2Str(Eval("ord")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Наименование" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="_LB_Name" CommandName="EditItem" CommandArgument='<%# Try2Str(Eval("id")) %>'><%# Try2Str(Eval("name")) %></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Продолжительность" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label><%# MyMathRound(Try2Str(Eval("duration"))) %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Единица измерения продолжительности" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <label><%# Try2Str(Eval("m_name")) %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Предшественники" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label><%# Try2Str(Eval("parent_jobs"))%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Удалить" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div onclick="return confirm('Удалить работу?');" style="width:1px; margin:auto 0px;">
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

            </table>
            </div>
	        </td></tr>
	        </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>




    <asp:UpdatePanel ID="_UP_JobEditor" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="_PNL_JobEditor" CssClass="parent" DefaultButton="_BTN_JobCancel">
	        <table style="height: 100%; width: 100%;" cellpadding="0" cellspacing="0" >
	        <tr align="center"><td>
            <div class="window" style="border-color:#333333;">
            <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="3" class="text_header1">
                        Параметры работы
                    </td>
                </tr>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label12" Text="Порядковый номер" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_JobOrd" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label1" Text="Наименование" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_JobID" Visible="false" />
                            <asp:TextBox runat="server" ID="_TB_JobName" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label2" Text="Продолжительность" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_Duration" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label3" Text="Единица измерения продолжительности" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 5px 5px 5px;">
                            <asp:DropDownList runat="server" ID="_DDL_Measure" Width="100%" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td colspan="3" class="contentItemTitle30 contentItem" style="width:100%; text-align:center; padding:9px;">
                        <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="width:20px;">
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Label5" Text="Предшественники" />
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
                        <asp:Label runat="server" ID="Label6" Text="Работы, предшествующие текущей, отсутствуют"></asp:Label>
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
                                                <div onclick="return confirm('Удалить предшествующую работу?');" style="width:1px; margin:auto 0px;">
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

                <asp:Panel runat="server" ID="_PNL_Resource" Visible="false">
                <tr>
                    <td colspan="3" class="text_header1">
                        Ресурсы работы
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="_RP_Resource">
                    <ItemTemplate>
                        <tr>
                            <td class="contentItemTitle50 contentItem">
                                <asp:Label runat="server" ID="_LBL_ID" Text='<%# Eval("id") %>' Visible="false" />
                                <asp:Label runat="server" ID="_LBL_Name" Text='<%# Eval("name") %>' />
                            </td>
                            <td class="itemSpacingHoriz"></td>
                            <td class="controls">
                                <div style="margin: 5px 9px 5px 5px;">
                                    <asp:TextBox runat="server" ID="_TB_Value" Width="100%" Text='<%# MyMathRound(Try2Int(Eval("value"))) %>' Enabled='<%# Try2Bool(Eval("is_enabled")) %>' />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="itemSpacing"></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </asp:Panel>

                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td align="right" valign="middle">
                        <asp:Button runat="server" ID="_BTN_JobAdd" Text="Добавить" CssClass="_AM_Label" ForeColor="Black" />
                        <asp:Button runat="server" ID="_BTN_JobUpdate" Text="Сохранить" CssClass="_AM_Label" ForeColor="Black" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td align="left" valign="middle">
                        <asp:Button runat="server" ID="_BTN_JobCancel" Text="Отмена" CssClass="_AM_Label" ForeColor="Black" />
                    </td>
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
                        Предшествующая работа
                    </td>
                </tr>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label7" Text="Наименование" />
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
                Альтернативы
            </td>
            <td style="width:50px;">
                <asp:ImageButton ID="_IMGBTN_Add" runat="server" ImageUrl="Images/add.gif" AlternateText="Добавить альтернативу" CssClass="imgInGrid" Visible="false" ToolTip="Добавить альтернативу" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:UpdatePanel ID="_UP_GridView" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:SqlDataSource ID="_DS_DataSource" runat="server" ConnectionString="<%$ ConnectionStrings:DSSConnectionString %>" SelectCommand="dbo.issdss_alternative_Read_All" SelectCommandType="StoredProcedure">
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
                                        <asp:LinkButton runat="server" ID="_LB_Name" CommandName="EditItem" CommandArgument='<%# Try2Str(Eval("id")) %>'><%# Try2Str(Eval("name")) %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ранг" SortExpression="rank" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="_LB_Rank" Text='<%# Try2Str(Eval("rank")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Работы" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <label><%# Try2Str(Eval("jobs"))%></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Пользователи" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="_IMGBTN_Person" ImageUrl="Images/edit.png" CommandName="Person" CommandArgument='<%# Try2Str(Eval("id")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Удалить" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div onclick="return confirm('Удалить альтернативу?');" style="width:1px; margin:auto 0px;">
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
