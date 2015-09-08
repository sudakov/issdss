<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Value.aspx.cs" Inherits="DSS.DSS.Value" %>

<script type="text/C#" runat="server">
    string Try2Str(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Экспертные оценки
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Экспертные оценки
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/ValueTable.js" type="text/javascript"></script>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr id="_TR_Head">
            <td colspan="3" class="text_header1">
                Экспертные оценки
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td id="_TD_Name" class="_TBL_HeadCell">
                            Наименование
                        </td>
                        <td>
                            <div id="_DIV_Head" style="overflow-x:hidden; overflow-y:scroll;">
                                <div id="_DIV_SubHead" style="position:reluctive;">
                                    <asp:Table ID="_TBL_Head" runat="server" CellSpacing="0"></asp:Table>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="_DIV_Left" style="overflow-x:scroll; overflow-y:hidden;">
                                <div id="_DIV_SubLeft" style="position:reluctive;">
                                    <table id="_TBL_Left" cellspacing="0" width="100%">
                                        <asp:Panel runat="server" ID="_PNL_NotEditValue">
                                            <asp:Repeater ID="_RP_Left_NotEdit" runat="server">
                                                <ItemTemplate>
                                                    <tr><td class="_TBL_MainCell">
                                                        <label style="font-weight:bold;"><%# Try2Str(Eval("name")) %></label>
                                                    </td></tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr><td class="_TBL_MainCellAlt">
                                                        <label style="font-weight:bold;"><%# Try2Str(Eval("nucme")) %></label>
                                                    </td></tr>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="_PNL_EditValue">
                                        <asp:Repeater ID="_RP_Left_Edit" runat="server">
                                            <ItemTemplate>
                                                <tr><td class="_TBL_MainCell">
                                                    <a href='Assessment.ucspx?id=<%# Try2Str(Eval("id")) %>' class="labelInGrid"><%# Try2Str(Eval("nucme")) %></a>
                                                </td></tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr><td class="_TBL_MainCellAlt">
                                                    <a href='Assessment.ucspx?id=<%# Try2Str(Eval("id")) %>' class="labelInGrid"><%# Try2Str(Eval("nucme")) %></a>
                                                </td></tr>
                                            </AlternatingItemTemplate>
                                        </asp:Repeater>
                                        </asp:Panel>
                                    </table>
                                </div>
                            </div>
                        </td>
                        <td valign="top">
                            <div id="_DIV_Main" style="overflow:scroll; position:reluctive;">
                                <div id="_DIV_SubMain" style="position:reluctive;">
                                    <asp:Table ID="_TBL_Main" runat="server" CellSpacing="0"></asp:Table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>                        
            </td>
        </tr>
    </table>
</asp:Content>
