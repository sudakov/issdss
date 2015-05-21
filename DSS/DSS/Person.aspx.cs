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
    public partial class Person : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Разрешения
            int id, pagePermission = 0;
            _GV_MainGridView.Columns[0].Visible = true;
            _GV_MainGridView.Columns[1].Visible = false;
            _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 4].Visible = false;
            _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 3].Visible = false;
            _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 2].Visible = false;
            _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 1].Visible = false;
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
                        case 51: pagePermission = id; break;
                        case 52: _GV_MainGridView.Columns[0].Visible = false; _GV_MainGridView.Columns[1].Visible = true; break;
                        case 53: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 4].Visible = true; break;
                        case 55: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 3].Visible = true; break;
                        case 57: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 2].Visible = true; break;
                        case 59: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 1].Visible = true; break;
                    }
                }
                if (pagePermission == 0)
                    Response.Redirect("Default.aspx");
            }
            
            _GV_MainGridView.RowCommand += new GridViewCommandEventHandler(_GV_MainGridView_RowCommand);
        }

        void _GV_MainGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditItem")
            {
                Response.Redirect("PersonInfo.aspx?id=" + ID.ToString());
            }

            if (e.CommandName == "Task")
            {
                Response.Redirect("PersonTask.aspx?id=" + ID.ToString());
            }

            if (e.CommandName == "Alterntive")
            {
                Response.Redirect("PersonAlternative.aspx?id=" + ID.ToString());
            }

            if (e.CommandName == "Role")
            {
                Response.Redirect("PersonRole.aspx?id=" + ID.ToString());
            }

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_person_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("PersonID", ID);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                _GV_MainGridView.DataBind();
                _UP_GridView.Update();
            }
        }
    }
}