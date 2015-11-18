using System;
using System.Web.UI;

namespace DSS.DSS
{
    public partial class Fuzzy : Page
    {
        protected string DefaultText = "Нечеткое ранжирование";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count == 0)
                return;
            
            int criteriaId = int.Parse(Request.QueryString["id"]);
            mfControl.CriteriaId = criteriaId;
        }
    }
}