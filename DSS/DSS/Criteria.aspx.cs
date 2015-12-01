using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DSS.DSS.Classes;

namespace DSS.DSS
{
    public partial class Criteria1 : System.Web.UI.Page
    {
        private Helper.CTreeNode HTree = new Helper.CTreeNode();

        // Пейдж лоад
        protected void Page_Load(object sender, EventArgs e)
        {
            // Разрешения
            int id, pagePermission = 0;
            _TB_Name.Enabled = false;
            _TB_Description.Enabled = false;
            _DDL_Parent.Enabled = false;
            _IMGBTN_ScaleAdd.Visible = false;
            _GV_ScaleGridView.Columns[1].Visible = true;
            _GV_ScaleGridView.Columns[2].Visible = false;
            _GV_ScaleGridView.Columns[_GV_ScaleGridView.Columns.Count - 1].Visible = false;
            _HL_EditMethod.Visible = false;
            _DDL_ImprovementDirection.Enabled = false;
            _TB_IdealValue.Enabled = false;
            _DDL_AggregationMethod.Enabled = false;
            _CHBX_IsNumber.Enabled = false;
            _TB_Ord.Enabled = false;
            _BTN_Add.Enabled = false;
            _BTN_Update.Enabled = false;
            _PNL_CriteriaDelete.Visible = false;
            ScaleEditTd.Visible = false;
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
                        case 21: pagePermission = id; break;
                        case 22: _IMGBTN_Add.Visible = true; break;
                        case 23: // Разрешение редактирования критериев
                            _TB_Name.Enabled = true;
                            _TB_Description.Enabled = true;
                            _DDL_Parent.Enabled = true;
                            _IMGBTN_ScaleAdd.Visible = true;
                            _GV_ScaleGridView.Columns[1].Visible = false;
                            _GV_ScaleGridView.Columns[2].Visible = true;
                            _GV_ScaleGridView.Columns[_GV_ScaleGridView.Columns.Count - 1].Visible = true;
                            _HL_EditMethod.Visible = true;
                            _DDL_ImprovementDirection.Enabled = true;
                            _TB_IdealValue.Enabled = true;
                            _DDL_AggregationMethod.Enabled = true;
                            _CHBX_IsNumber.Enabled = true;
                            _TB_Ord.Enabled = true;
                            _BTN_Add.Enabled = true;
                            _BTN_Update.Enabled = true;
                            ScaleEditTd.Visible = IsScaleVisible();
                            break;
                        case 24: _PNL_CriteriaDelete.Visible = true; break;
                    }
                }
                if (pagePermission == 0)
                    Response.Redirect("Default.aspx");
            }

            if (!IsPostBack)
            {
                string ID = Convert.ToString(Request.QueryString["id"]);
                DrawTree(ID);

                if (!string.IsNullOrEmpty(ID))
                {
                    TreeView1.SelectedNode.Value = ID;
                    TreeView1_SelectedNodeChanged(sender, e);
                }
            }

            // Событиия по редактированию шкал
            _GV_ScaleGridView.RowCommand += new GridViewCommandEventHandler(_GV_ScaleGridView_RowCommand);
            _IMGBTN_ScaleAdd.Click += new ImageClickEventHandler(_IMGBTN_ScaleAdd_Click);
            _BTN_ScaleAdd.Click += new EventHandler(_BTN_ScaleAdd_Click);
            _BTN_ScaleUpdate.Click += new EventHandler(_BTN_ScaleUpdate_Click);
            _BTN_ScaleCancel.Click += new EventHandler(_BTN_ScaleCancel_Click);

            // События по всей форме редактирования критериев
            _IMGBTN_Add.Click += new ImageClickEventHandler(_IMGBTN_Add_Click);
            _BTN_Add.Click += new EventHandler(_BTN_Add_Click);
            _BTN_Update.Click += new EventHandler(_BTN_Update_Click);
            _BTN_Cancel.Click += new EventHandler(_BTN_Cancel_Click);
            _IMGBTN_Delete.Click += new ImageClickEventHandler(_IMGBTN_Delete_Click);
            _DDL_ImprovementDirection.SelectedIndexChanged += new EventHandler(_DDL_ImprovementDirection_SelectedIndexChanged);
            _DDL_AggregationMethod.SelectedIndexChanged += new EventHandler(_DDL_AggregationMethod_SelectedIndexChanged);

            _PNL_Editor.Visible = false;
            _PNL_ScaleEditor.Visible = false;
        }

        protected void DrawTree(string id)
        {
            //  список элементов 
            List<Helper.CTreeNode> TreeList = HTree.GetTreeItem(Context.Request.Cookies["TaskID"].Value);
            TreeView1.Nodes.Clear();
            HTree.BindTree(Context.Request.Cookies["TaskID"].Value, TreeList, null, id, TreeView1);
        }

        // Действия в главном окне
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                // Заполнение ДропДаунЛиста родителей
                SqlCommand Command = new SqlCommand("dbo.issdss_criteria_ChooseParent", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("id", TreeView1.SelectedNode.Value);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                _DDL_Parent.Items.Clear();
                _DDL_Parent.Items.Add(new ListItem("Корень", "0"));
                while (Reader.Read())
                    _DDL_Parent.Items.Add(new ListItem(Reader["name"].ToString(), Reader["id"].ToString()));
                Reader.Close();

                // Заполнение грида со шкалми
                Command = new SqlCommand("dbo.issdss_crit_scale_Read", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("CriteriaID", TreeView1.SelectedNode.Value);
                Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    _PNL_ScaleIsNotEmpty.Visible = true;
                    _PNL_ScaleIsEmpty.Visible = false;
                    _GV_ScaleGridView.DataSource = Reader;
                    _GV_ScaleGridView.DataBind();
                }
                else
                {
                    _PNL_ScaleIsNotEmpty.Visible = false;
                    _PNL_ScaleIsEmpty.Visible = true;
                }
                Reader.Close();

                // Заполнение ДропДаунЛиста методов свертки
                Command = new SqlCommand("dbo.issdss_method_Read_All", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Reader = Command.ExecuteReader();
                _DDL_AggregationMethod.Items.Clear();
                while (Reader.Read())
                {
                    _DDL_AggregationMethod.Items.Add(new ListItem(Reader["name"].ToString(), Reader["id"].ToString()));
                    _DDL_AggregationMethodUrl.Items.Add(new ListItem(Reader["url"].ToString(), Reader["id"].ToString()));
                }
                Reader.Close();

                // Непосредственный селект по текущему критерию
                Command = new SqlCommand("dbo.issdss_criteria_Read_by_id", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ID", TreeView1.SelectedNode.Value);
                Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    // Имя
                    _TB_Name.Text = Reader["name"].ToString();

                    // Описание
                    _TB_Description.Text = Reader["description"].ToString();

                    // Родитель
                    if (Reader["parent_crit_id"].ToString() != "")
                        _DDL_Parent.SelectedValue = Reader["parent_crit_id"].ToString();
                    else
                        _DDL_Parent.SelectedValue = "0";

                    // Обобщенный критерий или нет
                    if (Reader["is_parent"].ToString() != "0")
                    {
                        _PNL_Simple.Visible = false;
                        _PNL_Integrated.Visible = true;
                    }
                    else
                    {
                        _PNL_Simple.Visible = true;
                        _PNL_Integrated.Visible = false;
                    }

                    // Направление улучшений
                    switch (Reader["ismin"].ToString())
                    {
                        case "1": _DDL_ImprovementDirection.SelectedValue = "1"; _PNL_IdealValue.Visible = false; break;
                        case "-1": _DDL_ImprovementDirection.SelectedValue = "-1"; _PNL_IdealValue.Visible = false; break;
                        case "0": _DDL_ImprovementDirection.SelectedValue = "0"; _PNL_IdealValue.Visible = true; _TB_IdealValue.Text = Helper.MyMath.Round(Reader["idealvalue"].ToString()); break;
                        default: _DDL_ImprovementDirection.SelectedValue = "1"; break;
                    }

                    // Метод аггрегирования
                    if (Reader["method_id"].ToString() != "")
                        _DDL_AggregationMethod.SelectedValue = Reader["method_id"].ToString();
                    else
                        _DDL_AggregationMethod.SelectedValue = "1";

                    // Ссылка на редактирование метода свертки
                    _HL_EditMethod.NavigateUrl = "~/DSS/" + _DDL_AggregationMethodUrl.Items.FindByValue(_DDL_AggregationMethod.SelectedValue).Text + ".aspx?id=" + TreeView1.SelectedNode.Value;

                    // Числовой показатель
                    if (Reader["is_number"].ToString() != "")
                        _CHBX_IsNumber.Checked = Convert.ToBoolean(Reader["is_number"]);
                    else
                        _CHBX_IsNumber.Checked = false;

                    // Порядковый номер
                    _TB_Ord.Text = Reader["ord"].ToString();
                }
                Reader.Close();
                Connection.Close();
            }
            _PNL_Editor.Visible = true;
            _BTN_Add.Visible = false;
            _BTN_Update.Visible = true;
            _IMGBTN_Delete.Visible = true;
            _UP_Editor.Update();
        }

        void _IMGBTN_Add_Click(object sender, ImageClickEventArgs e)
        {
            _PNL_Editor.Visible = true;
            _BTN_Add.Visible = true;
            _BTN_Update.Visible = false;
            _IMGBTN_Delete.Visible = false;
            _TB_Name.Text = "";
            _TB_Description.Text = "";

            // Заполнение ДропДаунЛиста родителей
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Read_All", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.Cookies["TaskID"] != null)
                    Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                _DDL_Parent.Items.Clear();
                _DDL_Parent.Items.Add(new ListItem("Корень", "0"));
                while (Reader.Read())
                    _DDL_Parent.Items.Add(new ListItem(Reader["name"].ToString(), Reader["id"].ToString()));
            }
            _DDL_Parent.SelectedValue = "0";

            // Активация элементов управления только для добавления нового критерия
            _TB_Name.Enabled = true;
            _TB_Description.Enabled = true;
            _DDL_Parent.Enabled = true;
            _DDL_ImprovementDirection.Enabled = true;
            _TB_IdealValue.Enabled = true;
            _CHBX_IsNumber.Enabled = true;
            _TB_Ord.Enabled = true;
            _BTN_Add.Enabled = true;
            _BTN_Update.Enabled = true;

            // Сделать невидимой кнопку добавления градаций, т.к. низя их добавлять пока критерий не создан
            _IMGBTN_ScaleAdd.Visible = false;

            _PNL_ScaleIsNotEmpty.Visible = false;
            _PNL_ScaleIsEmpty.Visible = true;
            _PNL_Simple.Visible = true;
            _DDL_ImprovementDirection.SelectedValue = "1";
            _PNL_IdealValue.Visible = false;
            _TB_IdealValue.Text = "";
            _PNL_Integrated.Visible = false;
            _CHBX_IsNumber.Checked = false;
            _TB_Ord.Text = "";
            _UP_Editor.Update();
        }

        void _BTN_Add_Click(object sender, EventArgs e)
        {
            if (Context.Request.Cookies["TaskID"] != null)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Insert", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                    Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                    Command.Parameters.AddWithValue("Description", _TB_Description.Text);
                    if (_DDL_Parent.SelectedValue != "0")
                        Command.Parameters.AddWithValue("Parent_ID", _DDL_Parent.SelectedValue);
                    Command.Parameters.AddWithValue("IsMin", _DDL_ImprovementDirection.SelectedValue);
                    if (_TB_IdealValue.Text != "")
                        Command.Parameters.AddWithValue("IdealValue", Convert.ToDecimal(_TB_IdealValue.Text));
                    if (_DDL_AggregationMethod.SelectedValue != "")
                        Command.Parameters.AddWithValue("Method_ID", _DDL_AggregationMethod.SelectedValue);
                    Command.Parameters.AddWithValue("IsNumber", _CHBX_IsNumber.Checked);
                    Command.Parameters.AddWithValue("Ord", _TB_Ord.Text);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                }
            }
            Response.Redirect("Criteria.aspx");
        }

        void _BTN_Update_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("ID", TreeView1.SelectedNode.Value);
                Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                Command.Parameters.AddWithValue("Description", _TB_Description.Text);
                if (_DDL_Parent.SelectedValue != "0")
                    Command.Parameters.AddWithValue("Parent_ID", _DDL_Parent.SelectedValue);
                Command.Parameters.AddWithValue("IsMin", _DDL_ImprovementDirection.SelectedValue);
                if (_TB_IdealValue.Text != "")
                    Command.Parameters.AddWithValue("IdealValue", Convert.ToDecimal(_TB_IdealValue.Text));
                if (_DDL_AggregationMethod.SelectedValue != "")
                    Command.Parameters.AddWithValue("Method_ID", _DDL_AggregationMethod.SelectedValue);
                Command.Parameters.AddWithValue("IsNumber", _CHBX_IsNumber.Checked);
                Command.Parameters.AddWithValue("Ord", _TB_Ord.Text);
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            Response.Redirect("Criteria.aspx");
        }

        void _BTN_Cancel_Click(object sender, EventArgs e)
        {
            //TreeView1.SelectedNode.Selected = false;
            //DrawTree(null);
            //_PNL_Editor.Visible = false;
            Response.Redirect("Criteria.aspx");
        }

        void _IMGBTN_Delete_Click(object sender, ImageClickEventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Delete", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ID", TreeView1.SelectedNode.Value);
                Connection.Open();
                Command.ExecuteNonQuery();
                Connection.Close();
            }
            Response.Redirect("Criteria.aspx");
        }

        // Действия в окне шкал
        void _GV_ScaleGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_crit_scale_Read_by_ID", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("ScaleID", ID);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.Read())
                    {
                        _TB_ScaleID.Text = Reader["id"].ToString();
                        _TB_ScaleName.Text = Reader["name"].ToString();
                        _TB_ScaleRank.Text = Helper.MyMath.Round(Reader["rank"].ToString());
                        _TB_ScaleOrd.Text = Reader["ord"].ToString();
                    }
                }
                _PNL_Editor.Visible = true;
                _PNL_ScaleEditor.Visible = true;
                _BTN_ScaleAdd.Visible = false;
                _BTN_ScaleUpdate.Visible = true;
                _UP_ScaleEditor.Update();
            }

            if (e.CommandName == "DeleteItem")
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_crit_scale_Delete", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("CriteriaID", TreeView1.SelectedNode.Value);
                    Command.Parameters.AddWithValue("ScaleID", ID);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _PNL_ScaleIsNotEmpty.Visible = true;
                        _PNL_ScaleIsEmpty.Visible = false;
                        _GV_ScaleGridView.DataSource = Reader;
                        _GV_ScaleGridView.DataBind();
                    }
                    else
                    {
                        _PNL_ScaleIsNotEmpty.Visible = false;
                        _PNL_ScaleIsEmpty.Visible = true;
                    }
                }
                _PNL_Editor.Visible = true;
                _PNL_ScaleEditor.Visible = false;
                _UP_Editor.Update();
            }
        }

        void _IMGBTN_ScaleAdd_Click(object sender, ImageClickEventArgs e)
        {
            _PNL_Editor.Visible = true;
            _PNL_ScaleEditor.Visible = true;
            _BTN_ScaleAdd.Visible = true;
            _BTN_ScaleUpdate.Visible = false;
            _TB_ScaleID.Text = "";
            _TB_ScaleName.Text = "";
            _TB_ScaleRank.Text = "";
            _TB_ScaleOrd.Text = "";
            _UP_ScaleEditor.Update();
        }

        void _BTN_ScaleAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_crit_scale_Insert", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("CriteriaID", TreeView1.SelectedNode.Value);
                Command.Parameters.AddWithValue("Name", _TB_ScaleName.Text);
                if (_TB_ScaleRank.Text != "")
                    Command.Parameters.AddWithValue("Rank", Convert.ToDecimal(_TB_ScaleRank.Text));
                Command.Parameters.AddWithValue("Ord", _TB_ScaleOrd.Text);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    _PNL_ScaleIsNotEmpty.Visible = true;
                    _PNL_ScaleIsEmpty.Visible = false;
                    _GV_ScaleGridView.DataSource = Reader;
                    _GV_ScaleGridView.DataBind();
                }
                else
                {
                    _PNL_ScaleIsNotEmpty.Visible = false;
                    _PNL_ScaleIsEmpty.Visible = true;
                }
            }
            _PNL_Editor.Visible = true;
            _PNL_ScaleEditor.Visible = false;
            _UP_Editor.Update();
        }

        void _BTN_ScaleUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_crit_scale_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("CriteriaID", TreeView1.SelectedNode.Value);
                Command.Parameters.AddWithValue("ID", _TB_ScaleID.Text);
                Command.Parameters.AddWithValue("Name", _TB_ScaleName.Text);
                if (_TB_ScaleRank.Text != "")
                    Command.Parameters.AddWithValue("Rank", Convert.ToDecimal(_TB_ScaleRank.Text));
                Command.Parameters.AddWithValue("Ord", _TB_ScaleOrd.Text);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    _PNL_ScaleIsNotEmpty.Visible = true;
                    _PNL_ScaleIsEmpty.Visible = false;
                    _GV_ScaleGridView.DataSource = Reader;
                    _GV_ScaleGridView.DataBind();
                }
                else
                {
                    _PNL_ScaleIsNotEmpty.Visible = false;
                    _PNL_ScaleIsEmpty.Visible = true;
                }
            }
            _PNL_Editor.Visible = true;
            _PNL_ScaleEditor.Visible = false;
            _UP_Editor.Update();
        }

        void _BTN_ScaleCancel_Click(object sender, EventArgs e)
        {
            _PNL_Editor.Visible = true;
            _PNL_ScaleEditor.Visible = false;
            _UP_Editor.Update();
        }

        void _DDL_ImprovementDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_DDL_ImprovementDirection.SelectedValue == "0")
            {
                _PNL_Editor.Visible = true;
                _PNL_IdealValue.Visible = true;
                _UP_Editor.Update();
            }
            else
            {
                _PNL_Editor.Visible = true;
                _PNL_IdealValue.Visible = false;
                _UP_Editor.Update();
            }
        }

        void _DDL_AggregationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            _HL_EditMethod.NavigateUrl = "~/DSS/" + _DDL_AggregationMethodUrl.Items.FindByValue(_DDL_AggregationMethod.SelectedValue).Text + ".aspx?id=" + TreeView1.SelectedNode.Value;
            _PNL_Editor.Visible = true;

            ScaleEditTd.Visible = IsScaleVisible();
        }

        private bool IsScaleVisible()
        {
            if (TreeView1.SelectedNode == null)
                return false;
            var critId = int.Parse(TreeView1.SelectedNode.Value);
            using (var context = new DssDataContext())
            {
                bool hasChild = context.criterias.Any(x => x.parent_crit_id == critId);
                bool isScaleVisible = !hasChild;
                if (isScaleVisible)
                    ScaleEditHL.NavigateUrl = string.Format("Fuzzy.aspx?id={0}&isScale=1", critId);
                return isScaleVisible;
            }
        }
    }
}