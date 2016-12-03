using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonModel.RandomStreamProducing;
using CommonModel.Kernel;

namespace DSS.PSS.Sample
{
    public partial class SMOModel : Model
    {
        public override bool MustStopRun(int variantCount, int runCount)
        {
            return (Time >= TP);
        }

        public override bool MustPerformNextVariant(int variantCount)
        {
            return variantCount < 1;
        }

        private enum crit_params
        {
            M1 = 0,
            M2 = 1,
            M3 = 2,
            Lambda1 = 3,
            Lambda2 = 4,
            Mu1 = 5,
            Mu2 = 6,
            P = 7,
            Tp = 8
        }

        public override void SetNextVariant(int variantCount)
        {
            try
            {
                TP = (int)DSS.Alternative.ParamsTable["Tp"];
                restartP = DSS.Alternative.ParamsTable["P"];
                MaxLength = new int[3];
                MaxLength[0] = (int)DSS.Alternative.ParamsTable["M1"];
                MaxLength[1] = (int)DSS.Alternative.ParamsTable["M2"];
                MaxLength[2] = (int)DSS.Alternative.ParamsTable["M3"];
                Lambda = new double[2];
                Lambda[0] = DSS.Alternative.ParamsTable["Lambda1"];
                Lambda[1] = DSS.Alternative.ParamsTable["Lambda2"];
                Mu = new double[2];
                Mu[0] = DSS.Alternative.ParamsTable["Mu1"];
                Mu[1] = DSS.Alternative.ParamsTable["Mu2"];
            }
            catch (FormatException ex)
            {

            }


            inFlowGenerator[0].A = Lambda[0];
            inFlowGenerator[1].A = Lambda[1];
            servFlowGenerator[0].Lyambda = Mu[0];
            servFlowGenerator[1].Lyambda = Mu[1];
        }

        public override bool MustPerformNextRun(int variantCount, int runCount)
        {
            return runCount < 1;
        }

        public override void StartModelling(int variantCount, int runCount)
        {
            KVZS.Value = 0;
            TimeIn_FirstFlow.Value = 0;
            TimeIn_SecondFlow.Value = 0;

            for (int i = 0; i < 3; i++)
            {
                KVZ[i].Value = 0;
                KPZ[i].Value = 0;
                queue[i].Clear();
            }

            for (int i = 0; i < 2; i++)
            {
                KZ[i].Value = false;
                TZKO[i].Value = 0;
            }

            for (int i = 0; i < 3; i++)
            {
                Variance_QueueCount[i].ResetCollector();
            }

            Variance_TimeIn_FirstFlow.ResetCollector();
            Min_TimeIn_FirstFlow.ResetCollector();
            Max_TimeIn_FirstFlow.ResetCollector();

            Variance_TimeIn_SecondFlow.ResetCollector();
            Min_TimeIn_SecondFlow.ResetCollector();
            Max_TimeIn_SecondFlow.ResetCollector();

            for (int i = 0; i < 2; i++)
            {
                Bool_Kanal[i].ResetCollector();
            }

            Event_K1 event_k1 = new Event_K1();
            event_k1.Nz = 1;
            event_k1.Np = 1;
            double lambda_1 = inFlowGenerator[0].GenerateValue();
            PlanEvent(event_k1, lambda_1);

            Event_K2 event_k2 = new Event_K2();
            event_k2.Nz = 2;
            event_k2.Np = 2;
            double lambda_2 = inFlowGenerator[1].GenerateValue();
            PlanEvent(event_k2, lambda_2);

            ModelTitle();
        }



