using System;
using System.Web.UI;

namespace DSS.DSS.Controls
{
    public partial class MembershipFunctionControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FunctionName = "Скорость автомобиля";
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            DataBind();
        }

        public string FunctionName { get; set; }
    }
}