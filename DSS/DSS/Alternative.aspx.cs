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
using CommonModel;

namespace DSS.DSS
{
    public partial class Alternative : System.Web.UI.Page
    {
        // Пейдж лоад
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager1.RegisterAsyncPostBackControl(_IMGBTN_Add);
            ScriptManager1.RegisterAsyncPostBackControl(_BTN_Add);
            ScriptManager1.RegisterAsyncPostBackControl(_BTN_Update);
            ScriptManager1.RegisterAsyncPostBackControl(_BTN_Cancel);

            // Разрешения
            int id, pagePermission = 0;
            // Главная таблица альтернатив
            _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 2].Visible = false;
            _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 1].Visible = false;
            // Окно редактирования альтернатив
            _TB_Name.Enabled = false;
            _IMGBTN_JobAdd.Visible = false;
            _GV_JobGridView.Columns[_GV_JobGridView.Columns.Count - 1].Visible = false;
            _BTN_Add.Enabled = false;
            _BTN_Update.Enabled = false;
            // Окно редактирования работ
            _TB_JobOrd.Enabled = false;
            _TB_JobName.Enabled = false;
            _TB_Duration.Enabled = false;
            _DDL_Measure.Enabled = false;
            _IMGBTN_ParentAdd.Visible = false;
            _GV_ParentGridView.Columns[_GV_ParentGridView.Columns.Count - 1].Visible = false;
            _BTN_JobAdd.Enabled = false;
            _BTN_JobUpdate.Enabled = false;
            _PNL_Models.Visible = true;
            _LBL_Model.Visible = true;

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
                        case 11: pagePermission = id; break;
                        case 12: _IMGBTN_Add.Visible = true; break;
                        case 13: // Разрешение редактирования альтернатив
                            // Окно редактирования альтернатив
                            _TB_Name.Enabled = true;
                            _IMGBTN_JobAdd.Visible = true;
                            _GV_JobGridView.Columns[_GV_JobGridView.Columns.Count - 1].Visible = true;
                            _BTN_Add.Enabled = true;
                            _BTN_Update.Enabled = true;
                            // Окно редактирования работ
                            _TB_JobOrd.Enabled = true;
                            _TB_JobName.Enabled = true;
                            _TB_Duration.Enabled = true;
                            _DDL_Measure.Enabled = true;
                            _IMGBTN_ParentAdd.Visible = true;
                            _GV_ParentGridView.Columns[_GV_ParentGridView.Columns.Count - 1].Visible = true;
                            _BTN_JobAdd.Enabled = true;
                            _BTN_JobUpdate.Enabled = true;
                            break;
                        case 55: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 2].Visible = true; break;
                        case 14: _GV_MainGridView.Columns[_GV_MainGridView.Columns.Count - 1].Visible = true; break;
                    }
                }
                if (pagePermission == 0)
                    Response.Redirect("Default.aspx");
                Reader.Close();
                SqlCommand command = new SqlCommand("dbo.issdss_model_Read_All", Connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                _DDL_Models.Items.Clear();


                while (reader.Read())
                    _DDL_Models.Items.Add(new ListItem(reader["name"].ToString()));
                reader.Close();
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
                    Reader.Close();
                    Connection.Close();
                }
            }

            // События в главном окне
            _GV_MainGridView.RowCommand += new GridViewCommandEventHandler(_GV_MainGridView_RowCommand);
            _IMGBTN_Add.Click += new ImageClickEventHandler(_IMGBTN_Add_Click);
            _BTN_Add.Click += new EventHandler(_BTN_Add_Click);
            _BTN_Update.Click += new EventHandler(_BTN_Update_Click);
            _BTN_Cancel.Click += new EventHandler(_BTN_Cancel_Click);

            // Событиия в окне редактирования альтернатив
            _GV_JobGridView.RowCommand += new GridViewCommandEventHandler(_GV_JobGridView_RowCommand);
            _IMGBTN_JobAdd.Click += new ImageClickEventHandler(_IMGBTN_JobAdd_Click);
            _BTN_JobAdd.Click += new EventHandler(_BTN_JobAdd_Click);
            _BTN_JobUpdate.Click += new EventHandler(_BTN_JobUpdate_Click);
            _BTN_JobCancel.Click += new EventHandler(_BTN_JobCancel_Click);

            // Событиия в окне редактирования альтернатив
            _GV_ParentGridView.RowCommand += new GridViewCommandEventHandler(_GV_ParentGridView_RowCommand);
            _IMGBTN_ParentAdd.Click += new ImageClickEventHandler(_IMGBTN_ParentAdd_Click);
            _BTN_ParentAdd.Click += new EventHandler(_BTN_ParentAdd_Click);
            _BTN_ParentCancel.Click += new EventHandler(_BTN_ParentCancel_Click);

            _PNL_Editor.Visible = false;
            _PNL_JobEditor.Visible = false;
            _PNL_ParentEditor.Visible = false;
        }
        


        //
        // Действия в главном окне
        //

        // GridView альтернатив
        void _GV_MainGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ID = Convert.ToInt32(e.CommandArgument);
            
            if (e.CommandName == "EditItem")
            {
                _IMGBTN_RunModel.Visible = true;
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    // Заполнение грида с работами
                    SqlCommand Command = new SqlCommand("dbo.issdss_job_Read_All", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("AlternativeID", ID);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _PNL_JobIsNotEmpty.Visible = true;
                        _PNL_JobIsEmpty.Visible = false;
                        _GV_JobGridView.DataSource = Reader;
                        _GV_JobGridView.DataBind();
                    }
                    else
                    {
                        _PNL_JobIsNotEmpty.Visible = false;
                        _PNL_JobIsEmpty.Visible = true;
                    }
                    Reader.Close();

                    // Непосредственный селект по текущей альтернативе
                    Command = new SqlCommand("dbo.issdss_alternative_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("AlternativeID", ID);
                    Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        _TB_ID.Text = Reader["id"].ToString();
                        _TB_Name.Text = Reader["name"].ToString();
                    }
                    Reader.Close();

                    //заполнить DDL по текущей альтернативе
                    Command = new SqlCommand("dbo.issdss_alternative_model_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("ThisAlternativeId", ID);
                    Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        _DDL_Models.SelectedValue = Reader["name"].ToString();
                    }
                    Reader.Close();

                    Connection.Close();
                }
                _PNL_Editor.Visible = true;
                _BTN_Add.Visible = false;
                _BTN_Update.Visible = true;
                _UP_Editor.Update();
            }

            if (e.CommandName == "Person")
            {
                Response.Redirect("AlternativePerson.aspx?id=" + ID.ToString());
            }

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_alternative_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("AlternativeID", ID);
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
        
        // Открыть форму для добавления альтернативы
        void _IMGBTN_Add_Click(object sender, ImageClickEventArgs e)
        {
            _IMGBTN_RunModel.Visible = false;
            _PNL_Editor.Visible = true;
            _BTN_Add.Visible = true;
            _BTN_Update.Visible = false;
            _TB_ID.Text = "";
            _TB_Name.Text = "";

            // Активация элементов управления только для добавления новой альтернативы
            _TB_Name.Enabled = true;
            _BTN_Add.Enabled = true;
            _BTN_Update.Enabled = true;

            // Сделать невидимой кнопку добавления работ, т.к. низя их добавлять пока альтернатива не создана
            _IMGBTN_JobAdd.Visible = false;

            _PNL_JobIsNotEmpty.Visible = false;
            _PNL_JobIsEmpty.Visible = true;
            _UP_Editor.Update();
        }



        //
        // Действия в окне редактора альтернатив
        //

        // GridView работ
        void _GV_JobGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    // Заполнение грида с родителями
                    SqlCommand Command = new SqlCommand("dbo.issdss_job_loop_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("JobID", ID);
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

                    // Заполнение репитера ресурсов
                    Command = new SqlCommand("dbo.issdss_job_resource_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                    Command.Parameters.AddWithValue("PersonID", Context.Request.Cookies["UserID"].Value);
                    Command.Parameters.AddWithValue("JobID", ID);
                    Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _RP_Resource.DataSource = Reader;
                        _RP_Resource.DataBind();
                    }
                    Reader.Close();

                    // Непосредственный селект по текущей работе
                    Command = new SqlCommand("dbo.issdss_job_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("JobID", ID);
                    Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        _TB_JobID.Text = Reader["id"].ToString();
                        _TB_JobOrd.Text = Reader["ord"].ToString();
                        _TB_JobName.Text = Reader["name"].ToString();
                        _TB_Duration.Text = Helper.MyMath.Round(Reader["duration"].ToString());
                        if (Reader["measure_id"].ToString() != "")
                            _DDL_Measure.SelectedValue = Reader["measure_id"].ToString();
                        else
                            _DDL_Measure.SelectedValue = "0";
                    }
                    Reader.Close();
                    Connection.Close();
                }
                _PNL_Editor.Visible = true;
                _PNL_JobEditor.Visible = true;
                _BTN_JobAdd.Visible = false;
                _BTN_JobUpdate.Visible = true;
                //_IMGBTN_ParentAdd.Visible = true;
                _PNL_Resource.Visible = true;
                _UP_JobEditor.Update();
            }

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_job_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("AlternativeID", _TB_ID.Text);
                    Command.Parameters.AddWithValue("JobID", ID);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _PNL_JobIsNotEmpty.Visible = true;
                        _PNL_JobIsEmpty.Visible = false;
                        _GV_JobGridView.DataSource = Reader;
                        _GV_JobGridView.DataBind();
                    }
                    else
                    {
                        _PNL_JobIsNotEmpty.Visible = false;
                        _PNL_JobIsEmpty.Visible = true;
                    }
                }
                _PNL_Editor.Visible = true;
                _PNL_JobEditor.Visible = false;
                _UP_Editor.Update();
            }
        }

        // Открыть форму для добавления работы
        void _IMGBTN_JobAdd_Click(object sender, ImageClickEventArgs e)
        {
            _PNL_Editor.Visible = true;
            _PNL_JobEditor.Visible = true;
            _BTN_JobAdd.Visible = true;
            _BTN_JobUpdate.Visible = false;
            _TB_JobID.Text = "";
            _TB_JobName.Text = "";
            _TB_Duration.Text = "";
            _DDL_Measure.SelectedValue = "0";
            _TB_JobOrd.Text = "";

            // Сделать невидимой кнопку добавления родителей, т.к. низя их добавлять пока работа не создана
            // + невидима панель родителей
            _IMGBTN_ParentAdd.Visible = false;
            _PNL_ParentIsNotEmpty.Visible = false;
            _PNL_ParentIsEmpty.Visible = true;
            _PNL_Resource.Visible = false;

            _UP_JobEditor.Update();
        }

        // Добавить альтернативу
        void _BTN_Add_Click(object sender, EventArgs e)
        {
            _IMGBTN_RunModel.Visible = false;
            string modelId = "";
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_model_Read", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("ModelName", _DDL_Models.SelectedValue.ToString());
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.Read())
                    modelId = reader["id"].ToString();
                reader.Close();
            }

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_alternative_Insert", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                Command.Parameters.AddWithValue("UserID", Context.Request.Cookies["UserID"].Value);
                Command.Parameters.AddWithValue("ModelID", int.Parse(modelId));
                Command.Parameters.AddWithValue("ModelName", _DDL_Models.SelectedValue.ToString());
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

        // Обновить альтернативу
        void _BTN_Update_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_alternative_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("AlternativeID", _TB_ID.Text);
                Command.Parameters.AddWithValue("Name", _TB_Name.Text);
