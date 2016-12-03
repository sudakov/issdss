using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSS.DSS
{
    public partial class Modeling : System.Web.UI.Page
    {
        private static string traceString = String.Empty;

        public static string TraceString
        {
            get { return traceString; }
            set { traceString = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Alternative.RunStart)
            {
                _LBL_Run_Trace.Text = TraceString;
                _LBL_Up.Visible = true;
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_alternative_Update_Trace", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("TraceText", TraceString);
                    Command.Parameters.AddWithValue("AlternativeID", Alternative.AlternativeId);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
            }
        }
    }
}