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

                bool isNumber = criteria.is_number != 0;
                ViewState.Add("isNumber", isNumber);

                if (isNumber)
                {
                    var depended = GetDependedValues(context);  
                    Categories = string.Join(",", depended.Select(x => "'" + x.value + "'"));
                    Data = string.Join(",", depended.Select(x => x.value.Value.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    var depended = GetDependedScale(context);
                    Categories = string.Join(",", depended.Select(x => "'" + x.name + "'"));
                    Data = string.Join(",", depended.Select(x => x.rank.Value.ToString(CultureInfo.InvariantCulture)));
                }
            }
            hfKeys.Value = "[" + Categories + "]";
        }

        private List<crit_value> GetDependedValues(DssDataContext context)
        {
            return (from crit in context.crit_values
                    where crit.criteria_id == CriteriaId
                    orderby crit.value
                    select crit).ToList();
        }

        private List<crit_scale> GetDependedScale(DssDataContext context)
        {
            return (from crit in context.crit_scales
                    where crit.criteria_id == CriteriaId
                    orderby crit.ord
                    select crit).ToList();
        }

        protected void SaveOnClick(object sender, EventArgs e)
        {
            string[] keys = JsonConvert.DeserializeObject<string[]>(hfKeys.Value);
            decimal[] values = JsonConvert.DeserializeObject<decimal[]>(hfData.Value);
            var dictionary = new Dictionary<string, decimal>(keys.Length);
            for (int i = 0; i < keys.Length; i++)
            {
                dictionary.Add(keys[i], values[i]);
            }

            using (var context = new DssDataContext())
            {
                bool hasChanges = false;
                bool isNumber = (bool) ViewState["isNumber"];

                if (isNumber)
                {
//TODO
                }
                else
                {
                    var depended = GetDependedScale(context);
                    foreach (var critFuzzy in depended)
                    {
                        decimal newValue = dictionary[critFuzzy.name];
                        if (critFuzzy.rank.Value != newValue)
                        {
                            critFuzzy.rank = newValue;
                            hasChanges = true;
                        }
                    }
                }
                if (hasChanges)
                    context.SubmitChanges();
            }
        }
    }
}