using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonModel.StatisticsCollecting;
using CommonModel.RandomStreamProducing;
using CommonModel.Collections;
using CommonModel.Kernel;
using System.Runtime.Serialization;

namespace DSS.PSS.VS
{
    partial class KPD : Model
    {
        //Задаём параметры модели для каждого варианта прогона
        public override void SetNextVariant(int variantCount)
        {
            KOEF = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.КоэфтКПД]);
        }

        //Задаём начальное состояние модели
        public override void StartModelling(int variantCount, int runCount)
        {
            #region Задание начальных значений модельных переменных и объектов
            KPZ.Value = 0;
            KOZ.Value = 0;
            Zanyatost.Ref = null;
            #endregion


            #region Cброс сборщиков статистики
            Variance_Q_Vhod.ResetCollector();
            Variance_Q_Vozvrat.ResetCollector();
            Min_Q_Vhod.ResetCollector();
            Min_Q_Vozvrat.ResetCollector();
            Max_Q_Vozvrat.ResetCollector();
            Max_Q_Vhod.ResetCollector();
            Zanyato.ResetCollector();
            Q_Vhod.Clear();
            Q_Vozvrat.Clear();
            #endregion

            Zanyatost.Ref = null;


        }

        //Записи в трассировку
        public void TraceModel()
        {
        }
    }
}