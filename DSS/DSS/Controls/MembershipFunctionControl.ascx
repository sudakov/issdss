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
                    animation: false,
                    ignoreHiddenSeries: false
                },

                credits: false,

                title: {
                    text: '<%# FunctionName %>'
                },

                xAxis: {
                    categories: ['0-10', '10-20', '20-30', '30-40', '40-50', '50-60', '60+']
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

                yAxis: { min: 0, max: 1 },

                series: [
                    {
                        showInLegend: false,
                        data: [null, null, null, null, null, null, null],
                        draggableY: true,
                        dragMinY: 0,
                        dragMaxY: 1,
                        name: "Hidden",
                        visible: false
                    }
                ]

            });
        });

        function AddSeries() {
            var tbSeries = document.getElementById('<%= tbSeries.ClientID %>');
            chart.addSeries({
                data: [0, 0, 0, 0, 0, 0, 0],
                draggableY: true,
                dragMinY: 0,
                dragMaxY: 1,
                name: tbSeries.value,
                visible: true
            });
        }

        function Save() {
            var savedSeries = JSON.stringify(chart.series.map(function (s) {
                var data = s.data.map(function (a) {
                    return a.y;
                });
                var result = {};
                result[s.name] = data;
                return result;
            }));

            var hfData = document.getElementById('<%= hfData.ClientID %>');
            hfData.value = savedSeries;
        }
    </script>

    <div id="container" style="height: 400px"></div>
    <div>
        <asp:Label runat="server" Text="Имя нового параметра"></asp:Label>
        <asp:TextBox runat="server" ID="tbSeries"></asp:TextBox>
        <input type="button" onclick="AddSeries()" value="Добавить" />
    </div>
    <div>
        <input type="button" onclick="Save()" value="Save" />
        <asp:HiddenField runat="server" ID="hfData" />
    </div>
</div>
