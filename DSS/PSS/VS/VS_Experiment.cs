using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonModel.StatisticsCollecting;
using CommonModel.RandomStreamProducing;
using CommonModel.Collections;
using CommonModel.Kernel;

namespace DSS.PSS.VS
{
    public partial class VS : Model 
    {

        //Условие завершения прогона модели True - завершить прогон. По умолчанию false. 
        public override bool MustStopRun(int variantCount, int runCount)
        {
            TraceModel();
            return (Time >= MaxT);
        }

        //установка метода перебора вариантов модели
        public override bool MustPerformNextVariant(int variantCount)
        {
            //используем один вариант модели
            return variantCount < 1;
        }

        

        //Задание начального состояния модели для нового варианта модели
        public override void SetNextVariant(int variantCount)
        {
            #region  Задание параметров модели для текущего варианта
            
            MaxT = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.ВремяПрогона]);         //время моделирования (в секундах)
            NumVar = 2;
            trace = 1;
            UZEL[0].trace = 1;
            UZEL[1].trace = 1;
            UZEL[2].trace = 1;
            UZEL[0].UV.trace = 1;
            UZEL[0].UVD.trace = 1;
            UZEL[0].UR.trace = 1;
            UZEL[1].UV.trace = 1;
            UZEL[1].UVD.trace = 1;
            UZEL[1].UR.trace = 1;
            UZEL[2].UV.trace = 1;
            UZEL[2].UVD.trace =1;
            UZEL[2].UR.trace = 1;
            KANAL[0].trace = 1;
            KANAL[1].trace = 1;
            KANAL[2].trace = 1;
            #endregion

