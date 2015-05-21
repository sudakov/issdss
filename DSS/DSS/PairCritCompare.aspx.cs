using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSS.DSS
{
    public partial class PairCritCompare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _BTN_Save.Click += new EventHandler(_BTN_Save_Click);
            _BTN_Cancel.Click += new EventHandler(_BTN_Cancel_Click);
        }

        void _BTN_Save_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_pair_crit_comp_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                string criteriaXML = "<crit_value>";
                for (int i = 0; i < _RP_Main.Items.Count; i++)
                {
                    criteriaXML += "<row>";
                    criteriaXML += "<criteria1_id>" + ((Label)_RP_Main.Items[i].FindControl("_LBL_ID1")).Text + "</criteria1_id>";
                    criteriaXML += "<value>" + ((TextBox)_RP_Main.Items[i].FindControl("_TB_Value")).Text + "</value>";
                    criteriaXML += "<criteria2_id>" + ((Label)_RP_Main.Items[i].FindControl("_LBL_ID2")).Text + "</criteria2_id>";
                    criteriaXML += "</row>";
                }
                criteriaXML += "</crit_value>";
                Command.Parameters.AddWithValue("@d", criteriaXML);
                if (Context.Request.QueryString["id"] != null)
                    Command.Parameters.AddWithValue("@parent_crit_id", Context.Request.QueryString["id"]);
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            string s = String.Empty;
            if (Context.Request.QueryString["id"] != null)
                s = "?id=" + Context.Request.QueryString["id"];
            else
                s = "";

            Response.Redirect("Criteria.aspx" + s);
        }

        void _BTN_Cancel_Click(object sender, EventArgs e)
        {
            string s = String.Empty;
            if (Context.Request.QueryString["id"] != null)
                s = "?id=" + Context.Request.QueryString["id"];
            else
                s = "";

            Response.Redirect("Criteria.aspx" + s);
        }
    }
}