using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DSS.DSS.Classes;
using DSS.DSS.FuzzyModel;

namespace DSS.DSS
{
    public partial class FuzzyRank : Page
    {
        private double bestResult;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = CreateAlts();
            InitAlts(dt);
            gvAlts.DataSource = dt;
            gvAlts.DataBind();
        }

        private static void InitAlts(DataTable dt)
        {
            for (int i = 0; i < Core.U.GetLength(0); i++)
            {
                var row = dt.NewRow();
                for (int j = 0; j < Core.U.GetLength(1); j++)
                {
                    row[j] = Core.U[i, j];
                }
                dt.Rows.Add(row);
            }
        }

        private static DataTable CreateAlts()
        {
            DataTable dt = new DataTable();
            foreach (string s in Core.Criteria)
            {
                dt.Columns.Add(s, typeof (double));
            }
            return dt;
        }

        protected void RunFuzzyRanking(object sender, EventArgs e)
        {
            int iterCount = int.Parse(tbIterCount.Text);
            var fuzzyRankResult = Core.Compute(Core.U, iterCount);

            bestResult = fuzzyRankResult.Best;
            
            gvResult.DataSource = fuzzyRankResult.All.Select((x, i) => new
                                                                       {
                                                                           Name = "Alt #" + (i + 1),
                                                                           Value = x.ToString("F5")
                                                                       });
            gvResult.DataBind();

            lblResult.Text = "По результатам анализа введенных данных оптимальной следует считать альтернативу #" +
                             (Array.IndexOf(fuzzyRankResult.All, bestResult) + 1);
            divResult.Visible = true;
        }

        protected void gvResult_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
            double d = double.Parse(e.Row.Cells[1].Text);
            if (Math.Abs(d - bestResult) < 1E-5)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.BackColor = Color.MediumSeaGreen;
                }
            }

        }
    }
}