                #region Установка параметров законов распределения
                //Для узлов
                #region Задание параметров для первого узла
                UZEL[0].Vhod_Lamd = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел1Лямбда]);
                UZEL[0].RazmerVvoda_L = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел1L]);
                UZEL[0].RazmerVvoda_R = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел1R]);
                UZEL[0].RazmerVyvoda_MO = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел1МО]);
                UZEL[0].RazmerVyvoda_SKO = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел1СКО]);
                #endregion

                #region Задание параметров для второго узла
                UZEL[1].Vhod_Lamd = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел2Лямбда]);
                UZEL[1].RazmerVvoda_L = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел2L]);
                UZEL[1].RazmerVvoda_R = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел2R]);
                UZEL[1].RazmerVyvoda_MO = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел2МО]);
                UZEL[1].RazmerVyvoda_SKO = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел2СКО]);
                #endregion

                #region Задание параметров для третьего узла
                UZEL[2].Vhod_Lamd = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел3Лямбда]);
                UZEL[2].RazmerVvoda_L = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел3L]);
                UZEL[2].RazmerVvoda_R = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел3R]);
                UZEL[2].RazmerVyvoda_MO = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел3МО]);
                UZEL[2].RazmerVyvoda_SKO = double.Parse(DSS.Alternative.ParamsList[(int)PL.paramList.Узел3СКО]);
                #endregion
            
                UZEL[0].SetNextVariant(1);
                UZEL[0].StartModelling(1, 1);
                UZEL[1].SetNextVariant(1);
                UZEL[1].StartModelling(1, 1);
                UZEL[2].SetNextVariant(1);
                UZEL[2].StartModelling(1, 1);
            
                KANAL[0].SetNextVariant(1);
                KANAL[0].StartModelling(1,1);
                KANAL[1].SetNextVariant(1);
                KANAL[1].StartModelling(1, 1);
                KANAL[2].SetNextVariant(1);
                KANAL[2].StartModelling(1,1);
                #endregion

        }

        //true - продолжить выполнение ПРОГОНОВ модели;
        //false - прекратить выполнение ПРОГОНОВ модели. по умолчению false.
        public override bool MustPerformNextRun(int variantCount, int runCount)
        {
            return runCount < 1; //выполняем 1 прогон модели
        }


        //задание начальных параметров моделирования
        public override void StartModelling(int variantCount, int runCount)
        {


            #region Задание начальных значений модельных переменных и объектов
            KVZ.Value = 0;
            KOZ.Value = 0;
            TOZ.Value = 0;
            #endregion

            #region Cброс сборщиков статистики
            Variance_TOZ.ResetCollector();
            Min_TOZ.ResetCollector();
            Max_TOZ.ResetCollector();
            His_TOZ.ResetCollector();
            #endregion

          /*  #region Задание начальных значений модельных переменных и объектов
            KVZ.Value = 0;
            KOZ.Value = 0;
            TOZ.Value = 0;
            #endregion
            */

            #region Планирование начальных событий
            TraceModel();//

            var potok1 = new Event_1_Vhod_VS();//Создаем новый поток
            Zayavka Z1 = new Zayavka();//Создаем новую заявку
            Z1.UzelVhoda = UZEL[0];//Первый поток работает с первым узлом
            Z1.Num = KVZ.Value+1;
            Z1.RazmerVvod = Z1.UzelVhoda.Gener_RazmerVvod.GenerateValue();
            Z1.RazmerVyvod = Z1.UzelVhoda.Gener_RazmerVyvoda.GenerateValue();
            potok1.Z = Z1;//Передаем заявку в поток
            double dt1 = UZEL[0].Gener_Vhod.GenerateValue();
            PlanEvent(potok1,dt1);//Планируем событие
        
            
            var potok2 = new Event_1_Vhod_VS();
            Zayavka Z2 = new Zayavka();
            Z2.UzelVhoda = UZEL[1];
            Z2.Num = KVZ.Value + 2;
            Z2.RazmerVvod = Z2.UzelVhoda.Gener_RazmerVvod.GenerateValue();
            Z2.RazmerVyvod = Z2.UzelVhoda.Gener_RazmerVyvoda.GenerateValue();
            potok2.Z=Z2;
            double dt2 = UZEL[1].Gener_Vhod.GenerateValue();
            PlanEvent(potok2, dt2);


            var potok3 = new Event_1_Vhod_VS();
            Zayavka Z3 = new Zayavka();
            Z3.Num = KVZ.Value + 3;
            Z3.UzelVhoda = UZEL[2];
            Z3.RazmerVvod = Z3.UzelVhoda.Gener_RazmerVvod.GenerateValue();
            Z3.RazmerVyvod = Z3.UzelVhoda.Gener_RazmerVyvoda.GenerateValue();
            potok3.Z=Z3;
            double dt3 = UZEL[2].Gener_Vhod.GenerateValue();
            PlanEvent(potok3,dt3);
           
            #endregion
            
            //Печать заголовка строки состояния модели
            TraceModelHeader(variantCount);
        }

        //-------установка действий по завершению моделированию варианта--------
        public override void FinishModelling(int variantCount, int runCount)
        {
            //Финальные расчеты                
            //Вывод результатов моделирования
            DSS.Modeling.TraceString += "-----------------------------------------------------------------------------------" + "</br>";
            DSS.Modeling.TraceString += "                        Результаты имитационного моделирования                                   " + "</br>";
            DSS.Modeling.TraceString += "Среднее время пребывания заявки в системе " + Variance_TOZ.Mx + "</br>";
            DSS.Modeling.TraceString += "Минимальное время нахождения заявки в системе " + Min_TOZ.Stat + "</br>";
            DSS.Modeling.TraceString += "Максимальное время нахождения заявки в системе " + Max_TOZ.Stat + "</br>";
            DSS.Modeling.TraceString += "Количество обработанных заявок:" + KOZ.Value + "</br>";
            DSS.Modeling.TraceString += "Статистика по ВУ1:" + "</br>";
            DSS.Modeling.TraceString += "Количество обработанных заявок: " + UZEL[0].UV.KOZ.Value + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УВД: " + Math.Round(UZEL[0].UVD.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УОД: " + Math.Round(UZEL[0].UR.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УВР: " + Math.Round(UZEL[0].UV.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность передачи заявки в ВУ2: " + Math.Round(Convert.ToDouble(UZEL[0].KPZ[1]) / Convert.ToDouble(UZEL[0].KVZ.Value), 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность передачи заявки в ВУ3: " + Math.Round(Convert.ToDouble(UZEL[0].KPZ[2]) / Convert.ToDouble(UZEL[0].KVZ.Value), 3) + "</br>";
            DSS.Modeling.TraceString += "Среднее время пребывания заявки в ВУ: " + Math.Round(UZEL[0].Variance_TOZ.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УВД: " + Math.Round(UZEL[0].UVD.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УОД: " + Math.Round(UZEL[0].UR.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УВР: " + Math.Round(UZEL[0].UV.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Статистика по ВУ2:" + "</br>";
            DSS.Modeling.TraceString += "Количество обработанных заявок: " + UZEL[1].UV.KOZ.Value + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УВД: " + Math.Round(UZEL[1].UVD.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УОД: " + Math.Round(UZEL[1].UR.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УВР: " + Math.Round(UZEL[1].UV.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность передачи заявки в ВУ2: " + Math.Round(Convert.ToDouble(UZEL[1].KPZ[0]) / Convert.ToDouble(UZEL[1].KVZ.Value), 2) + "</br>";
            DSS.Modeling.TraceString += "Вероятность передачи заявки в ВУ3: " + Math.Round(Convert.ToDouble(UZEL[1].KPZ[2]) / Convert.ToDouble(UZEL[1].KVZ.Value), 2) + "</br>";
            DSS.Modeling.TraceString += "Среднее время пребывания заявки в ВУ: " + Math.Round(UZEL[1].Variance_TOZ.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УВД: " + Math.Round(UZEL[1].UVD.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УОД: " + Math.Round(UZEL[1].UR.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УВР: " + Math.Round(UZEL[1].UV.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Статистика по ВУ3:" + "</br>";
            DSS.Modeling.TraceString += "Количество обработанных заявок: " + UZEL[2].UV.KOZ.Value + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УВД: " + Math.Round(UZEL[2].UVD.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УОД: " + Math.Round(UZEL[2].UR.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости УВР: " + Math.Round(UZEL[2].UV.Zanyto.TrueFrequency, 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность передачи заявки в ВУ2: " + Math.Round(Convert.ToDouble(UZEL[2].KPZ[0]) / Convert.ToDouble(UZEL[2].KVZ.Value), 3) + "</br>";
            DSS.Modeling.TraceString += "Вероятность передачи заявки в ВУ3: " + Math.Round(Convert.ToDouble(UZEL[2].KPZ[1]) / Convert.ToDouble(UZEL[2].KVZ.Value), 3) + "</br>";
            DSS.Modeling.TraceString += "Среднее время пребывания заявки в ВУ: " + Math.Round(UZEL[2].Variance_TOZ.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УВД: " + Math.Round(UZEL[2].UVD.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УОД: " + Math.Round(UZEL[2].UR.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Средняя длина очереди к УВР: " + Math.Round(UZEL[2].UV.Variance_QueCount.Mx, 3) + "</br>";
            DSS.Modeling.TraceString += "Занятость КПД" + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости КПД 1-2 : " +Math.Round( KANAL[0].Zanyato.TrueFrequency,6) + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости КПД 1-3 :" + Math.Round(KANAL[1].Zanyato.TrueFrequency, 6) + "</br>";
            DSS.Modeling.TraceString += "Вероятность занятости КПД 2-3 :" + Math.Round(KANAL[2].Zanyato.TrueFrequency, 6) + "</br>";
            DSS.Modeling.TraceString += "-----------------------------------------------------------------------------------" + "</br>";
        }

        void TraceModelHeader(int variantCount)
        {
            DSS.Modeling.TraceString += "-----------------------------------------------------------------------------------" + "</br>";
            DSS.Modeling.TraceString += "                             ПАРАМЕТРЫ МОДЕЛИ      прогон №"+variantCount +"                                  " + "</br>";
            DSS.Modeling.TraceString += "Вариант условий передачи : " + NumVar + "</br>";
            DSS.Modeling.TraceString += "Время моделирования "+MaxT + "</br>";
            DSS.Modeling.TraceString += "Интенсивность входного потока заявок в ВУ1 "+UZEL[0].Vhod_Lamd + "</br>";
            DSS.Modeling.TraceString += "Левая граница распределения объема данных ввода (в ВУ1) "+UZEL[0].RazmerVvoda_L + "</br>";
            DSS.Modeling.TraceString += "Правая граница распределения размера данных для ввода (в ВУ1) "+UZEL[0].RazmerVvoda_R + "</br>";
            DSS.Modeling.TraceString += "Математическое ожидание объема вывода результатов обработки по заявкам (в ВУ1) "+UZEL[0].RazmerVyvoda_MO + "</br>";
            DSS.Modeling.TraceString += "СКО объема вывода результатов обработки по заявкам (в ВУ1) "+UZEL[0].RazmerVyvoda_SKO + "</br>";
            DSS.Modeling.TraceString += "Интенсивность входного потока заявок в ВУ2 "+UZEL[1].Vhod_Lamd + "</br>";
            DSS.Modeling.TraceString += "Левая граница распределения размера данных для ввода (в ВУ2) "+UZEL[1].RazmerVvoda_L + "</br>";
            DSS.Modeling.TraceString += "Правая граница распределения размера данных для ввода (в ВУ2) "+UZEL[1].RazmerVvoda_R + "</br>";
            DSS.Modeling.TraceString += "Математическое ожидание объема вывода результатов обработки по заявкам (в ВУ2) "+ UZEL[1].RazmerVyvoda_MO + "</br>";
            DSS.Modeling.TraceString += "СКО объема вывода результатов обработки по заявкам (в ВУ2) "+UZEL[1].RazmerVyvoda_SKO + "</br>";
            DSS.Modeling.TraceString += "Интенсивность входного потока заявок в ВУ3 "+UZEL[2].Vhod_Lamd + "</br>";
            DSS.Modeling.TraceString += "Левая граница распределения размера данных для ввода (в ВУ3) "+UZEL[2].RazmerVvoda_L + "</br>";
            DSS.Modeling.TraceString += "Правая граница распределения размера данных для ввода (в ВУ3) "+UZEL[2].RazmerVvoda_R + "</br>";
            DSS.Modeling.TraceString += "Математическое ожидание объема вывода результатов обработки по заявкам (в ВУ3) "+UZEL[2].RazmerVyvoda_MO + "</br>";
            DSS.Modeling.TraceString += "СКО объема вывода результатов обработки по заявкам (в ВУ3) "+UZEL[2].RazmerVyvoda_SKO  + "</br>";
            DSS.Modeling.TraceString += "Коэффициент пропорциональности времени ввода данных объему вводимых данных "+UZEL[0].UVD.KOEF + "</br>";
            DSS.Modeling.TraceString += "Коэффициент пропорциональности времени обработки суммарному объему ввода и вывода данных в ВУ "+UZEL[0].UR.KOEF + "</br>";
            DSS.Modeling.TraceString += "Коэффициент пропорциональности времени вывода данных "+UZEL[0].UV.KOEF + "</br>";
            DSS.Modeling.TraceString += "Коэффициент времени КПД "+KANAL[0].KOEF + "</br>";
            DSS.Modeling.TraceString += "БПЧ для потока ВУ1 "+UZEL[0].param1 + "</br>";
            DSS.Modeling.TraceString += "БПЧ для потока ВУ2 "+UZEL[1].param2 + "</br>";
            DSS.Modeling.TraceString += "БПЧ для потока ВУ3 "+UZEL[2].param3 + "</br>";
            DSS.Modeling.TraceString += "-----------------------------------------------------------------------------------" + "</br>";
        }

        int[] num = new int[3];

       public void TraceModel()
        {
           
            DSS.Modeling.TraceString += "ВУ1: УВД-[" + UZEL[0].UVD.Zanyatost.Value + ";" + UZEL[0].UVD.ZayNum + ";" + UZEL[0].UVD.Que.Count.Value + "],[" + UZEL[0].UVD.KOZ.Value + ";" + Math.Round(UZEL[0].UVD.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[0].UVD.Zanyto.TrueFrequency, 3) + "] УОД-[" + UZEL[0].UR.Zanyatost.Value + ";" + UZEL[0].UR.ZayNum + ";" + UZEL[0].UR.Que.Count.Value + "],[" + UZEL[0].UR.KOZ.Value + ";" + Math.Round(UZEL[0].UR.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[0].UR.Zanyto.TrueFrequency, 3) + "] УВР-[" + UZEL[0].UV.Zanyatost.Value + ";" + UZEL[0].UV.ZayNum + ";" + UZEL[0].UV.Que.Count.Value + "],[" + UZEL[0].UV.KOZ.Value + ";" + Math.Round(UZEL[0].UV.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[0].UV.Zanyto.TrueFrequency, 3) + "] " + "</br>";
            DSS.Modeling.TraceString += "ВУ2: УВД-[" + UZEL[1].UVD.Zanyatost.Value + ";" + UZEL[1].UVD.ZayNum + ";" + UZEL[1].UVD.Que.Count.Value + "],[" + UZEL[1].UVD.KOZ.Value + ";" + Math.Round(UZEL[1].UVD.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[1].UVD.Zanyto.TrueFrequency, 3) + "] УОД-[" + UZEL[1].UR.Zanyatost.Value + ";" + UZEL[1].UR.ZayNum + ";" + UZEL[1].UR.Que.Count.Value + "],[" + UZEL[1].UR.KOZ.Value + ";" + Math.Round(UZEL[1].UR.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[1].UR.Zanyto.TrueFrequency, 3) + "] УВР-[" + UZEL[1].UV.Zanyatost.Value + ";" + UZEL[1].UV.ZayNum + ";" + UZEL[1].UV.Que.Count.Value + "],[" + UZEL[1].UV.KOZ.Value + ";" + Math.Round(UZEL[1].UV.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[1].UV.Zanyto.TrueFrequency, 3) + "] " + "</br>";
            DSS.Modeling.TraceString += "ВУ3: УВД-[" + UZEL[2].UVD.Zanyatost.Value + ";" + UZEL[2].UVD.ZayNum + ";" + UZEL[2].UVD.Que.Count.Value + "],[" + UZEL[2].UVD.KOZ.Value + ";" + Math.Round(UZEL[2].UVD.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[2].UVD.Zanyto.TrueFrequency, 3) + "] УОД-[" + UZEL[2].UR.Zanyatost.Value + ";" + UZEL[2].UR.ZayNum + ";" + UZEL[2].UR.Que.Count.Value + "],[" + UZEL[2].UR.KOZ.Value + ";" + Math.Round(UZEL[2].UR.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[2].UR.Zanyto.TrueFrequency, 3) + "] УВР-[" + UZEL[2].UV.Zanyatost.Value + ";" + UZEL[2].UV.ZayNum + ";" + UZEL[2].UV.Que.Count.Value + "],[" + UZEL[2].UV.KOZ.Value + ";" + Math.Round(UZEL[2].UV.Variance_QueCount.Mx, 3) + ";" + Math.Round(UZEL[2].UV.Zanyto.TrueFrequency, 3) + "]"  + "</br>";
            
            for (int i = 0; i <= 2; i++)
            {
                if (KANAL[i].Zanyatost.Ref == null) num[i] = 0;
                else num[i] = KANAL[i].Zanyatost.Ref.Num;
            }

            DSS.Modeling.TraceString += "КПД(1,2) :[" + KANAL[0].Zanyatost.Value + ";" + num[0] + ";" + KANAL[0].Q_Vhod.Count.Value + ";" + KANAL[0].Q_Vozvrat.Count.Value + "],[" + KANAL[0].KOZ.Value + ";" + Math.Round(KANAL[0].Variance_Q_Vhod.Mx, 3) + ";" + Math.Round(KANAL[0].Variance_Q_Vozvrat.Mx, 3) + ";" + Math.Round(KANAL[0].Zanyato.TrueFrequency, 3) + "] КПД(1,3):[" + KANAL[1].Zanyatost.Value + ";" + num[1] + ";" + KANAL[1].Q_Vhod.Count.Value + ";" + KANAL[1].Q_Vozvrat.Count.Value + "],[" + KANAL[1].KOZ.Value + ";" + Math.Round(KANAL[1].Variance_Q_Vhod.Mx, 3) + ";" + Math.Round(KANAL[1].Variance_Q_Vozvrat.Mx, 3) + ";" + Math.Round(KANAL[1].Zanyato.TrueFrequency, 3) + "] КПД(2,3):[" + KANAL[2].Zanyatost.Value + ";" + num[2] + ";" + KANAL[2].Q_Vhod.Count.Value + ";" + KANAL[2].Q_Vozvrat.Count.Value + "],[" + KANAL[2].KOZ.Value + ";" + Math.Round(KANAL[2].Variance_Q_Vhod.Mx, 3) + ";" + Math.Round(KANAL[2].Variance_Q_Vozvrat.Mx, 3) + ";" + Math.Round(KANAL[2].Zanyato.TrueFrequency, 3) + "]" + "</br>";
      
       }

        
      public void VariantUslov(int NumVar, Zayavka Z, ref bool flPered)
       {
           if (NumVar == 2)
           {
               foreach (var uzel in UZEL)
               {
                   //Если УОД в родительском узле занято, а в рассматриваемом - свободно
                   if ((Z.UzelVhoda.UR.Zanyatost.Value) && (!uzel.UR.Zanyatost.Value))
                   {
                       //Найдем канал, который связывает текущий узел и найденый
                       foreach (var Kanal in KANAL)
                       {
                           if (((Kanal.VU_1 == Z.UzelVhoda) || (Kanal.VU_2 == Z.UzelVhoda)) && ((Kanal.VU_1 == uzel) || (Kanal.VU_2 == uzel)))
                           {
                               //Если канал свободен
                               if (!Kanal.Zanyatost.Value)
                               {
                                   flPered = true; //Произошла передача заявки через канал
                                   //Передаем заявку через канал
                                   var evPrd = new KPD.Event_Vhod_KPD();
                                   evPrd.UzelPered = uzel.Name; //Передаём имя узла, в который передаётся заявка
                                   //Увеличиваем количество переданных заявок
                                   if (uzel == UZEL[0]) Z.UzelVhoda.KPZ[0]++;
                                   if (uzel == UZEL[1]) Z.UzelVhoda.KPZ[1]++;
                                   if (uzel == UZEL[2]) Z.UzelVhoda.KPZ[2]++;
                                   evPrd.Z = Z; //Передаем заявку в событие
                                   Kanal.RunEventHandlerNow(evPrd);
                               }
                               break; //После обнаружения нужного канала, остальные не проверяем 
                           }
                       }
                       if (flPered == true) break;
                   }
               }
 
           }
           if (NumVar == 1)
           {
               flPered = false;
           }
 
       }

    }
}