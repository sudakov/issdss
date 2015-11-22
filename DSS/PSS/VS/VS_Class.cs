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
    public partial class VS : Model
    {
        #region  Параметры ИМ
        double MaxT;          //время прогона модели  
        public int NumVar; // Номер варианта, условий передачи заявки по КПД
        public int trace;
        #endregion

        #region  Состояния ИМ
        TIntVar KVZ;        //Количество заявок, вошедших в ВС 
        TIntVar KOZ;        //Количество заявок, обработанных в ВС
        TRealVar TOZ;        //Время обработки заявки в системе
        #endregion

        #region Сборщики статистики
        Variance<Double> Variance_TOZ;  //МО и дисперсия
        Min<double> Min_TOZ;            //минимум
        Max<double> Max_TOZ;            //максимум
        DynamicHistogram His_TOZ;       //гистрограмма

        //для тестирования визуализации сборщика AllValues и DynamicHistogram
        public TIntVar testAV;
        public TRealVar testDH;
        public AllValues<int> allValuesTest;
        public DynamicHistogram dhTest;
        //-------------------------------------------------
        #endregion

        #region Структура заявки
        public class Zayavka
        {
            public int Num;            //Порядковый номер заявки
            public VU UzelVhoda;     //Узел входа заявки
            public double TimeVhod;    //Время входа заявки в систему
            public double RazmerVvod;  //Объем ввода данных
            public double RazmerVyvod; //Объем вывода данных
        }
        #endregion

        #region Подмодели
        public VU[] UZEL; //Модели вычислительных узлов
        public KPD[] KANAL; //Модели каналов передачи данных
        #endregion

        public VS(Model parent, string name)
            : base(parent, name)
        {
            #region  Инициализация переменных объектов модели
            KVZ = InitModelObject<TIntVar>();
            KOZ = InitModelObject<TIntVar>();
            TOZ = InitModelObject<TRealVar>();

            UZEL = new VU[3];
            UZEL[0] = new VU(this, "ВУ1");
            UZEL[1] = new VU(this, "ВУ2");
            UZEL[2] = new VU(this, "ВУ3");

            this.AddModelObject(UZEL[0]);
            this.AddModelObject(UZEL[1]);
            this.AddModelObject(UZEL[2]);

            KANAL = new KPD[3];
            KANAL[0] = new KPD(this, "КПД(1,2)");
            KANAL[1] = new KPD(this, "КПД(1,3)");
            KANAL[2] = new KPD(this, "КПД(2,3)");

            this.AddModelObject(KANAL[0]);
            this.AddModelObject(KANAL[1]);
            this.AddModelObject(KANAL[2]);

            KANAL[0].VU_1 = UZEL[0];
            KANAL[0].VU_2 = UZEL[1];
            KANAL[1].VU_1 = UZEL[0];
            KANAL[1].VU_2 = UZEL[2];
            KANAL[2].VU_1 = UZEL[1];
            KANAL[2].VU_2 = UZEL[2];
            #endregion

            #region Инициализация сборщиков статистики

            Variance_TOZ = InitModelObject<Variance<Double>>();   //создаем сборщик
            Variance_TOZ.ConnectOnSet(TOZ);              //подключаем сборщик к переменной

            Min_TOZ = InitModelObject<Min<double>>();   //создаем сборщик
            Min_TOZ.ConnectOnSet(TOZ);                  //подключаем сборщик к переменной

            Max_TOZ = InitModelObject<Max<double>>();   //создаем сборщик
            Max_TOZ.ConnectOnSet(TOZ);              //подключаем сборщик к переменной

            His_TOZ = InitModelObject<DynamicHistogram>();
            His_TOZ.ConnectOnSet(TOZ);


            //--------------------------------------------------
            allValuesTest = InitModelObject<AllValues<int>>("Сборщик полной статистики");
            dhTest = InitModelObject<DynamicHistogram>("Динамическая гистограмма");
            testAV = InitModelObject<TIntVar>("Тестовая переменная 1");
            testDH = InitModelObject<TRealVar>("Тестовая переменная 2");
            testAV.AddCollectors_OnSet(allValuesTest);
            testDH.AddCollectors_OnSet(dhTest);
            //--------------------------------------------------
            #endregion


            /*Tracer.AddAutoTabModel(this);
            UZEL.ToList().ForEach(u => Tracer.AddAutoTabModel(u));
            KANAL.ToList().ForEach(k => Tracer.AddAutoTabModel(k));*/

        }
    }
}