        public override void FinishModelling(int variantCount, int runCount)
        {
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "==============================================================" + "</br>";
            DSS.Modeling.TraceString += "============Статистические результаты моделирования===========" + "</br>";
            DSS.Modeling.TraceString += "==============================================================" + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди 1: " + Variance_QueueCount[0].Mx.ToString() + "</br>";
            DSS.Modeling.TraceString += "==============================================================" + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди 2: " + Variance_QueueCount[1].Mx.ToString() + "</br>";
            DSS.Modeling.TraceString += "==============================================================" + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди 3: " + Variance_QueueCount[2].Mx.ToString() + "</br>";
            DSS.Modeling.TraceString += "==============================================================" + "</br>";

            DSS.Alternative.KormTable.Add("Avg_M1", Variance_QueueCount[0].Mx);
            DSS.Alternative.KormTable.Add("Avg_M2", Variance_QueueCount[1].Mx);
            DSS.Alternative.KormTable.Add("Avg_M3", Variance_QueueCount[2].Mx);

            DSS.Modeling.TraceString += "Среднее время пребывания в СМО заявок потока 1: " + Variance_TimeIn_FirstFlow.Mx + "</br>";
            DSS.Modeling.TraceString += "Минимальное время: " + Min_TimeIn_FirstFlow.Stat + "</br>";
            DSS.Modeling.TraceString += "Максимальное время: " + Max_TimeIn_FirstFlow.Stat + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
            DSS.Modeling.TraceString += "Среднее время пребывания в СМО заявок потока 2: " + Variance_TimeIn_SecondFlow.Mx + "</br>";
            DSS.Modeling.TraceString += "Минимальное время: " + Min_TimeIn_SecondFlow.Stat + "</br>";
            DSS.Modeling.TraceString += "Максимальное время: " + Max_TimeIn_SecondFlow.Stat + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
            DSS.Modeling.TraceString += "Оценка вероятности потери заявки в очереди 1: " + (double)KPZ[0].Value / KVZ[0].Value + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
            DSS.Modeling.TraceString += "Оценка вероятности потери заявки в очереди 2: " + (double)KPZ[1].Value / KVZ[1].Value + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
            DSS.Modeling.TraceString += "Оценка вероятнотси потери заявки в очереди 3: " + (double)KPZ[2].Value / KVZ[2].Value + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";

            DSS.Alternative.KormTable.Add("Freq_M1_lost", (double)KPZ[0].Value / KVZ[0].Value);
            DSS.Alternative.KormTable.Add("Freq_M2_lost", (double)KPZ[1].Value / KVZ[1].Value);
            DSS.Alternative.KormTable.Add("Freq_M3_lost", (double)KPZ[2].Value / KVZ[2].Value);

            DSS.Modeling.TraceString += "Оценка вероятности занятости канала 1: " + Bool_Kanal[0].TrueFrequency + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
            DSS.Modeling.TraceString += "Оценка вероятности занятости канала 2: " + Bool_Kanal[1].TrueFrequency + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";

            DSS.Alternative.KormTable.Add("Freq_KO1_busy", Bool_Kanal[0].TrueFrequency);
            DSS.Alternative.KormTable.Add("Freq_KO2_busy", Bool_Kanal[1].TrueFrequency);

            DSS.Modeling.TraceString += "Оценка интенсивности потока 1: " + (double)KVZ[0].Value / TP + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
            DSS.Modeling.TraceString += "Оценка интенсивности потока 2: " + (double)KVZ[2].Value / TP + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
            DSS.Modeling.TraceString += "========================Конец статистики======================</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
        }

        void ModelTitle()
        {
            DSS.Modeling.TraceString += "Рассматривается следующая СМО:</br>";
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "Интенсивности поступления = { " + 1.0 / Lambda[0] + " , " + 1.0 / Lambda[1] + " }" + "</br>";
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "Интенсивности обслуживания = { " + Mu[0] + " , " + Mu[1] + " }" + "</br>";
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "Размеры очередей = { " + MaxLength[0] + " , " + MaxLength[1] + " , " + MaxLength[2] + " }" + "</br>";
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "Время прогона модели = " + TP + "</br>";
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "Вероятность необходимости дообслуживания заявки = " + restartP + "</br>";
            DSS.Modeling.TraceString += "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
        }

        void ModelStatus()
        {
            DSS.Modeling.TraceString += "==============================================================</br>";
            DSS.Modeling.TraceString += "Суммарное количество заявко в СМО = " + KVZS.Value + "</br>";
            DSS.Modeling.TraceString += "Занятость каналов { " + KZ[0].Value + " , " + KZ[1].Value + " }" + "</br>";
            DSS.Modeling.TraceString += "Количество заявок, пришедших на вход каждой очереди { " + KVZ[0].Value + " , " + KVZ[1].Value + " , " + KVZ[2].Value + " }" + "</br>";
            DSS.Modeling.TraceString += "Количество потерянных заявок { " + KPZ[0].Value + " , " + KPZ[1].Value + " , " + KPZ[2].Value + " }" + "</br>";
            DSS.Modeling.TraceString += "Количество заявок в очередях { " + queue[0].Count.Value + " , " + queue[1].Count.Value + " , " + queue[2].Count.Value + " }" + "</br>";
            DSS.Modeling.TraceString += "==============================================================</br>";
        }
    }


}