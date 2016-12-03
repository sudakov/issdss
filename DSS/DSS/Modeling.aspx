<%@ Page Language="C#" MasterPageFile="~/DSS/Site.Master" AutoEventWireup="true" CodeBehind="Modeling.aspx.cs" Inherits="DSS.DSS.Modeling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Моделирование
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    Моделирование
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <span style="font-family:Tahoma; font-size:13px;">
        <p>На данной странице отображаются результаты прогона имитационной модели: строки состояния ИМ в моменты наступления основных модельных событий, критерий оценки результатов моделирования.</p>
        <p>Для запуска прогона ИМ необходимо пройти в раздел <a href="Alternative.aspx">"Альтернативы"</a>, выбрать альтернативу из спика и нажать на кнопку "Запустить прогон модели". По истечении модельного времени результаты немедленно отобразятся на этой странице.</p>
        <p>_______________________________________________________________________________________________________________________________</p>
    </span>

    <span style="font-family:Consolas; font-size:15px;">
        <asp:Label ID="_LBL_Run_Trace" Visible="true" runat="server" Text="Ни одна из моделей не была запущена.">
        </asp:Label>
        <span style="font-family:Tahoma; font-size:20px; color:darkblue;">
            <asp:Label ID="_LBL_Up" Visible="false" runat="server" Text="<a href=#>Наверх</a>.">
            </asp:Label>
        </span>
    </span>
</asp:Content>