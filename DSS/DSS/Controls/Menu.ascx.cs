using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSS.DSS.Controls
{
    public partial class Menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((Label)Parent.FindControl("_LBL_TaskView")).Text == "0")
                _PNL_Task.Visible = false;
            if (((Label)Parent.FindControl("_LBL_AltView")).Text == "0")
                _PNL_Alternative.Visible = false;
            if (((Label)Parent.FindControl("_LBL_CritView")).Text == "0")
                _PNL_Criteria.Visible = false;
            if (((Label)Parent.FindControl("_LBL_ResView")).Text == "0")
                _PNL_Resource.Visible = false;
            if (((Label)Parent.FindControl("_LBL_ValueView")).Text == "0")
                _PNL_Value.Visible = false;
            if (((Label)Parent.FindControl("_LBL_RankResultView")).Text == "0")
                _PNL_Ranking.Visible = false;
            if (((Label)Parent.FindControl("_LBL_PlanView")).Text == "0")
                _PNL_Plan.Visible = false;
            if (((Label)Parent.FindControl("_LBL_PersonView")).Text == "0")
                _PNL_Person.Visible = false;
            if (((Label)Parent.FindControl("_LBL_RoleView")).Text == "0")
                _PNL_Role.Visible = false;
            if (((Label)Parent.FindControl("_LBL_RoleView")).Text == "0")
                _PNL_Fuzzy.Visible = false;
        }
    }
}