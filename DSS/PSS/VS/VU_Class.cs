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
        #region  Параметры ИМ
        public double Vhod_Lamd;         //Интенсивность входного потока заявок в i-ом ВУ
        public double RazmerVvoda_L;     //Левая граница распределения размера данных для ввода
        public double RazmerVvoda_R;     //Правая граница распределения размера данных для ввода
        public double RazmerVyvoda_MO;   //Математическое ожидание объема вывода результатов обработки по заявкам
        public double RazmerVyvoda_SKO;  //СКО объема вывода результатов обработки по заявкам
        public VS ParentVS; //Родительский класс
        public int param1, param2, param3; //бпч
        public int trace;
        #endregion

        #region Моделирование случайных явлений и организация зависимых испытаний
        public ExpStream Gener_Vhod;             //Интервалы времени между заявками во входных потоках
        public UniformStream Gener_RazmerVvod;       //Объемы данных ввода по поступившим заявкам
        public NormalStream Gener_RazmerVyvoda;     //Объемы вывода результатов обработки по поступившим заявкам    
        //public ExpStream Gener_RazmerRabota;     //Вычислительная сложность задачи
        #endregion

        #region  Состояния ИМ
        public TIntVar KVZ;        //Количество заявок, вошедших в ВС 
        public TIntVar KOZ;        //Количество заявок, обработанных в ВС
        TRealVar TOZ;        //Время обработки заявки в системе
        public int[] KPZ; //Количество переданных заявок
        #endregion

        #region Подмодели
        public UstroystvoVvoda UVD; //Устройство ввода данных
        public UstroystvoRabota UR; //Устройвство обработки данных
        public UstroystvoVyvoda UV; //Устройство ввода данных
        #endregion

        #region Сборщики статистики
        public Variance<Double> Variance_TOZ;  //МО и дисперсия
        public Min<double> Min_TOZ;            //минимум
        public Max<double> Max_TOZ;            //максимум
        public DynamicHistogram His_TOZ;       //гистрограмма
        #endregion

        public VU(VS parent, string name)
            : base(parent, name)
        {
            ParentVS = parent;
            UVD = new UstroystvoVvoda(this, "УВД" + "(" + name + ")");
            UR = new UstroystvoRabota(this, "УОД" + "(" + name + ")");
            UV = new UstroystvoVyvoda(this, "УВР" + "(" + name + ")");

            this.AddModelObject(UVD);
            this.AddModelObject(UR);
            this.AddModelObject(UV);

            #region  Инициализация переменных объектов модели
            KVZ = InitModelObject<TIntVar>();
            KOZ = InitModelObject<TIntVar>();
            TOZ = InitModelObject<TRealVar>();
            KPZ = new int[3];

            Gener_Vhod = InitModelObject<ExpStream>();
            Gener_RazmerVvod = InitModelObject<UniformStream>();
            Gener_RazmerVyvoda = InitModelObject<NormalStream>();
            //  Gener_RazmerRabota = InitModelObject<ExpStream>();
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
            #endregion
        }

    }
}