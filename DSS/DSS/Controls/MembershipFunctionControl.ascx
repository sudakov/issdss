<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipFunctionControl.ascx.cs" Inherits="DSS.DSS.Controls.MembershipFunctionControl" %>
<div>
    <script type="text/javascript" src="Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/highcharts.js"></script>
    <script type="text/javascript" src="Scripts/modules/exporting.js"></script>
    <script type="text/javascript" src="Scripts/draggable-points.js"></script>
    <script type="text/javascript">
        var chart;
        $(function () {
            chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'container',
                    animation: false
                },

                credits: false, 

                title: {
                    text: '<%# FunctionName %>'
                },

                xAxis: {
                    categories: [<%# Categories %>]
                },

                plotOptions: {
                    column: {
                        stacking: 'normal'
                    },
                    line: {
                        cursor: 'ns-resize'
                    }
                },

                tooltip: {
                    pointFormat: "Принадлежность: {point.y:.3f}"
                },

                yAxis: { min: 0, max: <%# MaxValue %> },

                series: [
                    {
                        data: [<%# Data %>],
                        draggableY: true,
                        dragMinY: 0,
                        dragMaxY: <%# MaxValue %>,
                        name: '<%# CriteriaName %>'
                    }
                ]

            });
        });
        
        function Save() {
            var savedSeries = JSON.stringify(chart.series[0].data.map(function (a) {
                return a.y;
            }));

            var hfData = document.getElementById('<%= hfData.ClientID %>');
            hfData.value = savedSeries;
        }
    </script>

    <div id="container" style="height: 400px"></div>
    <div>
        <asp:Button runat="server" OnClientClick="Save()" OnClick="SaveOnClick" Text="Сохранить значения"/>
        <asp:HiddenField runat="server" ID="hfData" />
        <asp:HiddenField runat="server" ID="hfKeys"/>
    </div>
</div>
