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
    public partial class Options : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _DDL_Task_DataBind();
            _DDL_Task.SelectedIndexChanged += new EventHandler(_DDL_Task_SelectedIndexChanged);
        }

        void _DDL_Task_SelectedIndexChanged(object sender, EventArgs e)
        {
            Context.Response.Cookies.Add(new HttpCookie("TaskID", _DDL_Task.SelectedItem.Value));
            Response.Redirect("Options.aspx");
        }

        void _DDL_Task_DataBind()
        {
            if (!IsPostBack)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_task_Read_All", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.Cookies["TaskID"] != null)
                        Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                    if (Context.Request.Cookies["UserID"] != null)
                        Command.Parameters.AddWithValue("UserID", Context.Request.Cookies["UserID"].Value);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _DDL_Task.DataSource = Reader;
                        _DDL_Task.DataBind();
                        if (Context.Request.Cookies["TaskID"] != null)
                            _DDL_Task.SelectedValue = Context.Request.Cookies["TaskID"].Value;
                        else
                        {
                            Context.Response.Cookies.Add(new HttpCookie("TaskID", _DDL_Task.SelectedItem.Value));
                            Response.Redirect("Options.aspx");
                        }
                    }
                }
                if (_DDL_Task.Items.Count < 2)
                    _PNL_TaskDDL.Visible = false;
            }
        }
    }
}