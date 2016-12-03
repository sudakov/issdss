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
    
    public partial class KPD : Model
    {
        #region Параметры модели КПД
        public double KOEF; //Коэффициент пропорциональности времени передачи обрабатываемых данных заявки
        public VU VU_1; //ВУ, с которым связан канал
        public VU VU_2; //ВУ, с которым связан канал
        public int trace;
        #endregion

        #region Состояние КПД
        public TRefVar<VS.Zayavka> Zanyatost;
        public TIntVar KOZ; //Количество обработанных заявок
        public TIntVar KPZ; //Количество потерянных заявок

        
        public class QRec : QueueRecord //очередь заявок в устройстве ввода данных
        {
            public VS.Zayavka Z; //Заявка
            public string UzelPered;//Имя узла в который или из которого осуществляется передача
        }

        public SimpleModelList<QRec> Q_Vhod; //очередь заявок для передачи на обработку
        public SimpleModelList<QRec> Q_Vozvrat; //очередь заявок на возврат

        #endregion

        #region  Сборщики статистики
        public BoolCollector Zanyato; //Вероятность занятости канала
        public Variance<int> Variance_Q_Vhod; //МО и дисперсия очереди входящих заявок
        public Variance<int> Variance_Q_Vozvrat; //МО и дисперсия очереди возвращающихся заявок
        public Min<int> Min_Q_Vhod;            //минимум длины очереди
        public Max<int> Max_Q_Vhod;            //максимум длины очереди
        public Min<int> Min_Q_Vozvrat;            //минимум длины очереди
        public Max<int> Max_Q_Vozvrat;            //максимум длины очереди
        #endregion


        public KPD(VS parent, string name)
            : base(parent, name)
        {

            #region Инициализация переменных объектов модели
            KOZ = InitModelObject<TIntVar>();
            KPZ = InitModelObject<TIntVar>();
            Zanyatost = InitModelObject<TRefVar<VS.Zayavka>>();

            Q_Vhod = InitModelObject<SimpleModelList<QRec>>();
            Q_Vozvrat = InitModelObject<SimpleModelList<QRec>>();
            #endregion

            #region Инициализация сборщиков статистики
            Variance_Q_Vhod = InitModelObject<Variance<int>>();
            Variance_Q_Vhod.ConnectOnSet(Q_Vhod.Count);
            Min_Q_Vhod = InitModelObject<Min<int>>();
            Min_Q_Vhod.ConnectOnSet(Q_Vhod.Count);
            Max_Q_Vhod = InitModelObject<Max<int>>();
            Max_Q_Vhod.ConnectOnSet(Q_Vhod.Count);

            Variance_Q_Vozvrat = InitModelObject<Variance<int>>();
            Variance_Q_Vozvrat.ConnectOnSet(Q_Vozvrat.Count);
            Min_Q_Vozvrat = InitModelObject<Min<int>>();
            Min_Q_Vozvrat.ConnectOnSet(Q_Vozvrat.Count);
            Max_Q_Vozvrat = InitModelObject<Max<int>>();
            Max_Q_Vozvrat.ConnectOnSet(Q_Vozvrat.Count);

            Zanyato = InitModelObject<BoolCollector>();
            Zanyato.ConnectOnSet(Zanyatost);
            #endregion
        }
    }
}