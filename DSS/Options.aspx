<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Options.aspx.cs" Inherits="DSS.DSS.Options" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Настройки
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Настройки
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="_PNL_TaskDDL">
    <table class="contentItems" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td colspan="3" class="text_header1">
                Смена задачи
            </td>
        </tr>
        <tr>
            <td class="contentItemTitle50 contentItem">
                <asp:Label runat="server" ID="_LBL_Name" Text="Текущая задача" />
            </td>
            <td class="itemSpacingHoriz"></td>
            <td class="controls">
                <div style="margin: 5px 5px 5px 5px;">
                    <asp:DropDownList runat="server" ID="_DDL_Task" Width="100%" DataTextField="name" DataValueField="id" AutoPostBack="true" />
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>
</asp:Content>
