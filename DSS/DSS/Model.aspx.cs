using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSS.DSS
{
    public partial class Model : System.Web.UI.Page
    {
        private static bool runStart;

        public static bool RunStart
        {
            get { return runStart; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //runStart = false;
            // заполнение ДДЛ списка моделей, см. Criteria.aspx.cs

            _IMG_RunModel.Click += new ImageClickEventHandler(_IMG_RunModel_Click);
        }

        void _IMG_RunModel_Click(object sender, ImageClickEventArgs e)
        {
            var smoModel = new PSS.Sample.SMOModel(null, "Модель СМО");

            smoModel.PERFORM(); // В коде модель переводится в состояние приостановлена

            runStart = true;
        }
    }
}