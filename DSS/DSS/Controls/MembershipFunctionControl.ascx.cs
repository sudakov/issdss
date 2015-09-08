using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using Newtonsoft.Json;

namespace DSS.DSS.Controls
{
    public partial class MembershipFunctionControl : UserControl
    {
        public int CriteriaId { get; set; }
        protected string FunctionName { get; set; }
        protected string Categories { get; set; }
        protected string CriteriaName { get; set; }
        protected string Data { get; set; }

        public MembershipFunctionControl()
        {
            CriteriaId = -1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FunctionName = "Скорость автомобиля";
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            MyDataBind();
            DataBind();
        }

        private void MyDataBind()
        {
            if (CriteriaId < 0)
            {
                Visible = false;
                return;
            }
            using (var context = new DssDataContext())
            {
                var criteria = context.criterias.First(x => x.id == CriteriaId);
                FunctionName = criteria.name;
                var dependedCrit = context.criterias.First(x => x.parent_crit_id == CriteriaId);
                CriteriaName = dependedCrit.name;

                var depended = GetDepended(context);

                Categories = string.Join(",", depended.Select(x => "'" + x.name + "'"));
                Data = string.Join(",", depended.Select(x => x.value.ToString(CultureInfo.InvariantCulture)));
            }
            hfKeys.Value = "[" + Categories + "]";
        }

        private List<crit_fuzzy> GetDepended(DssDataContext context)
        {
            return (from critFuzzy in context.crit_fuzzies
                    where critFuzzy.critId == CriteriaId
                    orderby critFuzzy.position
                    select critFuzzy).ToList();
        }

        protected void SaveOnClick(object sender, EventArgs e)
        {
            string[] keys = JsonConvert.DeserializeObject<string[]>(hfKeys.Value);
            double[] values = JsonConvert.DeserializeObject<double[]>(hfData.Value);
            var dictionary = new Dictionary<string, double>(keys.Length);
            for (int i = 0; i < keys.Length; i++)
            {
                dictionary.Add(keys[i], values[i]);
            }

            using (var context = new DssDataContext())
            {
                var depended = GetDepended(context);
                bool hasChanges = false;
                foreach (var critFuzzy in depended)
                {
                    double newValue = dictionary[critFuzzy.name];
                    if (Math.Abs(critFuzzy.value - newValue) > double.Epsilon)
                    {
                        critFuzzy.value = newValue;
                        hasChanges = true;
                    }
                }
                if (hasChanges)
                    context.SubmitChanges();
            }
        }
    }
}