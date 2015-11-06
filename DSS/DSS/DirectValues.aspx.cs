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
    public partial class DirectValues : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.QueryString["id"] != null)
                        Command.Parameters.AddWithValue("@ParentID", Context.Request.QueryString["id"]);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    _RP_Main.DataSource = Reader;
                    _RP_Main.DataBind();
                }
            }

            _BTN_Save.Click += new EventHandler(_BTN_Save_Click);
            _BTN_Cancel.Click += new EventHandler(_BTN_Cancel_Click);
        }

        void _BTN_Save_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command;
                Connection.Open();
                for (int i = 0; i < _RP_Main.Items.Count; i++)
                {
                    Command = new SqlCommand("dbo.issdss_criteria_Update_Rank", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@CriteriaID", ((Label)_RP_Main.Items[i].FindControl("_LBL_ID")).Text);
                    try
                    {
                        Command.Parameters.AddWithValue("@Rank", Convert.ToDouble(((TextBox)_RP_Main.Items[i].FindControl("_TB_")).Text));
                    }
                    catch
                    {
                        Command.Parameters.AddWithValue("@Rank", Convert.ToDouble(((TextBox)_RP_Main.Items[i].FindControl("_TB_")).Text.Replace(".", ",")));
                    }
                    Command.ExecuteNonQuery();
                }
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