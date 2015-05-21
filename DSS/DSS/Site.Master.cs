using System;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSS.DSS
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Если юзер залогинен (т.е. в сессии висит его UserID), то лезет в базу, читает его имя и разрешения и выводит в _LBL_'ы
            if (Context.Request.Cookies["UserID"] != null)
            {
                int id;
                Authority1.Visible = false;
                Identity1.Visible = true;
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_permission_Read", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.Cookies["TaskID"] != null)
                        Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                    Command.Parameters.AddWithValue("PersonID", Context.Request.Cookies["UserID"].Value);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    Reader.Read();
                    _LBL_Name.Text = Reader["name"].ToString();
                    Reader.Read();
                    _LBL_Task.Text = Reader["task"].ToString();

                    while (Reader.Read())
                    {
                        id = Convert.ToInt32(Reader["permission_id"]);
                        switch (id)
                        {
                            case 1: _LBL_TaskView.Text = id.ToString(); break;
                            case 11: _LBL_AltView.Text = id.ToString(); break;
                            case 21: _LBL_CritView.Text = id.ToString(); break;
                            case 31: _LBL_ValueView.Text = id.ToString(); break;
                            case 41: _LBL_RankResultView.Text = id.ToString(); break;
                            case 51: _LBL_PersonView.Text = id.ToString(); break;
                            case 61: _LBL_RoleView.Text = id.ToString(); break;
                            case 71: _LBL_PlanView.Text = id.ToString(); break;
                            case 81: _LBL_ViewAllPerson.Text = id.ToString(); break;
                            case 91: _LBL_ResView.Text = id.ToString(); break;
                        }
                    }
                }
            }
            else
            {
                Authority1.Visible = true;
                Identity1.Visible = false;
                //if (!Request.Url.AbsoluteUri.Contains("Default.aspx"))
                //    Response.Redirect("Default.aspx");
            }
        }
    }
}