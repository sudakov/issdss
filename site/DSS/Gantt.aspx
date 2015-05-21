<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gantt.aspx.cs" Inherits="DSS.DSS.Gantt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        Диаграмма
    </title>
    <link href="Styles/Site.css" type="text/css" rel="stylesheet" />
    <link href="Styles/jsgantt.css" type="text/css" rel="stylesheet" />
    <script src="Scripts/jsgantt.js" type="text/javascript"></script>

    <script type="text/C#" runat="server">
        string Try2Str(object Obj)
        {
            return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "" : Obj.ToString();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="main_admin">
        <table style="width: 100%; left: 0px; top: 0px;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td style="height:60px; background:url(Images/head.png); border:1px solid #bbc9d7; background-repeat:repeat-x;">
                    <table style="width: 100%; height:100%;" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center" valign="middle" style="width:1px;">
                                <img runat="server" ID="_IMG_Header" src="~/DSS/Images/wheels.png" style="padding-left:10px; padding-right:10px;" alt="header" />
                            </td>
                            <td valign="middle">
                                <asp:Label runat="server" ID="Label1" style=" vertical-align:middle; color:#FFFFFF; font-family:Comic Sans MS; font-size:18pt; font-weight:bold; white-space:nowrap;">
                                    <asp:Label runat="server" ID="_TB_Header"></asp:Label>
                                </asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" style="padding-bottom:10px; padding-left:10px; padding-right:10px; padding-top:10px; background:#e5f2fb; border-bottom:1px solid #bbc9d7; border-right:1px solid #bbc9d7;">
                    <asp:Panel runat="server" ID="_PNL_MainContent" style="border:1px solid #bbc9d7; background-color:White; padding:20px;">
                        <div id="clientArea">
                            
                            <div style="display:none;">
                            <table id="_TBL_Main">
                                <asp:Repeater ID="_RP_Main" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Try2Str(Eval("id")) %></td>
                                            <td><%# Try2Str(Eval("name")) %></td>
                                            <td><%# Try2Str(Eval("start_date"))%></td>
                                            <td><%# Try2Str(Eval("end_date"))%></td>
                                            <td><%# Try2Str(Eval("link")) %></td>
                                            <td><%# Try2Str(Eval("group")) %></td>
                                            <td><%# Try2Str(Eval("parent")) %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                            </div>

                            <div style="position:relative" class="gantt" id="GanttChartDIV"></div>

                            <script type="text/javascript">
                                var g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');
                                var r = document.getElementById('_TBL_Main').rows.length;
                                var tableMain = document.getElementById('_TBL_Main')
                                var g;
                                var tableMainRowsI;

                                g.setShowRes(0); // Show/Hide Responsible (0/1)
                                g.setShowDur(1); // Show/Hide Duration (0/1)
                                g.setShowComp(0); // Show/Hide % Complete(0/1)
                                g.setCaptionType('None');  // Set to Show Caption (None,Caption,Resource,Duration,Complete)
                                g.setDateInputFormat('dd/mm/yyyy');
                                g.setDateDisplayFormat('dd/mm/yyyy');

                                if (g) {
                                    for (var i = 0; i < r; i++) {
                                        //alert(tableMain.rows[i].cells[4].innerHTML);
                                        tableMainRowsI = tableMain.rows[i];
                                        g.AddTaskItem(new JSGantt.TaskItem(
                                            tableMainRowsI.cells[0].innerHTML,   // ID
                                            tableMainRowsI.cells[1].innerHTML,   // Name
                                            tableMainRowsI.cells[2].innerHTML,   // Start Date
                                            tableMainRowsI.cells[3].innerHTML,   // End Date
                                            '0000ff',                               // Color
                                            tableMainRowsI.cells[4].innerHTML,   // Link
                                            0,                                      // Show Bar
                                            '',                                     // Resourse Name
                                            0,                                      // Complete %
                                            Number(tableMainRowsI.cells[5].innerHTML),   // Group
                                            tableMainRowsI.cells[6].innerHTML,   // Parent
                                            1                                       // Предшественник
                                            ));
                                    }
                                    g.Draw();
                                    g.DrawDependencies();
                                }

                                else {
                                    alert("not defined");
                                }
                            </script>

                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
