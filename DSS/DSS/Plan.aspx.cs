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
    public partial class Plan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Разрешения
            int id, pagePermission = 0;
            _IMGBTN_ParentAdd.Visible = false; 
            _GV_MainGridView.Columns[0].Visible = true;
            _GV_MainGridView.Columns[1].Visible = false;
            _GV_MainGridView.Columns[2].Visible = false;
            _GV_MainGridView.Columns[3].Visible = false;
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
                        case 71: pagePermission = id; break;
                        case 72: _IMGBTN_Add.Visible = true; break;
                        case 73: _IMGBTN_ParentAdd.Visible = true; _GV_MainGridView.Columns[0].Visible = false; _GV_MainGridView.Columns[1].Visible = true; _GV_MainGridView.Columns[2].Visible = true; _GV_MainGridView.Columns[3].Visible = true; break;
                        case 74: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 1].Visible = true; break;
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

                // Заполнение ДропДаунЛиста методов
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_plan_method_Read_All", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    _DDL_Method.Items.Clear();
                    _DDL_Method.Items.Add(new ListItem("", "0"));
                    while (Reader.Read())
                        _DDL_Method.Items.Add(new ListItem(Reader["name"].ToString(), Reader["id"].ToString()));
                }
                _DDL_Method.SelectedValue = "0";
            }

            // События в главном окне
            _GV_MainGridView.RowCommand += new GridViewCommandEventHandler(_GV_MainGridView_RowCommand);
            _IMGBTN_Add.Click += new ImageClickEventHandler(_IMGBTN_Add_Click);

            // Событиия в окне редактирования планов
            _BTN_Add.Click += new EventHandler(_BTN_Add_Click);
            _BTN_Update.Click += new EventHandler(_BTN_Update_Click);
            _BTN_Cancel.Click += new EventHandler(_BTN_Cancel_Click);

            // Событиия в окне редактирования планов в таблице предшественников
            _GV_ParentGridView.RowCommand += new GridViewCommandEventHandler(_GV_ParentGridView_RowCommand);
            _IMGBTN_ParentAdd.Click += new ImageClickEventHandler(_IMGBTN_ParentAdd_Click);

            // Событиия в окне редактирования предшественников
            _BTN_ParentAdd.Click += new EventHandler(_BTN_ParentAdd_Click);
            _BTN_ParentCancel.Click += new EventHandler(_BTN_ParentCancel_Click);

            _PNL_Editor.Visible = false;
            _PNL_ParentEditor.Visible = false;
        }



        //
        // Действия в главном окне
        //

        void _GV_MainGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ID = 0;
            if (e.CommandArgument.ToString() != "")
                ID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    // Заполнение грида с родителями
                    SqlCommand Command = new SqlCommand("dbo.issdss_plan_loop_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("PlanID", ID);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _PNL_ParentIsNotEmpty.Visible = true;
                        _PNL_ParentIsEmpty.Visible = false;
                        _GV_ParentGridView.DataSource = Reader;
                        _GV_ParentGridView.DataBind();
                    }
                    else
                    {
                        _PNL_ParentIsNotEmpty.Visible = false;
                        _PNL_ParentIsEmpty.Visible = true;
                    }
                    Reader.Close();

                    // Непосредственный селект по текущему плану
                    Command = new SqlCommand("dbo.issdss_plandss_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("PlanID", ID);
                    Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        _TB_ID.Text = Reader["id"].ToString();
                        _TB_Name.Text = Reader["name"].ToString();
                        if (Reader["plan_method_id"].ToString() != "")
                            _DDL_Method.SelectedValue = Reader["plan_method_id"].ToString();
                        else
                            _DDL_Method.SelectedValue = "0";
                        _TB_BeginDate.Text = ((DateTime)Convert.ToDateTime(Reader["begin_date"])).ToShortDateString();
                        _TB_EndDate.Text = ((DateTime)Convert.ToDateTime(Reader["end_date"])).ToShortDateString();
                        if (Reader["measure_id"].ToString() != "")
                            _DDL_Measure.SelectedValue = Reader["measure_id"].ToString();
                        else
                            _DDL_Measure.SelectedValue = "0";
                        if (Reader["isready"].ToString() != "")
                            _CHBX_IsReady.Checked = Convert.ToBoolean(Reader["isready"]);
                        else
                            _CHBX_IsReady.Checked = false;
                        _TB_Alfa.Text = Helper.MyMath.Round(Reader["alfa"].ToString());
                        //_TB_FuncValue.Text = Helper.MyMath.Round(Reader["func_value"].ToString());
                        _TB_ReservPercent.Text = Helper.MyMath.Round(Reader["reserv_percent"].ToString());
                    }
                    Reader.Close();
                    Connection.Close();
                }
                _PNL_Editor.Visible = true;
                _BTN_Add.Visible = false;
                _BTN_Update.Visible = true;
                _UP_Editor.Update();
            }

            if (e.CommandName == "Plan")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.prc_plandss", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("plan_id", ID);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                Response.Redirect("#");
            }

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_plandss_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("PlanID", ID);
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                _PNL_Editor.Visible = false;
                _BTN_Add.Visible = true;
                _BTN_Update.Visible = true;
                _TB_ID.Text = "";
                _TB_Name.Text = "";
                _DDL_Method.SelectedValue = "0";
                _TB_BeginDate.Text = "";
                _TB_EndDate.Text = "";
                _DDL_Measure.SelectedValue = "0";
                _CHBX_IsReady.Checked = false;
                _TB_Alfa.Text = "";
                //_TB_FuncValue.Text = "";
                _TB_ReservPercent.Text = "";
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
            _DDL_Method.SelectedValue = "0";
            _TB_BeginDate.Text = "";
            _TB_EndDate.Text = "";
            _DDL_Measure.SelectedValue = "0";
            _CHBX_IsReady.Checked = false;
            _TB_Alfa.Text = "";
            //_TB_FuncValue.Text = "";
            _TB_ReservPercent.Text = "";

            // Сделать невидимой кнопку добавления родителей, т.к. низя их добавлять пока план не создан
            _IMGBTN_ParentAdd.Visible = false;

            _PNL_ParentIsNotEmpty.Visible = false;
            _PNL_ParentIsEmpty.Visible = true;
            _UP_Editor.Update();
        }

        //
        // Действия в окне редактора планов
        //

        void _BTN_Add_Click(object sender, EventArgs e)
        {
            if (Context.Request.Cookies["TaskID"] != null)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_plandss_Insert", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                    Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                    if (_DDL_Method.SelectedValue != "0")
                        Command.Parameters.AddWithValue("MethodID", _DDL_Method.SelectedValue);
                    Command.Parameters.AddWithValue("BeginDate", _TB_BeginDate.Text);
                    Command.Parameters.AddWithValue("EndDate", _TB_EndDate.Text);
                    if (_DDL_Measure.SelectedValue != "0")
                        Command.Parameters.AddWithValue("MeasureID", _DDL_Measure.SelectedValue);
                    if (_CHBX_IsReady.Checked)
                        Command.Parameters.AddWithValue("IsReady", 1);
                    else
                        Command.Parameters.AddWithValue("IsReady", 0);
                    Command.Parameters.AddWithValue("Alfa", Convert.ToDecimal(_TB_Alfa.Text));
                    //Command.Parameters.AddWithValue("FuncValue", Convert.ToDecimal(_TB_FuncValue.Text));
                    Command.Parameters.AddWithValue("ReservPercent", Convert.ToDecimal(_TB_ReservPercent.Text));
                    Connection.Open();
                    Command.ExecuteNonQuery();
                }
                _PNL_Editor.Visible = false;
                _BTN_Add.Visible = true;
                _BTN_Update.Visible = true;
                _TB_ID.Text = "";
                _TB_Name.Text = "";
                _DDL_Method.SelectedValue = "0";
                _TB_BeginDate.Text = "";
                _TB_EndDate.Text = "";
                _DDL_Measure.SelectedValue = "0";
                _CHBX_IsReady.Checked = false;
                _TB_Alfa.Text = "";
                //_TB_FuncValue.Text = "";
                _TB_ReservPercent.Text = "";
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
                SqlCommand Command = new SqlCommand("dbo.issdss_plandss_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("PlanID", _TB_ID.Text);
                Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                if (_DDL_Method.SelectedValue != "0")
                    Command.Parameters.AddWithValue("MethodID", _DDL_Method.SelectedValue);
                Command.Parameters.AddWithValue("BeginDate", _TB_BeginDate.Text);
                Command.Parameters.AddWithValue("EndDate", _TB_EndDate.Text);
                if (_DDL_Measure.SelectedValue != "0")
                    Command.Parameters.AddWithValue("MeasureID", _DDL_Measure.SelectedValue);
                if (_CHBX_IsReady.Checked)
                    Command.Parameters.AddWithValue("IsReady", 1);
                else
                    Command.Parameters.AddWithValue("IsReady", 0);
                Command.Parameters.AddWithValue("Alfa", Convert.ToDecimal(_TB_Alfa.Text));
                //Command.Parameters.AddWithValue("FuncValue", Convert.ToDecimal(_TB_FuncValue.Text));
                Command.Parameters.AddWithValue("ReservPercent", Convert.ToDecimal(_TB_ReservPercent.Text));
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            _PNL_Editor.Visible = false;
            _BTN_Add.Visible = true;
            _BTN_Update.Visible = true;
            _TB_ID.Text = "";
            _TB_Name.Text = "";
            _DDL_Method.SelectedValue = "0";
            _TB_BeginDate.Text = "";
            _TB_EndDate.Text = "";
            _DDL_Measure.SelectedValue = "0";
            _CHBX_IsReady.Checked = false;
            _TB_Alfa.Text = "";
            //_TB_FuncValue.Text = "";
            _TB_ReservPercent.Text = "";
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
            _DDL_Method.SelectedValue = "0";
            _TB_BeginDate.Text = "";
            _TB_EndDate.Text = "";
            _DDL_Measure.SelectedValue = "0";
            _CHBX_IsReady.Checked = false;
            _TB_Alfa.Text = "";
            //_TB_FuncValue.Text = "";
            _TB_ReservPercent.Text = "";
            _UP_Editor.Update();
        }

        //
        // Действия в окне редактора планов в таблице предшественников
        //

        void _GV_ParentGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_plan_loop_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("PlanID", _TB_ID.Text);
                    Command.Parameters.AddWithValue("ParentPlanID", ID);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _PNL_ParentIsNotEmpty.Visible = true;
                        _PNL_ParentIsEmpty.Visible = false;
                        _GV_ParentGridView.DataSource = Reader;
                        _GV_ParentGridView.DataBind();
                    }
                    else
                    {
                        _PNL_ParentIsNotEmpty.Visible = false;
                        _PNL_ParentIsEmpty.Visible = true;
                    }
                }
                _PNL_Editor.Visible = true;
                _PNL_ParentEditor.Visible = false;
                _UP_Editor.Update();
            }
        }

        void _IMGBTN_ParentAdd_Click(object sender, ImageClickEventArgs e)
        {
            _PNL_Editor.Visible = true;
            _PNL_ParentEditor.Visible = true;

            // Заполнение ДропДаунЛиста родителей
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_plan_loop_Read_All", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                Command.Parameters.AddWithValue("PlanID", _TB_ID.Text);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                _DDL_Parent.Items.Clear();
                _DDL_Parent.Items.Add(new ListItem("", "0"));
                while (Reader.Read())
                    _DDL_Parent.Items.Add(new ListItem(Reader["name"].ToString(), Reader["id"].ToString()));
                Reader.Close();
                Connection.Close();
            }

            _DDL_Parent.SelectedValue = "0";
            _UP_ParentEditor.Update();
        }

        //
        // Действия в окне редактора предшественников
        //

        // Добавить родителя
        void _BTN_ParentAdd_Click(object sender, EventArgs e)
        {
            if (_DDL_Parent.SelectedValue != "0")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_plan_loop_Insert", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("PlanID", _TB_ID.Text);
                    Command.Parameters.AddWithValue("ParentPlanID", _DDL_Parent.SelectedValue);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _PNL_ParentIsNotEmpty.Visible = true;
                        _PNL_ParentIsEmpty.Visible = false;
                        _GV_ParentGridView.DataSource = Reader;
                        _GV_ParentGridView.DataBind();
                    }
                    else
                    {
                        _PNL_ParentIsNotEmpty.Visible = false;
                        _PNL_ParentIsEmpty.Visible = true;
                    }
                }
            }
            _BTN_ParentCancel_Click(sender, e);
        }

        // Отмена редактирования родителей
        void _BTN_ParentCancel_Click(object sender, EventArgs e)
        {
            _PNL_Editor.Visible = true;
            _PNL_ParentEditor.Visible = false;
            _UP_Editor.Update();
        }
    }
}