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
    public partial class PersonRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id, pagePermission = 0, editPermission = 0;
                _BTN_Save.Visible = false;
                _BTN_Cancel.Visible = false;
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    // Разрешения
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
                            case 57: pagePermission = id; break;
                            case 58: editPermission = id; _BTN_Save.Visible = true; _BTN_Cancel.Visible = true; break;
                        }
                    }
                    if (pagePermission == 0)
                        Response.Redirect("Default.aspx");
                    Reader.Close();

                    // Основная загрузка данных
                    Command = new SqlCommand("dbo.issdss_person_role_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.Cookies["TaskID"] != null)
                        Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                    if (Context.Request.QueryString["id"] != null)
                        Command.Parameters.AddWithValue("@PersonID", Context.Request.QueryString["id"]);
                    Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        if (Reader["name"].ToString() != "")
                            _TB_Header.Text = "Пользователь: " + Reader["name"].ToString();
                        else
                            Response.Redirect("Person.aspx");
                    }
                    else
                        Response.Redirect("Person.aspx");
                    _RP_Main.DataSource = Reader;
                    _RP_Main.DataBind();
                }
                // Сделать репитер недоступным для редактирования
                if (editPermission == 0)
                    for (int i = 0; i < _RP_Main.Items.Count; i++)
                        ((CheckBox)_RP_Main.Items[i].FindControl("_CHBX_")).Enabled = false;
            }

            _BTN_Save.Click += new EventHandler(_BTN_Save_Click);
            _BTN_Cancel.Click += new EventHandler(_BTN_Cancel_Click);
        }

        void _BTN_Save_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_person_role_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                Command.Parameters.AddWithValue("@PersonID", Context.Request.QueryString["id"]);
                string s = ",";
                for (int i = 0; i < _RP_Main.Items.Count; i++)
                    if (((CheckBox)_RP_Main.Items[i].FindControl("_CHBX_")).Checked)
                        s += ((Label)_RP_Main.Items[i].FindControl("_LBL_ID")).Text + ",";
                Command.Parameters.AddWithValue("@Role_ids", s);
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            Response.Redirect("Person.aspx");
        }

        void _BTN_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Person.aspx");
        }
    }
}