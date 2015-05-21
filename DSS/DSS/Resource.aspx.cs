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

namespace DSS.DSS
{
    public partial class Resource : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Разрешения
            int id, pagePermission = 0;
            _GV_MainGridView.Columns[0].Visible = true;
            _GV_MainGridView.Columns[1].Visible = false;
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
                        case 91: pagePermission = id; break;
                        case 92: _IMGBTN_Add.Visible = true; break;
                        case 93: _GV_MainGridView.Columns[0].Visible = false; _GV_MainGridView.Columns[1].Visible = true; break;
                        case 94: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 1].Visible = true; break;
                    }
                }
                if (pagePermission == 0)
                    Response.Redirect("Default.aspx");
            }

            if (!IsPostBack)
            {
                // Заполнение ДропДаунЛиста мер
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_measure_Read_All", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    _DDL_Measure.Items.Clear();
                    _DDL_Measure.Items.Add(new ListItem("", "0"));
                    while (Reader.Read())
                        _DDL_Measure.Items.Add(new ListItem(Reader["name"].ToString(), Reader["id"].ToString()));
                }
                _DDL_Measure.SelectedValue = "0";

                // Заполнение ДропДаунЛиста критериев
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Read_All", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.Cookies["TaskID"] != null)
                        Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    _DDL_Criteria.Items.Clear();
                    _DDL_Criteria.Items.Add(new ListItem("", "0"));
                    while (Reader.Read())
                        _DDL_Criteria.Items.Add(new ListItem(Reader["name"].ToString(), Reader["id"].ToString()));
                }
                _DDL_Criteria.SelectedValue = "0";
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
                    SqlCommand Command = new SqlCommand("dbo.issdss_resource_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("ResourceID", ID);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        _TB_ID.Text = Reader["ID"].ToString();
                        _TB_Name.Text = Reader["Name"].ToString();
                        _TB_Description.Text = Reader["Description"].ToString();
                        _TB_Value.Text = Helper.MyMath.Round(Reader["Value"].ToString());
                        _TB_Period.Text = Reader["Period"].ToString();
                        if (Reader["Measure_id"].ToString() != "")
                            _DDL_Measure.SelectedValue = Reader["Measure_id"].ToString();
                        else
                            _DDL_Measure.SelectedValue = "0";
                        if (Reader["Criteria_id"].ToString() != "")
                            _DDL_Criteria.SelectedValue = Reader["Criteria_id"].ToString();
                        else
                            _DDL_Criteria.SelectedValue = "0";
                    }
                }
                _PNL_Editor.Visible = true;
                _BTN_Add.Visible = false;
                _BTN_Update.Visible = true;
                _UP_Editor.Update();
            }

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_resource_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("ResourceID", ID);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                _PNL_Editor.Visible = false;
                _BTN_Add.Visible = true;
                _BTN_Update.Visible = true;
                _TB_ID.Text = "";
                _TB_Name.Text = "";
                _TB_Description.Text = "";
                _TB_Value.Text = "";
                _TB_Period.Text = "";
                _DDL_Measure.SelectedValue = "0";
                _DDL_Criteria.SelectedValue = "0";
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
            _TB_Description.Text = "";
            _TB_Value.Text = "";
            _TB_Period.Text = "";
            _DDL_Measure.SelectedValue = "0";
            _DDL_Criteria.SelectedValue = "0";
            _UP_Editor.Update();
        }

        void _BTN_Add_Click(object sender, EventArgs e)
        {
            if (Context.Request.Cookies["TaskID"] != null)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_resource_Insert", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                    Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                    Command.Parameters.AddWithValue("Description", _TB_Description.Text);
                    Command.Parameters.AddWithValue("Value", _TB_Value.Text);
                    Command.Parameters.AddWithValue("Period", _TB_Period.Text);
                    if (_DDL_Measure.SelectedValue != "0")
                        Command.Parameters.AddWithValue("MeasureID", _DDL_Measure.SelectedValue);
                    if (_DDL_Criteria.SelectedValue != "0")
                        Command.Parameters.AddWithValue("CriteriaID", _DDL_Criteria.SelectedValue);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                _PNL_Editor.Visible = false;
                _BTN_Add.Visible = true;
                _BTN_Update.Visible = true;
                _TB_ID.Text = "";
                _TB_Name.Text = "";
                _TB_Description.Text = "";
                _TB_Value.Text = "";
                _TB_Period.Text = "";
                _DDL_Measure.SelectedValue = "0";
                _DDL_Criteria.SelectedValue = "0";
                _GV_MainGridView.DataBind();
                _UP_GridView.Update();
            }
            else
            {
                Response.Redirect("Resource.aspx");
            }
        }

        void _BTN_Update_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_resource_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("ResourceID", _TB_ID.Text);
                Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                Command.Parameters.AddWithValue("Description", _TB_Description.Text);
                Command.Parameters.AddWithValue("Value", Convert.ToDecimal(_TB_Value.Text));
                Command.Parameters.AddWithValue("Period", _TB_Period.Text);
                if (_DDL_Measure.SelectedValue != "0")
                    Command.Parameters.AddWithValue("MeasureID", _DDL_Measure.SelectedValue);
                if (_DDL_Criteria.SelectedValue != "0")
                    Command.Parameters.AddWithValue("CriteriaID", _DDL_Criteria.SelectedValue);
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            _PNL_Editor.Visible = false;
            _BTN_Add.Visible = true;
            _BTN_Update.Visible = true;
            _TB_ID.Text = "";
            _TB_Name.Text = "";
            _TB_Description.Text = "";
            _TB_Value.Text = "";
            _TB_Period.Text = "";
            _DDL_Measure.SelectedValue = "0";
            _DDL_Criteria.SelectedValue = "0";
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
            _TB_Description.Text = "";
            _TB_Value.Text = "";
            _TB_Period.Text = "";
            _DDL_Measure.SelectedValue = "0";
            _DDL_Criteria.SelectedValue = "0";
            _UP_Editor.Update();
        }
    }
}