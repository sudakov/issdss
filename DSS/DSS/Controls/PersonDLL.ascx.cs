using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSS.DSS.Controls
{
    public partial class PersonDLL : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.AbsoluteUri.Contains("/DSS/Value.aspx") && ((Label)Parent.FindControl("_LBL_ViewAllPerson")).Text != "0")
            {
                _PNL_PersonDDL.Visible = true;
                _DDL_Person_DataBind();
            }

            _DDL_Person.SelectedIndexChanged += new EventHandler(_DDL_Person_SelectedIndexChanged);
        }

        void _DDL_Person_SelectedIndexChanged(object sender, EventArgs e)
        {
            Context.Response.Cookies.Add(new HttpCookie("PersonID", _DDL_Person.SelectedItem.Value));
            Response.Redirect("Value.aspx");
        }

        void _DDL_Person_DataBind()
        {
            if (!IsPostBack)
            {
                using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                {
                    SqlCommand Command = new SqlCommand("dbo.issdss_personDDL_Read_All", Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    if (Context.Request.Cookies["TaskID"] != null)
                        Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                    Connection.Open();
                    SqlDataReader Reader = Command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        _DDL_Person.DataSource = Reader;
                        _DDL_Person.DataBind();
                        _DDL_Person.Items.Insert(0, new ListItem("Показать все записи", "0"));
                        if (Context.Request.Cookies["PersonID"] != null && Context.Request.Cookies["PersonID"].Value != "-1")
                            _DDL_Person.SelectedValue = Context.Request.Cookies["PersonID"].Value;
                        else
                            _DDL_Person.SelectedValue = Context.Request.Cookies["UserID"].Value;
                        //{
                        //    Context.Response.Cookies.Add(new HttpCookie("PersonID", _DDL_Person.SelectedItem.Value));
                        //    Response.Redirect(Request.Url.AbsoluteUri);
                        //}
                    }
                    else
                        _PNL_PersonDDL.Visible = false;
                }
            }
        }
    }
}