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
    public partial class Gantt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Разрешения
            int id, pagePermission = 0;
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_permission_Read", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.Cookies["TaskID"] != null)
                    Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                if (Context.Request.Cookies["UserID"] != null)
                    Command.Parameters.AddWithValue("PersonID", Context.Request.Cookies["UserID"].Value);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                Reader.Read(); // Пропускаем первую строчку с именем пользователя
                while (Reader.Read())
                {
                    id = Convert.ToInt32(Reader["permission_id"]);
                    switch (id)
                    {
                        case 71: pagePermission = id; break;
                    }
                }
                if (pagePermission == 0)
                    Response.Redirect("Default.aspx");
            }

            //
            // Контент
            //
            DataTable dt = new DataTable();
            DataRow dr;
            DataColumn dc;
            int currentParent = 0;

            dc = new DataColumn("id", System.Type.GetType("System.Int32"));
            dt.Columns.Add(dc);

            dc = new DataColumn("name", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);

            dc = new DataColumn("start_date", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);

            dc = new DataColumn("end_date", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);

            dc = new DataColumn("link", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);

            dc = new DataColumn("group", System.Type.GetType("System.Int32"));
            dt.Columns.Add(dc);

            dc = new DataColumn("parent", System.Type.GetType("System.Int32"));
            dt.Columns.Add(dc);

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_job_for_Gantt", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.QueryString["id"] != null)
                    Command.Parameters.AddWithValue("PlanID", Context.Request.QueryString["id"].ToString());
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                for (int i = 1; Reader.Read(); i++)
                {
                    dr = dt.NewRow();
                    dr["id"] = i;
                    dr["name"] = Reader["name"].ToString();
                    switch (Reader["type"].ToString())
                    {
                        case "plan":
                            _TB_Header.Text = "Диаграмма Ганта: \"" + Reader["name"].ToString() + "\"";
                            dr["start_date"] = "";
                            dr["end_date"] = "";
                            dr["link"] = "";
                            dr["group"] = 1;
                            dr["parent"] = 0;
                            break;
                        case "alternative":
                            currentParent = i;
                            dr["start_date"] = "";
                            dr["end_date"] = "";
                            dr["link"] = "";
                            dr["group"] = 1;
                            dr["parent"] = 1;
                            break;
                        case "job":
                            if (Reader["start_date"].ToString() != "" && Reader["end_date"].ToString() != "")
                            {
                                DateTime sd = Convert.ToDateTime(Reader["start_date"].ToString());
                                DateTime ed = Convert.ToDateTime(Reader["end_date"].ToString());
                                dr["start_date"] = sd.Day + "/" + sd.Month + "/" + sd.Year;
                                dr["end_date"] = ed.Day + "/" + ed.Month + "/" + ed.Year;
                            }
                            else
                            {
                                dr["start_date"] = "";
                                dr["end_date"] = "";
                            }
                            dr["link"] = "EditJob.aspx?jid=" + Reader["j_id"].ToString() + "&pid=" + Context.Request.QueryString["id"].ToString();
                            dr["group"] = 0;
                            dr["parent"] = currentParent;
                            break;
                    }
                    dt.Rows.Add(dr);
                }
                _RP_Main.DataSource = dt;
                _RP_Main.DataBind();
            }
        }
    }
}