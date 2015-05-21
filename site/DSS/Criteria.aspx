<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true"
    CodeBehind="Criteria.aspx.cs" Inherits="DSS.DSS.Criteria1" %>

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
    Редактор критериев
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Редактор критериев
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>




    <asp:UpdatePanel ID="_UP_Editor" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="_PNL_Editor" CssClass="parent" DefaultButton="_BTN_Cancel">
	        <table style="height: 100%; width: 100%;" cellpadding="0" cellspacing="0" >
	        <tr align="center"><td>
            <div class="window">
            <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="3" class="text_header1">
                        Параметры критерия
                    </td>
                </tr>
                <tr>
                    <td class="contentItemTitle50 contentItem">
                        <asp:Label runat="server" ID="Label1" Text="Наименование" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_Name" Width="100%" Enabled="false" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>
                <tr>
                    <td class="contentItemTitle30 contentItem" style="vertical-align:top; padding-top:10px;">
                        <asp:Label runat="server" ID="Label2" Text="Описание" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_Description" Width="100%" TextMode="MultiLine" Rows="5" Enabled="false" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>
                <tr>
                    <td class="contentItemTitle30 contentItem">
                        <asp:Label runat="server" ID="Label3" Text="Родитель" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 5px 5px 5px;">
                            <asp:DropDownList runat="server" ID="_DDL_Parent" Width="100%" Enabled="false" />
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
                                    <asp:Label runat="server" ID="Label4" Text="Шкала" />
                                </td>
                                <td style="width:20px;">
                                    <asp:ImageButton runat="server" ID="_IMGBTN_ScaleAdd" ImageUrl="Images/add.png" AlternateText="Добавить градацию шкалы" CssClass="imgInGrid" Visible="false" ToolTip="Добавить градацию шкалы" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing">
                    </td>
                </tr>


                <asp:Panel runat="server" ID="_PNL_ScaleIsEmpty" Visible="false">
                <tr>
                    <td colspan="3" class="controls" style="width:100%; text-align:center; padding:9px;">
                        <asp:Label runat="server" ID="_LBL_ScaleIsEmpty" Text="Шкала не заполнена"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                </asp:Panel>
                            
                <asp:Panel runat="server" ID="_PNL_ScaleIsNotEmpty" Visible="false">
                <tr>
                    <td colspan="3" style="width:100%; text-align:center; padding:0px;">

                        <asp:UpdatePanel ID="_UP_GridView" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="_GV_ScaleGridView" runat="server" CssClass="contentItem"
                                    CellPadding="3" GridLines="None" AlternatingRowStyle-CssClass="contentTableCellAlt"
                                    HeaderStyle-CssClass="contentTableTitle" HeaderStyle-Font-Names="Tahoma" HeaderStyle-Font-Size="11px"
                                    RowStyle-CssClass="contentTableCell" RowStyle-Font-Names="Tahoma" RowStyle-Font-Size="11px"
                                    AllowSorting="true" AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="№" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="_LB_Ord" Text='<%# Try2Str(Eval("ord")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Текст" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label style="font-weight:bold;"><%# Try2Str(Eval("name")) %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Текст" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="_LB_Name" CommandName="EditItem" CommandArgument='<%# Try2Str(Eval("id")) %>'><%# Try2Str(Eval("name")) %></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ранг" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="_LB_Rank" Text='<%# MyMathRound(Try2Str(Eval("rank"))) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Удалить" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div onclick="return confirm('Удалить градацию шкалы?');" style="width:1px; margin:auto 0px;">
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
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                </asp:Panel>

                <asp:Panel runat="server" ID="_PNL_Simple" Visible="false">
                <tr>
                    <td class="contentItemTitle30 contentItem">
                        <asp:Label runat="server" ID="Label5" Text="Направление улучшений" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 5px 5px 5px;">
                            <asp:DropDownList runat="server" ID="_DDL_ImprovementDirection" AutoPostBack="true" Width="100%" Enabled="false">
                                <asp:ListItem Text="Чем больше, тем лучше" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Чем меньше, тем лучше" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Идеальное значение" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <asp:Panel runat="server" ID="_PNL_IdealValue" Visible="false">
                <tr>
                    <td class="contentItemTitle30 contentItem">
                        <asp:Label runat="server" ID="Label8" Text="Идеальное значение" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_IdealValue" Width="100%" Enabled="false" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                </asp:Panel>
                </asp:Panel>

                <asp:Panel runat="server" ID="_PNL_Integrated" Visible="false">
                <tr>
                    <td class="contentItemTitle30 contentItem">
                        <asp:Label runat="server" ID="Label6" Text="Метод агрегирования" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <div style="margin: 5px 0px 5px 5px;">
                                        <asp:DropDownList runat="server" ID="_DDL_AggregationMethod" AutoPostBack="true" Width="100%" Enabled="false" />
                                        <asp:DropDownList runat="server" ID="_DDL_AggregationMethodUrl" Visible="false" />
                                    </div>
                                </td>
                                <td style="width:40px; text-align:center;">
                                    <asp:HyperLink runat="server" ID="_HL_EditMethod" ImageUrl="Images/edit.png" CssClass="imgInGrid" ToolTip="Редактировать метод" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                </asp:Panel>

                <tr>
                    <td class="contentItemTitle30 contentItem">
                        <asp:Label runat="server" ID="Label7" Text="Числовой показатель" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px;">
                            <asp:CheckBox runat="server" ID="_CHBX_IsNumber" Enabled="false" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="itemSpacing"></td>
                </tr>
                <tr>
                    <td class="contentItemTitle30 contentItem">
                        <asp:Label runat="server" ID="Label9" Text="Порядковый номер" />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <div style="margin: 5px 9px 5px 5px;">
                            <asp:TextBox runat="server" ID="_TB_Ord" Width="100%" Enabled="false" />
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
                    <td colspan="3">
                        <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                            <td align="right" valign="middle" style="width:50%;">
                                <asp:Button runat="server" ID="_BTN_Add" Text="Добавить" CssClass="_AM_Label" ForeColor="Black" />
                                <asp:Button runat="server" ID="_BTN_Update" Text="Сохранить" CssClass="_AM_Label" ForeColor="Black" />
                            </td>
                            <td class="itemSpacingHoriz"></td>
                            <td align="left" valign="middle">
                                <asp:Button runat="server" ID="_BTN_Cancel" Text="Отмена" CssClass="_AM_Label" ForeColor="Black" />
                            </td>
                            <asp:Panel runat="server" ID="_PNL_CriteriaDelete" Visible="false">
                            <td align="right" valign="middle" style="width:1px;">
                                <div onclick="return confirm('Удалить критерий?');">
                                    <asp:ImageButton runat="server" ID="_IMGBTN_Delete" ImageUrl="Images/del50.gif" AlternateText="Удалить критерий" CssClass="imgInGrid" ToolTip="Удалить критерий" />
                                </div>
                            </td>
                            </asp:Panel>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </div>
	        </td></tr>
	        </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>




    <asp:UpdatePanel ID="_UP_ScaleEditor" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="_PNL_ScaleEditor" CssClass="parent" DefaultButton="_BTN_ScaleCancel">
	        <table style="height: 100%; width: 100%;" cellpadding="0" cellspacing="0" >
	        <tr align="center"><td>
            <div class="window" style="border-color:#333333; width:400px;">
                <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3" class="text_header1">
                            Параметры пункта шкалы оценки
                        </td>
                    </tr>
                    <tr>
                        <td class="contentItemTitle50 contentItem" style="background-color:#ADC2CC;">
                            <asp:Label runat="server" ID="Label12" Text="Порядковый номер" />
                        </td>
                        <td class="itemSpacingHoriz"></td>
                        <td class="controls" style="background-color:#DBDBDB;">
                            <div style="margin: 5px 9px 5px 5px;">
                                <asp:TextBox runat="server" ID="_TB_ScaleOrd" Width="100%" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="itemSpacing"></td>
                    </tr>
                    <tr>
                        <td class="contentItemTitle50 contentItem" style="background-color:#ADC2CC;">
                            <asp:Label runat="server" ID="Label10" Text="Текст" />
                        </td>
                        <td class="itemSpacingHoriz"></td>
                        <td class="controls" style="background-color:#DBDBDB;">
                            <div style="margin: 5px 9px 5px 5px;">
                                <asp:TextBox runat="server" ID="_TB_ScaleID" Visible="false" />
                                <asp:TextBox runat="server" ID="_TB_ScaleName" Width="100%" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="itemSpacing"></td>
                    </tr>
                    <tr>
                        <td class="contentItemTitle50 contentItem" style="background-color:#ADC2CC;">
                            <asp:Label runat="server" ID="Label11" Text="Ранг" />
                        </td>
                        <td class="itemSpacingHoriz"></td>
                        <td class="controls" style="background-color:#DBDBDB;">
                            <div style="margin: 5px 9px 5px 5px;">
                                <asp:TextBox runat="server" ID="_TB_ScaleRank" Width="100%" />
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
                            <asp:Button runat="server" ID="_BTN_ScaleAdd" Text="Добавить" CssClass="_AM_Label" ForeColor="Black" />
                            <asp:Button runat="server" ID="_BTN_ScaleUpdate" Text="Сохранить" CssClass="_AM_Label" ForeColor="Black" />
                        </td>
                        <td class="itemSpacingHoriz"></td>
                        <td align="left" valign="middle">
                            <asp:Button runat="server" ID="_BTN_ScaleCancel" Text="Отмена" CssClass="_AM_Label" ForeColor="Black" />
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
                Дерево критериев
            </td>
            <td style="width:50px;">
                <asp:ImageButton ID="_IMGBTN_Add" runat="server" ImageUrl="Images/add.gif" AlternateText="Добавить критерий" CssClass="imgInGrid" Visible="false" ToolTip="Добавить критерий" />
            </td>
        </tr>
        <tr>
            <td colspan="3" class="controls" style="vertical-align: top; text-align: left">
                <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" Width="100%">
                    <HoverNodeStyle Font-Underline="False" BorderColor="Maroon" BorderStyle="Solid" BorderWidth="1px" />
                    <NodeStyle BorderColor="#FFFFFF" BorderStyle="Solid" BorderWidth="1px" Font-Names="Tahoma"
                        Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="2px"
                        VerticalPadding="2px" />
                    <ParentNodeStyle Font-Bold="False" />
                    <SelectedNodeStyle Font-Underline="False" HorizontalPadding="5px" VerticalPadding="2px"
                        ForeColor="White" NodeSpacing="2px" BackColor="#3366CC" BorderStyle="Solid" BorderWidth="1px"
                        BorderColor="Black" />
                </asp:TreeView>
            </td>
        </tr>
    </table>
</asp:Content>
