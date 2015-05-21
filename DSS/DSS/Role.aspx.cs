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
    public partial class Role : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Разрешения
            int id, pagePermission = 0;
            _GV_MainGridView.Columns[0].Visible = true;
            _GV_MainGridView.Columns[1].Visible = false;
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
                        case 61: pagePermission = id; break;
                        case 62: _IMGBTN_Add.Visible = true; break;
                        case 63: _GV_MainGridView.Columns[0].Visible = false; _GV_MainGridView.Columns[1].Visible = true; break;
                        case 57: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 3].Visible = true; break;
                        case 64: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 2].Visible = true; break;
                        case 66: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 1].Visible = true; break;
                    }
                }
                if (pagePermission == 0)
                    Response.Redirect("Default.aspx");
            }
            
            _GV_MainGridView.RowCommand += new GridViewCommandEventHandler(_GV_MainGridView_RowCommand);
            _IMGBTN_Add.Click += new ImageClickEventHandler(_IMGBTN_Add_Click);
            _BTN_Add.Click += new EventHandler(_BTN_Add_Click);
            _BTN_Update.Click += new EventHandler(_BTN_Update_Click);
            _BTN_Cancel.Click += new EventHandler(_BTN_Cancel_Click);

            _PNL_Editor.Visible = false;
        }

        void _GV_MainGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_role_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("RoleID", ID);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        _TB_ID.Text = Reader["ID"].ToString();
                        _TB_Name.Text = Reader["Name"].ToString();
                    }
                }
                _PNL_Editor.Visible = true;
                _BTN_Add.Visible = false;
                _BTN_Update.Visible = true;
                _UP_Editor.Update();
            }

            if (e.CommandName == "Person")
            {
                Response.Redirect("RolePerson.aspx?id=" + ID.ToString());
            }

            if (e.CommandName == "Permission")
            {
                Response.Redirect("RolePermission.aspx?id=" + ID.ToString());
            }

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_role_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("RoleID", ID);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                _PNL_Editor.Visible = false;
                _BTN_Add.Visible = true;
                _BTN_Update.Visible = true;
                _TB_ID.Text = "";
                _TB_Name.Text = "";
                _UP_Editor.Update();

                _GV_MainGridView.DataBind();
                _UP_GridView.Update();
            }
        }

        void _IMGBTN_Add_Click(object sender, ImageClickEventArgs e)
        {
            _PNL_Editor.Visible = true;
            _BTN_Add.Visible = true;
            _BTN_Update.Visible = false;
            _TB_ID.Text = "";
            _TB_Name.Text = "";
            _UP_Editor.Update();
        }

        void _BTN_Add_Click(object sender, EventArgs e)
        {
            if (Context.Request.Cookies["TaskID"] != null)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_role_Insert", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                    Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                _PNL_Editor.Visible = false;
                _BTN_Add.Visible = true;
                _BTN_Update.Visible = true;
                _TB_ID.Text = "";
                _TB_Name.Text = "";
                _GV_MainGridView.DataBind();
                _UP_GridView.Update();
            }
            else
            {
                Response.Redirect("Role.aspx");
            }
        }

        void _BTN_Update_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_role_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("RoleID", _TB_ID.Text);
                Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            _PNL_Editor.Visible = false;
            _BTN_Add.Visible = true;
            _BTN_Update.Visible = true;
            _TB_ID.Text = "";
            _TB_Name.Text = "";
            _GV_MainGridView.DataBind();
            _UP_GridView.Update();
        }

        void _BTN_Cancel_Click(object sender, EventArgs e)
        {
            _PNL_Editor.Visible = false;
            _BTN_Add.Visible = true;
            _BTN_Update.Visible = true;
            _TB_ID.Text = "";
            _TB_Name.Text = "";
            _UP_Editor.Update();
        }
    }
}