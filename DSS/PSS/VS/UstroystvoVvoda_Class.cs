using System.Text;
using CommonModel.StatisticsCollecting;
using CommonModel.RandomStreamProducing;
using CommonModel.Collections;
using CommonModel.Kernel;
using System.Runtime.Serialization;

namespace DSS.PSS.VS
{
    public partial class UstroystvoVvoda : Model
    {
        #region Параметры модели УВД
        public double KOEF; //Коэффициент пропорциональности времени ввода объему вводимых данных заявки
        public VU parentVU; //Родительский класс
        public int trace;
        #endregion

        #region Состояние УВД
        public TRefVar<VS.Zayavka> Zanyatost; //переменная, характеризующая занятость устройства
        public int ZayNum;//Номер обрабатываемой заявки
        public TIntVar KOZ; //Количество обработанных заявок
        TIntVar KVZ; //Количество вошедших заявок

        
        public class QRec : QueueRecord //очередь заявок в устройстве ввода данных
        {
            public VS.Zayavka Z; //Заявка
        }

        public SimpleModelList<QRec> Que;  //очередь

        #endregion

        #region  Сборщики статистики
        public Variance<int> Variance_QueCount;    //МО и дисперсия длины очереди
        public Min<int> Min_QueCount;            //минимум длины очереди
        public Max<int> Max_QueCount;            //максимум длины очереди
        public BoolCollector Zanyto; //Вероятность занятости устройства
        #endregion


        public UstroystvoVvoda(VU parent, string name)
            : base(parent, name)
        {

            #region Инициализация переменных объектов модели
            KOZ = InitModelObject<TIntVar>();
            KVZ = InitModelObject<TIntVar>();
            Zanyatost = InitModelObject<TRefVar<VS.Zayavka>>();


            Que = InitModelObject<SimpleModelList<QRec>>();

            parentVU = parent;
            #endregion

            #region Инициализация сборщиков статистики
            Variance_QueCount = InitModelObject<Variance<int>>();
            Variance_QueCount.ConnectOnSet(Que.Count);

            Min_QueCount = InitModelObject<Min<int>>();
            Min_QueCount.ConnectOnSet(Que.Count);
            Max_QueCount = InitModelObject<Max<int>>();
            Max_QueCount.ConnectOnSet(Que.Count);

            Zanyto = InitModelObject<BoolCollector>();
            Zanyto.ConnectOnSet(Zanyatost);

            #endregion

        }

    }
}