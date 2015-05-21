using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSS.DSS.Controls
{
    public partial class Identity : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _LBL_Name.Text = ((Label)Parent.FindControl("_LBL_Name")).Text;
            _LBL_Task.Text = ((Label)Parent.FindControl("_LBL_Task")).Text;
            _BTN_Exit.Click += new EventHandler(_BTN_Exit_Click);
        }

        void _BTN_Exit_Click(object sender, EventArgs e)
        {
            if (Context.Request.Cookies["UserID"] != null)
                Context.Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
            if (Context.Request.Cookies["TaskID"] != null)
                Context.Response.Cookies["TaskID"].Expires = DateTime.Now.AddDays(-1);
            if (Context.Request.Cookies["PersonID"] != null)
                Context.Response.Cookies["PersonID"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("/DSS/Default.aspx");
        }
    }
}