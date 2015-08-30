<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="FuzzyRank.aspx.cs" Inherits="DSS.DSS.FuzzyRank" %>

<%@ Register TagPrefix="uc" TagName="MembershipFunctionControl" Src="Controls/MembershipFunctionControl.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    <%= DefaultText %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <%= DefaultText %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc:MembershipFunctionControl runat="server"></uc:MembershipFunctionControl>
</asp:Content>