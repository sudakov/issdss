using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DSS.DSS.Classes;
using System.IO;

namespace DSS.DSS
{
    public partial class EditJob : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_plan_job_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.QueryString["jid"] != null)
                        Command.Parameters.AddWithValue("JobID", Context.Request.QueryString["jid"].ToString());
                    if (Context.Request.QueryString["pid"] != null)
                        Command.Parameters.AddWithValue("PlanID", Context.Request.QueryString["pid"].ToString());
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        DateTime sd,ed;
                        if (Reader["begin_date"] != DBNull.Value)
                        {
                            sd = Convert.ToDateTime(Reader["begin_date"]);
                            _CAL_StartDate.SelectedDate = sd;
                            _CAL_StartDate.VisibleDate = sd;
                        }
                        else
                        {
                            _CAL_StartDate.SelectedDate = Convert.ToDateTime(null);
                            _CAL_StartDate.VisibleDate = DateTime.Today;
                        }
                        if (Reader["end_date"] != DBNull.Value)
                        {
                            ed = Convert.ToDateTime(Reader["end_date"]);
                            _CAL_EndDate.SelectedDate = ed;
                            _CAL_EndDate.VisibleDate = ed;
                        }
                        else
                        {
                            _CAL_EndDate.SelectedDate = Convert.ToDateTime(null);
                            _CAL_EndDate.VisibleDate = DateTime.Today;
                        }
                        _TB_Duration.Text = Convert.ToInt32(Reader["duration"]).ToString();
                        _TB_MeasureID.Text = Reader["measure_id"].ToString();
                    }
                }
                _BTN_Save.Visible = false;
            }

            _CAL_StartDate.SelectionChanged += new EventHandler(_CAL_StartDate_SelectionChanged);
            _CAL_EndDate.SelectionChanged += new EventHandler(_CAL_EndDate_SelectionChanged);
            _BTN_Save.Click += new EventHandler(_BTN_Save_Click);
            _BTN_Delete.Click += new EventHandler(_BTN_Delete_Click);
        }

        void _BTN_Delete_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_plan_job_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.QueryString["jid"] != null)
                    Command.Parameters.AddWithValue("JobID", Context.Request.QueryString["jid"].ToString());
                if (Context.Request.QueryString["pid"] != null)
                    Command.Parameters.AddWithValue("PlanID", Context.Request.QueryString["pid"].ToString());
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            _CAL_StartDate.SelectedDate = Convert.ToDateTime(null);
            _CAL_EndDate.SelectedDate = Convert.ToDateTime(null);
            _BTN_Save.Visible = false;
        }

        void _CAL_EndDate_SelectionChanged(object sender, EventArgs e)
        {
            _BTN_Save.Visible = true;
            switch (_TB_MeasureID.Text)
            {
                case "1": _CAL_StartDate.SelectedDate = _CAL_EndDate.SelectedDate.AddYears(-Convert.ToInt32(_TB_Duration.Text)); _CAL_StartDate.VisibleDate = _CAL_StartDate.SelectedDate; break;
                case "2": _CAL_StartDate.SelectedDate = _CAL_EndDate.SelectedDate.AddMonths(-3 * Convert.ToInt32(_TB_Duration.Text)); _CAL_StartDate.VisibleDate = _CAL_StartDate.SelectedDate; break;
                case "3": _CAL_StartDate.SelectedDate = _CAL_EndDate.SelectedDate.AddMonths(-Convert.ToInt32(_TB_Duration.Text)); _CAL_StartDate.VisibleDate = _CAL_StartDate.SelectedDate; break;
                case "4": _CAL_StartDate.SelectedDate = _CAL_EndDate.SelectedDate.AddDays(-Convert.ToInt32(_TB_Duration.Text)); _CAL_StartDate.VisibleDate = _CAL_StartDate.SelectedDate; break;
            }
        }

        void _CAL_StartDate_SelectionChanged(object sender, EventArgs e)
        {
            _BTN_Save.Visible = true;
            switch (_TB_MeasureID.Text)
            {
                case "1": _CAL_EndDate.SelectedDate = _CAL_StartDate.SelectedDate.AddYears(Convert.ToInt32(_TB_Duration.Text)); _CAL_EndDate.VisibleDate = _CAL_EndDate.SelectedDate; break;
                case "2": _CAL_EndDate.SelectedDate = _CAL_StartDate.SelectedDate.AddMonths(3 * Convert.ToInt32(_TB_Duration.Text)); _CAL_EndDate.VisibleDate = _CAL_EndDate.SelectedDate; break;
                case "3": _CAL_EndDate.SelectedDate = _CAL_StartDate.SelectedDate.AddMonths(Convert.ToInt32(_TB_Duration.Text)); _CAL_EndDate.VisibleDate = _CAL_EndDate.SelectedDate; break;
                case "4": _CAL_EndDate.SelectedDate = _CAL_StartDate.SelectedDate.AddDays(Convert.ToInt32(_TB_Duration.Text)); _CAL_EndDate.VisibleDate = _CAL_EndDate.SelectedDate; break;
            }
        }

        void _BTN_Save_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_plan_job_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.QueryString["jid"] != null)
                    Command.Parameters.AddWithValue("JobID", Context.Request.QueryString["jid"].ToString());
                if (Context.Request.QueryString["pid"] != null)
                    Command.Parameters.AddWithValue("PlanID", Context.Request.QueryString["pid"].ToString());
                Command.Parameters.AddWithValue("StartDate", _CAL_StartDate.SelectedDate);
                Command.Parameters.AddWithValue("EndDate", _CAL_EndDate.SelectedDate);
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            _BTN_Save.Visible = false;
        }
    }
}