#warning странно считывается селектедвэлью
                Command.Parameters.AddWithValue("ModelName", _DDL_Models.SelectedValue.ToString());
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

        // Отмена редактирования альтернатив
        void _BTN_Cancel_Click(object sender, EventArgs e)
        {
            _PNL_Editor.Visible = false;
            _BTN_Add.Visible = true;
            _BTN_Update.Visible = true;
            _TB_ID.Text = "";
            _TB_Name.Text = "";
            _UP_Editor.Update();
        }



        //
        // Действия в окне редактора работ
        //

        // GridView родителей
        void _GV_ParentGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_job_loop_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("JobID", _TB_JobID.Text);
                    Command.Parameters.AddWithValue("ParentJobID", ID);
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
                _PNL_JobEditor.Visible = true;
                _PNL_ParentEditor.Visible = false;
                _UP_JobEditor.Update();
            }
        }

        // Открыть форму для добавления родителя
        void _IMGBTN_ParentAdd_Click(object sender, ImageClickEventArgs e)
        {
            _PNL_Editor.Visible = true;
            _PNL_JobEditor.Visible = true;
            _PNL_ParentEditor.Visible = true;

            // Заполнение ДропДаунЛиста родителей
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_job_loop_Read_All", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                Command.Parameters.AddWithValue("JobID", _TB_JobID.Text);
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

        // Добавить работу
        void _BTN_JobAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_job_Insert", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("AlternativeID", _TB_ID.Text);
                Command.Parameters.AddWithValue("Name", _TB_JobName.Text);
                if (_TB_Duration.Text != "")
                    Command.Parameters.AddWithValue("Duration", Convert.ToDecimal(_TB_Duration.Text));
                else
                    Command.Parameters.AddWithValue("Duration", 0.0);
                Command.Parameters.AddWithValue("MeasureID", _DDL_Measure.SelectedValue);
                Command.Parameters.AddWithValue("Ord", _TB_JobOrd.Text);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    _PNL_JobIsNotEmpty.Visible = true;
                    _PNL_JobIsEmpty.Visible = false;
                    _GV_JobGridView.DataSource = Reader;
                    _GV_JobGridView.DataBind();
                }
                else
                {
                    _PNL_JobIsNotEmpty.Visible = false;
                    _PNL_JobIsEmpty.Visible = true;
                }
            }
            _PNL_Editor.Visible = true;
            _PNL_JobEditor.Visible = false;
            _UP_Editor.Update();
        }

        // Обновить работу
        void _BTN_JobUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                // Обновление ресурсов работы
                string value;
                SqlCommand Command = new SqlCommand("dbo.issdss_job_resource_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("JobID", _TB_JobID.Text);
                string resXML = "<job_resource>";
                for (int i = 0; i < _RP_Resource.Items.Count; i++)
                {
                    resXML += "<row>";
                    resXML += "<resource_id>" + ((Label)_RP_Resource.Items[i].FindControl("_LBL_ID")).Text + "</resource_id>";
                    value = ((TextBox)_RP_Resource.Items[i].FindControl("_TB_Value")).Text;
                    if (value.Contains(","))
                        value = value.Replace(',', '.');
                    if (value == "")
                        value = "0";
                    resXML += "<value>" + value + "</value>";
                    resXML += "</row>";
                }
                resXML += "</job_resource>";
                Command.Parameters.AddWithValue("@d", resXML);
                Connection.Open();
                Command.ExecuteNonQuery();

                // Обновление основных параметров работы
                Command = new SqlCommand("dbo.issdss_job_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("AlternativeID", _TB_ID.Text);
                Command.Parameters.AddWithValue("JobID", _TB_JobID.Text);
                Command.Parameters.AddWithValue("Name", _TB_JobName.Text);
                if (_TB_Duration.Text != "")
                    Command.Parameters.AddWithValue("Duration", Convert.ToDecimal(_TB_Duration.Text));
                else
                    Command.Parameters.AddWithValue("Duration", 0.0);
                Command.Parameters.AddWithValue("MeasureID", _DDL_Measure.SelectedValue);
                Command.Parameters.AddWithValue("Ord", _TB_JobOrd.Text);
                //Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    _PNL_JobIsNotEmpty.Visible = true;
                    _PNL_JobIsEmpty.Visible = false;
                    _GV_JobGridView.DataSource = Reader;
                    _GV_JobGridView.DataBind();
                }
                else
                {
                    _PNL_JobIsNotEmpty.Visible = false;
                    _PNL_JobIsEmpty.Visible = true;
                }
            }
            _PNL_Editor.Visible = true;
            _PNL_JobEditor.Visible = false;
            _UP_Editor.Update();
        }

        // Отмена редактирования работ
        void _BTN_JobCancel_Click(object sender, EventArgs e)
        {
            _PNL_Editor.Visible = true;
            _PNL_JobEditor.Visible = false;
            _UP_Editor.Update();
        }



        //
        // Действия в окне редактора родителей
        //

        // Добавить родителя
        void _BTN_ParentAdd_Click(object sender, EventArgs e)
        {
            if (_DDL_Parent.SelectedValue != "0")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_job_loop_Insert", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("JobID", _TB_JobID.Text);
                    Command.Parameters.AddWithValue("ParentJobID", _DDL_Parent.SelectedValue);
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
            _PNL_JobEditor.Visible = true;
            _PNL_ParentEditor.Visible = false;
            _UP_JobEditor.Update();
        }


        #region Скорее всего убрать из глобальных переменных
        private static List<string> paramsList;

        public static List<string> ParamsList
        {
            get { return paramsList; }
        }

        private static List<string> codeList;

        public static List<string> CodeList
        {
            get { return Alternative.codeList; }
        }
        #endregion

        private static Dictionary<string, double> paramsTable;

        public static Dictionary<string, double> ParamsTable
        {
            get { return Alternative.paramsTable; }
        }

        private static Dictionary<string, double> kormTable;

        public static Dictionary<string, double> KormTable
        {
            get { return Alternative.kormTable; }
        }

        private static bool runStart;

        public static bool RunStart
        {
            get { return runStart; }
        }

        private static int alternativeId;

        public static int AlternativeId
        {
            get { return Alternative.alternativeId; }
        }

        private enum modelsList
        {
            МоделиНЕТ = 0,
            ПростаяСМО = 1,
            Лифт = 2,
            ВычислительнаяСеть = 3
        }



        /// <summary>
        /// Собирает значения параметров модели из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _IMGBTN_RunModel_Click(object sender, ImageClickEventArgs e)
        {
            runStart = false;
            paramsList = new List<string>();
            codeList = new List<string>();

            paramsTable = new Dictionary<string, double>();
            kormTable = new Dictionary<string, double>();

            alternativeId = int.Parse(_TB_ID.Text);
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_model_crit_values", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("ThisAlternativeId", int.Parse(_TB_ID.Text));
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    paramsList.Add(reader["value"].ToString());
                }
                reader.Close();

                Command = new SqlCommand("dbo.issdss_model_crit_Read_Code_Params", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("ThisAlternativeId", int.Parse(_TB_ID.Text));
                reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    codeList.Add(reader["code"].ToString());
                }
                reader.Close();

                int i = 0;
                foreach (var item in codeList)
                {
                    paramsTable.Add(item, double.Parse(paramsList[i]));
                    i++;
                }

            }
            modelsList modelID = modelsList.МоделиНЕТ;
            object modelId = "";
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_model_read_id", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("ThisAlternativeId", int.Parse(_TB_ID.Text));
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.Read())
                {
                    modelID = (modelsList)Enum.Parse(typeof(modelsList), reader["model_id"].ToString());
                    runStart = true;
                }
                reader.Close();
            }
            Action<int> action;
            switch (modelID)
            {
                case modelsList.ПростаяСМО:
                    Modeling.TraceString = "";
                    var smoModel = new PSS.Sample.SMOModel(null, "Модель СМО");
                    action = smoModel.PERFORM;
                    action.BeginInvoke(0, null, null);

                    Response.Redirect("#");
                    #region сохранение корм


                    foreach (KeyValuePair<string, double> item in kormTable)
                    {
                        using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                        {
                            SqlCommand Command = new SqlCommand("dbo.issdss_crit_value_Insert_KORM", Connection);
                            Command.CommandType = CommandType.StoredProcedure;
                            Command.Parameters.AddWithValue("Value", item.Value);
                            Command.Parameters.AddWithValue("Code", item.Key);
                            Connection.Open();
                            Command.ExecuteNonQuery();
                        }
                    }



                    #endregion

                    break;

                case modelsList.ВычислительнаяСеть:
                    Modeling.TraceString = "";
                    var vs = new PSS.VS.VS(null, "Модель ВС");
                    action = vs.PERFORM;
                    action.BeginInvoke(0, null, null);
                    Response.Redirect("#");
                    break;

                case modelsList.Лифт:
                    Modeling.TraceString = "ДАННАЯ МОДЕЛЬ ЕЩЕ НЕ РЕАЛИЗОВАНА";
                    Response.Redirect("#");
                    break;

                case modelsList.МоделиНЕТ:
                    Modeling.TraceString = "ПРОИЗОШЛА ОШИБКА";
                    Response.Redirect("#");
                    break;
            }
        }
    }
}