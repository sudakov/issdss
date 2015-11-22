using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonModel.StatisticsCollecting;
using CommonModel.RandomStreamProducing;
using CommonModel.Collections;
using CommonModel.Kernel;

namespace DSS.PSS.Sample
{
    public partial class SMOModel : Model
    {
        double[] Lambda = new double[2];        //интенсивности входных потоков
        double[] Mu = new double[2];            //интенсивности обслуживания
        int[] MaxLength = new int[3];           //ограничения на длины очередей
        double restartP;                        //вероятость необходимости дообслуживания заявки
        double TP;                              //время прогона модели

        TIntVar KVZS;                           //суммарное количество заявок, вошедших в СМО
        TIntVar[] KVZ;                          //количество заявок, пришедших на вход каждой из очередей
        TIntVar[] KPZ;                          //количество потерянных заявок на входе каждой из очередей
        TBoolVar[] KZ;                          //флаг занятости каждого КО
        TRealVar[] TZKO;                        //суммарное время занятости каждого КО

        class QueueRec : QueueRecord
        {
            public int NZ;                      //номер заявки
            public double timeIn;               //время входа в систему
            public byte NP;                     //номер потока
        }

        SimpleModelList<QueueRec>[] queue;      //группа очередей

        PoissonStream[] inFlowGenerator;        //генератор для входного потока
        ExpStream[] servFlowGenerator;          //генератор для КО
        UniformStream repeateGenerator;         //генератор для моделирования дообслуживания

        TRealVar TimeIn_FirstFlow;              //время заявки 1 потока в СМО
        TRealVar TimeIn_SecondFlow;             //время заявки 2 потока в СМО

        Variance<int>[] Variance_QueueCount;
        BoolCollector[] Bool_Kanal;

        Variance<double> Variance_TimeIn_FirstFlow;
        Min<double> Min_TimeIn_FirstFlow;
        Max<double> Max_TimeIn_FirstFlow;

        Variance<double> Variance_TimeIn_SecondFlow;
        Min<double> Min_TimeIn_SecondFlow;
        Max<double> Max_TimeIn_SecondFlow;

        public SMOModel(Model parent, string name)
            : base(parent, name)
        {
            KVZS = InitModelObject<TIntVar>();
            KVZ = InitModelObjectArray<TIntVar>(3, "");
            KPZ = InitModelObjectArray<TIntVar>(3, "");
            KZ = InitModelObjectArray<TBoolVar>(2, "");
            TZKO = InitModelObjectArray<TRealVar>(2, "");
            KPZ = InitModelObjectArray<TIntVar>(3, "");

            queue = InitModelObjectArray<SimpleModelList<QueueRec>>(3, "");

            TimeIn_FirstFlow = InitModelObject<TRealVar>();
            TimeIn_SecondFlow = InitModelObject<TRealVar>();

            inFlowGenerator = InitModelObjectArray<PoissonStream>(2, "");
            servFlowGenerator = InitModelObjectArray<ExpStream>(2, "");
            repeateGenerator = InitModelObject<UniformStream>();

            Variance_QueueCount = InitModelObjectArray<Variance<int>>(3, "");
            Variance_QueueCount[0].ConnectOnSet(queue[0].Count);
            Variance_QueueCount[1].ConnectOnSet(queue[1].Count);
            Variance_QueueCount[2].ConnectOnSet(queue[2].Count);

            Variance_TimeIn_FirstFlow = InitModelObject<Variance<double>>();
            Variance_TimeIn_FirstFlow.ConnectOnSet(TimeIn_FirstFlow);

            Variance_TimeIn_SecondFlow = InitModelObject<Variance<double>>();
            Variance_TimeIn_SecondFlow.ConnectOnSet(TimeIn_SecondFlow);

            Min_TimeIn_FirstFlow = InitModelObject<Min<double>>();
            Min_TimeIn_FirstFlow.ConnectOnSet(TimeIn_FirstFlow);

            Min_TimeIn_SecondFlow = InitModelObject<Min<double>>();
            Min_TimeIn_SecondFlow.ConnectOnSet(TimeIn_SecondFlow);

            Max_TimeIn_FirstFlow = InitModelObject<Max<double>>();
            Max_TimeIn_FirstFlow.ConnectOnSet(TimeIn_FirstFlow);

            Max_TimeIn_SecondFlow = InitModelObject<Max<double>>();
            Max_TimeIn_SecondFlow.ConnectOnSet(TimeIn_SecondFlow);

            Bool_Kanal = InitModelObjectArray<BoolCollector>(2, "");
            Bool_Kanal[0].ConnectOnSet(KZ[0]);
            Bool_Kanal[1].ConnectOnSet(KZ[1]);
        }
    }
}