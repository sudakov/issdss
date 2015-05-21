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
    public partial class Assessment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    int i = 0;
                    int criteriaID = 0;

                    //
                    // Получение из базы экспертных оценок текущей альтернативы
                    //
                    DataTable dtValues = new DataTable();
                    DataRow drValues;
                    DataTable dtDDL = null;
                    DataRow drDDL;

                    dtValues.Columns.Add("c_id", typeof(String));
                    dtValues.Columns.Add("c_name", typeof(String));
                    dtValues.Columns.Add("value_for_view", typeof(String));
                    dtValues.Columns.Add("is_resourse", typeof(String));
                    dtValues.Columns.Add("current_scale_id", typeof(String));
                    dtValues.Columns.Add("ddl_data_source", typeof(DataTable));

                    SqlCommand Command = new SqlCommand("dbo.issdss_crit_scale_Read_All", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.Cookies["TaskID"] != null)
                        Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                    else
                        _BTN_Save.Enabled = false;
                    if (Context.Request.Cookies["PersonID"] != null)
                        Command.Parameters.AddWithValue("@PersonID", Context.Request.Cookies["UserID"].Value);
                    else
                        _BTN_Save.Enabled = false;
                    if (Context.Request.QueryString["id"] != null)
                        Command.Parameters.AddWithValue("@AlternativeID", Convert.ToInt32(Context.Request.QueryString["id"]));
                    else
                        _BTN_Save.Enabled = false;
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    try
                    {
                        while (Reader.Read())
                        {
                            if (i == 0)
                            {
                                if (Reader["c_id"].ToString() == "0")
                                    _TB_Header.Text = "Экспертиза КЭ: " + Reader["value_for_view"].ToString();
                                else
                                    _TB_Header.Text = "КЭ не выбран";
                                i++;
                                continue;
                            }
                            else
                            {
                                if (criteriaID != Convert.ToInt32(Reader["c_id"]))
                                {
                                    drValues = dtValues.NewRow();
                                    drValues["c_id"] = Reader["c_id"].ToString();
                                    drValues["c_name"] = Reader["c_name"].ToString();
                                    drValues["value_for_view"] = Helper.MyMath.Round(Reader["value_for_view"].ToString());
                                    drValues["is_resourse"] = Reader["is_resourse"].ToString();
                                    drValues["current_scale_id"] = Reader["current_scale_id"].ToString();

                                    dtDDL = new DataTable();
                                    dtDDL.Columns.Add("id", typeof(String));
                                    dtDDL.Columns.Add("name", typeof(String));

                                    if (Reader["current_scale_id"].ToString() == "")
                                    {
                                        drDDL = dtDDL.NewRow();
                                        drDDL["id"] = "0";
                                        drDDL["name"] = "";
                                        dtDDL.Rows.Add(drDDL);
                                    }

                                    drValues["ddl_data_source"] = dtDDL;
                                    dtValues.Rows.Add(drValues);
                                    criteriaID = Convert.ToInt32(Reader["c_id"]);
                                }
                                if (dtDDL != null && Reader["scale_id"].ToString() != "")
                                {
                                    drDDL = dtDDL.NewRow();
                                    drDDL["id"] = Reader["scale_id"].ToString();
                                    drDDL["name"] = Reader["value_for_view"].ToString();
                                    dtDDL.Rows.Add(drDDL);
                                }
                            }
                        }
                    }
                    finally
                    {
                        Reader.Close();
                    }
                    Connection.Close();

                    //
                    // Получение из базы дерева критериев в правильном порядке
                    //
                    DataTable dtCriteria = new DataTable();
                    DataRow drCriteria;

                    dtCriteria.Columns.Add("c_id", typeof(String));
                    dtCriteria.Columns.Add("c_name", typeof(String));
                    dtCriteria.Columns.Add("lev", typeof(String));
                    dtCriteria.Columns.Add("is_parent", typeof(String));
                    dtCriteria.Columns.Add("description", typeof(String));

                    Command = new SqlCommand("dbo.issdss_criteria_Read_Names_by_Order", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.Cookies["TaskID"] != null)
                        Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                    Connection.Open();
                    Reader = Command.ExecuteReader();
                    try
                    {
                        while (Reader.Read())
                        {
                            drCriteria = dtCriteria.NewRow();
                            drCriteria["c_id"] = Reader["id"].ToString();
                            drCriteria["c_name"] = Reader["name"].ToString();
                            drCriteria["lev"] = Reader["lev"].ToString();
                            drCriteria["is_parent"] = Reader["is_parent"].ToString();
                            drCriteria["description"] = Reader["description"].ToString();
                            dtCriteria.Rows.Add(drCriteria);
                        }
                    }
                    finally
                    {
                        Reader.Close();
                    }
                    Connection.Close();

                    //
                    // Сложение двух полученных таблиц
                    //
                    DataTable dtAll = new DataTable();
                    DataRow drAll;
                    dtDDL = new DataTable();
                    dtDDL.Columns.Add("id", typeof(String));
                    dtDDL.Columns.Add("name", typeof(String));

                    dtAll.Columns.Add("c_id", typeof(String));
                    dtAll.Columns.Add("c_name", typeof(String));
                    dtAll.Columns.Add("lev", typeof(String));
                    dtAll.Columns.Add("is_parent", typeof(String));
                    dtAll.Columns.Add("description", typeof(String));
                    dtAll.Columns.Add("value_for_view", typeof(String));
                    dtAll.Columns.Add("is_resourse", typeof(String));
                    dtAll.Columns.Add("current_scale_id", typeof(String));
                    dtAll.Columns.Add("ddl_data_source", typeof(DataTable));

                    int dtValuesRowsCount = dtValues.Rows.Count;
                    int dtCriteriaRowsCount = dtCriteria.Rows.Count;
                    int j;
                    for (i = 0; i < dtCriteriaRowsCount; i++)
                    {
                        drAll = dtAll.NewRow();
                        drAll["c_id"] = dtCriteria.Rows[i]["c_id"];
                        drAll["c_name"] = dtCriteria.Rows[i]["c_name"];
                        drAll["lev"] = dtCriteria.Rows[i]["lev"];
                        drAll["is_parent"] = dtCriteria.Rows[i]["is_parent"];
                        drAll["description"] = dtCriteria.Rows[i]["description"];
                        drAll["value_for_view"] = "";
                        drAll["is_resourse"] = "";
                        drAll["current_scale_id"] = "";
                        drAll["ddl_data_source"] = dtDDL;
                        for (j = 0; j < dtValuesRowsCount; j++)
                        {
                            if (drAll["c_id"].ToString() == dtValues.Rows[j]["c_id"].ToString())
                            {
                                drAll["value_for_view"] = dtValues.Rows[j]["value_for_view"];
                                drAll["is_resourse"] = dtValues.Rows[j]["is_resourse"];
                                drAll["current_scale_id"] = dtValues.Rows[j]["current_scale_id"];
                                drAll["ddl_data_source"] = dtValues.Rows[j]["ddl_data_source"];
                                break;
                            }
                        }
                        dtAll.Rows.Add(drAll);
                    }
                    
                    _RP_Main.DataSource = dtAll;
                    _RP_Main.DataBind();
                }
            }
            _BTN_Save.Click += new EventHandler(_BTN_Save_Click);
            _BTN_Cancel.Click += new EventHandler(_BTN_Cancel_Click);
            _RP_Main.Load += new EventHandler(_RP_Main_Load);
        }

        void _RP_Main_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int i = 0; i < _RP_Main.Items.Count; i++)
                {
                    if (((Label)_RP_Main.Items[i].FindControl("_LBL_IsParent")).Text == "1")
                    {
                        ((Panel)_RP_Main.Items[i].FindControl("_PNL_Group")).Visible = true;
                        ((Panel)_RP_Main.Items[i].FindControl("_PNL_Criteria")).Visible = false;
                    }
                    else
                    {
                        ((Panel)_RP_Main.Items[i].FindControl("_PNL_Group")).Visible = false;
                        ((Panel)_RP_Main.Items[i].FindControl("_PNL_Criteria")).Visible = true;
                    }
                    if (((DropDownList)_RP_Main.Items[i].FindControl("_DDL_Value")).Items.Count > 1)
                    {
                        string val = ((Label)_RP_Main.Items[i].FindControl("_LBL_DDL_SelectedID")).Text;
                        if (val != "")
                            ((DropDownList)_RP_Main.Items[i].FindControl("_DDL_Value")).Items.FindByValue(val).Selected = true;
                        ((TextBox)_RP_Main.Items[i].FindControl("_TB_Value")).Visible = false;
                    }
                    else
                        ((DropDownList)_RP_Main.Items[i].FindControl("_DDL_Value")).Visible = false;
                    if (((Label)_RP_Main.Items[i].FindControl("_LBL_IsResourse")).Text == "1")
                    {
                        ((DropDownList)_RP_Main.Items[i].FindControl("_DDL_Value")).Visible = false;
                        ((TextBox)_RP_Main.Items[i].FindControl("_TB_Value")).Visible = true;
                        ((TextBox)_RP_Main.Items[i].FindControl("_TB_Value")).Enabled = false;
                    }
                }
            }
        }

        void _BTN_Save_Click(object sender, EventArgs e)
        {
            string value;
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_crit_value_Update", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@person_id", Context.Request.Cookies["UserID"].Value);
                Command.Parameters.AddWithValue("@alternative_id", Convert.ToInt32(Context.Request.QueryString["id"]));
                string criteriaXML = "<crit_value>";
                for (int i = 0; i < _RP_Main.Items.Count; i++)
                {
                    if (((Label)_RP_Main.Items[i].FindControl("_LBL_IsResourse")).Text == "1")
                        continue;
                    criteriaXML += "<row>";
                    criteriaXML += "<criteria_id>" + ((Label)_RP_Main.Items[i].FindControl("_LBL_ID")).Text + "</criteria_id>";
                    if (((DropDownList)_RP_Main.Items[i].FindControl("_DDL_Value")).Visible)
                        criteriaXML += "<value>" + ((DropDownList)_RP_Main.Items[i].FindControl("_DDL_Value")).SelectedValue + "</value>";
                    else
                    {
                        value = ((TextBox)_RP_Main.Items[i].FindControl("_TB_Value")).Text;
                        if (value.Contains(","))
                            value = value.Replace(',', '.');
                        if (value == "")
                            value = "0";
                        criteriaXML += "<value>" + value + "</value>";
                    }
                    criteriaXML += "</row>";
                }
                criteriaXML += "</crit_value>";
                Command.Parameters.AddWithValue("@d", criteriaXML);
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            Response.Redirect("Value.aspx");
        }

        void _BTN_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Value.aspx");
        }
    }
}