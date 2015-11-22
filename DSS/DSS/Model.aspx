<%@ Page Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Model.aspx.cs" Inherits="DSS.DSS.Model" %>

<script type="text/C#" runat="server">
    string Try2Str(object Obj)
    {
        return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Модели
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Список моделей
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <html xmlns="http://www.w3.org/1999/xhtml" >
    

<head>
    <title> Список моделей </title>
</head>
<body>

   <form id="form1">

      
       <table width="100%" cellpadding="0" cellspacing="0" border="0">
           <tr>
               <td style="width:50px;">
               </td>
               <td class="text_header1">
                   Список моделей
               </td>
               <td style="width:50px;">
                   <asp:ImageButton ID="_IMG_RunModel" runat="server" ImageUrl="Images/range.png" AlternateText="Запустить прогон модели" ToolTip="Запустить прогон модели" Visible="false" />
               </td>
           </tr>



           <tr>
               <td style="height:5px;"></td>
           </tr>
       
         
       
       </table>
       
       <asp:UpdatePanel ID="_UP_GridView_Model" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:SqlDataSource ID="_DS_DataSource_Model" runat="server" ConnectionString="<%$ ConnectionStrings:DSSConnectionString %>" SelectCommand="dbo.issdss_model_Read_All" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:CookieParameter CookieName="UserID" Name="UserID" />
                        <asp:CookieParameter CookieName="TaskID" Name="TaskID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:GridView ID="_GV_MainGridView_Model" runat="server" DataSourceID="_DS_DataSource_Model" CssClass="contentItem"
                    CellPadding="3" GridLines="None" AlternatingRowStyle-CssClass="contentTableCellAlt"
                    HeaderStyle-CssClass="contentTableTitle" HeaderStyle-Font-Names="Tahoma" HeaderStyle-Font-Size="11px"
                    RowStyle-CssClass="contentTableCell" RowStyle-Font-Names="Tahoma" RowStyle-Font-Size="11px"
                    AllowSorting="true" AutoGenerateColumns="false" Width="100%">
                    <Columns>

                        <asp:TemplateField HeaderText="Название" SortExpression="Name" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <label style="font-weight:bold;"><%# Try2Str(Eval("name")) %></label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Описание" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <label style="font-weight:normal;"><%# Try2Str(Eval("description")) %></label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
       </asp:UpdatePanel>
               

        

   </form>

</body>
</html>

</asp:Content>
