<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="PairCritCompare.aspx.cs" Inherits="DSS.DSS.PairCritCompare" %>

<script type="text/C#" runat="server">
    string Try2Str(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
    }

    int Try2Int(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? 50 : Convert.ToInt32(Obj);
    }

    //string MyMathRound(string s)
    //{
    //    if (s.Contains(","))
    //        s = s.Replace(',', '.');
        
    //    if ((s.Contains(".") || s.Contains(",")) && (s[s.Length - 1].Equals('0') || s[s.Length - 1].Equals('.') || s[s.Length - 1].Equals(',')))
    //    {
    //        s = s.Remove(s.Length - 1);
    //        return MyMathRound(s);
    //    }
    //    else
    //        return s;
    //}
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Попарное сравнение критериев
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Попарное сравнение критериев
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script src="Scripts/Slider.js" type="text/javascript"></script>

    <asp:SqlDataSource ID="_DS_PairCritComp" runat="server" ConnectionString="<%$ ConnectionStrings:DSSConnectionString %>" SelectCommand="dbo.issdss_pair_crit_comp_Read" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter
                ConvertEmptyStringToNull="True"
                DefaultValue=""
                Direction="Input"
                Name="parent_crit_id"
                QueryStringField="id"
                Type="Int32"
            />
        </SelectParameters>
    </asp:SqlDataSource>

    <input id="_INPUT_ID" style="display:none;" />

    <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td colspan="5" class="text_header1">
                Зависимость критериев
            </td>
        </tr>
        <asp:Repeater ID="_RP_Main" runat="server" DataSourceID="_DS_PairCritComp">
            <ItemTemplate>
                <tr>
                    <td class="contentItemTitle50 contentItem" style="width:25%;">
                        <asp:Label runat="server" ID="_LBL_ID1" Text='<%# Eval("criteria1_id") %>' Visible="false" />
                        <asp:Label runat="server" ID="_LBL_Name1" Text='<%# Try2Str(Eval("criteria1_name")) %>' />
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="controls">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="width:50px; padding:5px 2px 5px 5px;">
                                    <asp:TextBox runat="server" ID="_TB_Value" Text='<%# Try2Int(Eval("rank")) %>' Width="50px" CssClass="centerText"></asp:TextBox>
                                </td>
                                <td style="width:15px; padding-right:10px;">
                                    <img id="_IMG_Percent" src="/DSS/Images/Slider/percent.png" alt="%" style="cursor:default; width:15px;" />
                                </td>
                                <td style="background-image:url(/DSS/Images/Slider/line.png); background-repeat:repeat-x; background-position:center;">
                                    <div id="_DIV_Pointer" style="position:relative;">
                                        <img id="_IMG_Pointer_<%# Try2Str(Eval("number")) %>" src="/DSS/Images/Slider/pointer.png" alt="Pointer" style="cursor:pointer;" onmouseover="document.getElementById('_INPUT_ID').value = this.id.replace('_IMG_Pointer_', '') - 1;" />
                                    </div>
                                </td>
                                <td style="width:30px; padding-left:10px;">
                                    <img src="/DSS/Images/Slider/left.png" alt="0%" style="cursor:pointer;" title="Установить на 0%" onclick="SetValue(<%# Try2Str(Eval("number")) %> - 1, 0);" />
                                </td>
                                <td style="width:30px; padding-left:2px;">
                                    <img src="/DSS/Images/Slider/center.png" alt="50%" style="cursor:pointer;" title="Центрировать" onclick="SetValue(<%# Try2Str(Eval("number")) %> - 1, 50);" />
                                </td>
                                <td style="width:30px; padding:5px 5px 5px 2px;">
                                    <img src="/DSS/Images/Slider/right.png" alt="100%" style="cursor:pointer;" title="Установить на 100%" onclick="SetValue(<%# Try2Str(Eval("number")) %> - 1, 100);" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="itemSpacingHoriz"></td>
                    <td class="contentItemTitle50 contentItem" style="width:25%; text-align:left;">
                        <asp:Label runat="server" ID="_LBL_ID2" Text='<%# Eval("criteria2_id") %>' Visible="false" />
                        <asp:Label runat="server" ID="_LBL_Name2" Text='<%# Try2Str(Eval("criteria2_name")) %>' />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" class="itemSpacing"></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td colspan="5" class="text_header1">
                <asp:Button runat="server" ID="_BTN_Save" Text="Сохранить" CssClass="_AM_Label" ForeColor="Black" />
                <asp:Button runat="server" ID="_BTN_Cancel" Text="Отмена" CssClass="_AM_Label" ForeColor="Black" />
            </td>
        </tr>
    </table>

</asp:Content>
