<%@ Page Title="" Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="FuzzyRank.aspx.cs" Inherits="DSS.DSS.FuzzyRank" %>
<%@ Import Namespace="DSS.DSS.FuzzyModel" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Нечеткое ранжирование
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Нечеткое ранжирование
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:HiddenField runat="server" ID="hfHeaders"/>--%>
    <div>
        <asp:Label runat="server" Text="Точность"></asp:Label>
        <br/>
        <asp:TextBox ID="tbIterCount" runat="server" Text="11"/>
        <br/>
        <br/>
        <asp:Label runat="server" Text="Сравнительная таблица альтернатив"></asp:Label>
        <asp:CompareValidator ID="cv" runat="server" ControlToValidate="tbIterCount" Type="Integer" Operator="GreaterThan" ErrorMessage="Значение должно быть числом больше 1" ValueToCompare="1" />
        <asp:GridView runat="server" ID="gvAlts" AutoGenerateColumns="True"></asp:GridView>
        <br/>
        <asp:Button runat="server" OnClick="RunFuzzyRanking" Text="Рассчитать"/>
    </div>
    <br/>
    <br/>
    <div runat="server" id="divResult" Visible="False">
        <asp:Label runat="server" Text="Результаты анализа"></asp:Label>
        <asp:GridView runat="server" ID="gvResult" AutoGenerateColumns="True" OnRowDataBound="gvResult_OnRowDataBound"></asp:GridView>
        <br/>
        <asp:Label runat="server" ID="lblResult" Font-Size="14"></asp:Label>
    </div>
</asp:Content>