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
    public partial class VU : Model
    {

        public override void SetNextVariant(int variantCount)
        {
            #region  Задание параметров модели для текущего варианта
            param1 = 17981;
            param2 = 18194;
            param3 = 11983;
            #endregion

            #region Установка параметров законов распределения
            Gener_Vhod.Lyambda = Vhod_Lamd;
            (Gener_Vhod.BPN as GeneratedBaseRandomStream).Seed = param1;

            Gener_RazmerVvod.A = RazmerVvoda_L;
            Gener_RazmerVvod.B = RazmerVvoda_R;
            (Gener_RazmerVvod.BPN as GeneratedBaseRandomStream).Seed = param2;

            Gener_RazmerVyvoda.Mx = RazmerVyvoda_MO;
            Gener_RazmerVyvoda.Sigma = RazmerVyvoda_SKO;
            (Gener_RazmerVyvoda.BPN as GeneratedBaseRandomStream).Seed = param3;

            UVD.SetNextVariant(1);
            UVD.StartModelling(1, 1);

            UR.SetNextVariant(1);
            UR.StartModelling(1, 1);

            UV.SetNextVariant(1);
            UV.StartModelling(1, 1);
            #endregion
        }

        public override void StartModelling(int variantCount, int runCount)
        {

            #region Задание начальных значений модельных переменных и объектов
            KVZ.Value = 0;
            KOZ.Value = 0;
            TOZ.Value = 0;
            KPZ[0] = 0; KPZ[1] = 0; KPZ[2] = 0;
            #endregion


            #region Cброс сборщиков статистики
            Variance_TOZ.ResetCollector();
            Min_TOZ.ResetCollector();
            Max_TOZ.ResetCollector();
            His_TOZ.ResetCollector();
            #endregion

        }

        public void TraceModel()
        {
            UVD.TraceModel();
            UR.TraceModel();
            UV.TraceModel();
        }
    }
}