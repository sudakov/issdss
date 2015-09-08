<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DSS/Site.Master" CodeBehind="Fuzzy.aspx.cs" Inherits="DSS.DSS.Fuzzy" %>

<%@ Register TagPrefix="uc" TagName="MembershipFunctionControl" Src="Controls/MembershipFunctionControl.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    <%= DefaultText %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <%= DefaultText %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc:MembershipFunctionControl runat="server" ID="mfControl"></uc:MembershipFunctionControl>
</asp:Content>