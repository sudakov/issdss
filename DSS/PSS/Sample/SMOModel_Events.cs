using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonModel.Kernel;

namespace DSS.PSS.Sample
{
    public partial class SMOModel : Model
    {
        /// <summary>
        /// Событие 1: вход заявки первого потока в СМО
        /// </summary>
        public class Event_K1 : TimeModelEvent<SMOModel>
        {
            public int Nz;
            public byte Np;
            public double timeIn;

            protected override void HandleEvent(ModelEventArgs args)
            {
                //Model.Tracer.EventTrace(this, "Flow №" + Np);

                Model.KVZS.Value += 1;
                Model.KVZ[0].Value += 1;

                if (Model.KZ[0].Value == true)
                {
                    if (Model.queue[0].Count.Value < Model.MaxLength[0])
                    {
                        QueueRec record = new QueueRec();
                        record.NZ = Model.KVZ[0].Value;
                        record.timeIn = Time;
                        record.NP = 1;
                        Model.queue[0].Add(record);
                    }
                    else
                    {
                        Model.KPZ[0].Value += 1;
                    }
                }
                else
                {
                    Model.KZ[0].Value = true;
                    Event_K3 event_k3 = new Event_K3();
                    event_k3.Nz = Model.KVZ[0].Value;
                    event_k3.timeIn = Model.Time;
                    event_k3.Np = 1;
                    double lambda = Model.servFlowGenerator[0].GenerateValue();
                    Model.PlanEvent(event_k3, lambda);
                    //Model.Tracer.PlanEventTrace(event_k3, event_k3.Nz, event_k3.timeIn, "Flow №" + event_k3.Np);
                    Model.TZKO[0].Value += lambda;
                }
                Event_K1 event_k1 = new Event_K1();
                double lambda1 = Model.inFlowGenerator[0].GenerateValue();
                event_k1.Np = 1;
                Model.PlanEvent(event_k1, lambda1);
                //Model.Tracer.PlanEventTrace(event_k1, "Flow №" + event_k1.Np);
                Model.ModelStatus();
            }
        }

        /// <summary>
        /// Событие 2: вход заявки второго потока в СМО
        /// </summary>
        public class Event_K2 : TimeModelEvent<SMOModel>
        {
            public int Nz;
            public double timeIn;
            public byte Np;

            protected override void HandleEvent(ModelEventArgs args)
            {
                //Model.Tracer.EventTrace(this, "Flow №" + this.Np);
                Model.KVZS.Value += 1;
                Model.KVZ[2].Value += 1;

                if (Model.KZ[1].Value == false)
                {
                    Model.KZ[1].Value = true;
                    Event_K4 event_k4 = new Event_K4();
                    event_k4.Nz = Model.KVZS.Value;
                    event_k4.timeIn = Model.Time;
                    event_k4.Np = 2;
                    double mu = Model.servFlowGenerator[1].GenerateValue();
                    Model.PlanEvent(event_k4, mu);
                    //Model.Tracer.PlanEventTrace(event_k4, event_k4.Nz, event_k4.timeIn, "Flow №" + event_k4.Np);

                    Model.TZKO[1].Value += mu;
                }
                else
                {
                    if (Model.queue[2].Count.Value < Model.MaxLength[2])
                    {
                        QueueRec record = new QueueRec();
                        record.NZ = Model.KVZ[1].Value;
                        record.timeIn = Time;
                        record.NP = 2;
                        Model.queue[2].Add(record);
                    }
                    else
                    {
                        Model.KPZ[2].Value += 1;
                    }
                }

                Event_K2 event_k2 = new Event_K2();
                event_k2.Np = 2;
                double lambda = Model.inFlowGenerator[1].GenerateValue();
                Model.PlanEvent(event_k2, lambda);
                //Model.Tracer.PlanEventTrace(event_k2, "Flow №" + event_k2.Np);

                Model.ModelStatus();
            }
        }

        /// <summary>
        /// Событие 3: окончание обслуживания в КО1
        /// </summary>
        public class Event_K3 : TimeModelEvent<SMOModel>
        {
            public int Nz;
            public double timeIn;
            public byte Np;

            protected override void HandleEvent(ModelEventArgs args)
            {
                //Model.Tracer.EventTrace(this, this.Nz, this.timeIn, "Flow №" + this.Np);
                Model.KVZ[1].Value += 1;

                if (Model.KZ[1].Value == true)
                {
                    if (Model.queue[1].Count.Value < Model.MaxLength[1])
                    {
                        QueueRec record = new QueueRec();
                        record.NZ = Model.KVZ[0].Value;
                        record.timeIn = Time;
                        record.NP = 1;
                        Model.queue[1].Add(record);
                    }
                    else
                    {
                        Model.KPZ[1].Value += 1;
                    }
                }
                else
                {
                    Model.KZ[1].Value = true;
                    Event_K4 event_k4 = new Event_K4();
                    event_k4.Nz = Model.KVZ[1].Value;
                    event_k4.timeIn = Model.Time;
                    event_k4.Np = 1;
                    double mu = Model.servFlowGenerator[1].GenerateValue();
                    Model.PlanEvent(event_k4, mu);
                    //Model.Tracer.PlanEventTrace(event_k4, event_k4.Nz, event_k4.timeIn, "Flow №" + event_k4.Np);
                    Model.TZKO[1].Value += mu;
                }

                if (Model.queue[0].Count.Value != 0)
                {
                    QueueRec record = Model.queue[0].Pop();
                    Event_K3 event_k3 = new Event_K3();
                    event_k3.Nz = record.NZ;
                    event_k3.timeIn = record.timeIn;
                    event_k3.Np = 1;
                    double mu = Model.servFlowGenerator[0].GenerateValue();
                    Model.PlanEvent(event_k3, mu);
                    //Model.Tracer.PlanEventTrace(event_k3, event_k3.Nz, event_k3.timeIn, "Flow №" + event_k3.Np);
                    Model.TZKO[0].Value += mu;
                }
                else
                {
                    Model.KZ[0].Value = false;
                }
                Model.ModelStatus();
            }
        }

        /// <summary>
        /// Событие 4: окончание обслуживания в КО2
        /// </summary>
        public class Event_K4 : TimeModelEvent<SMOModel>
        {
            public int Nz;
            public double timeIn;
            public byte Np;

            protected override void HandleEvent(ModelEventArgs args)
            {

                //Model.Tracer.EventTrace(this, this.Nz, this.timeIn, "Flow №" + this.Np);

                if (Np == 2)
                {
                    double x = Model.repeateGenerator.GenerateValue();
                    if (x < Model.restartP)
                    {
                        //Model.Tracer.AnyTrace("RR");
                        if (Model.queue[2].Count.Value == 0)
                        {
                            Model.KZ[1].Value = true;
                            Event_K4 event_k4 = new Event_K4();
                            event_k4.Nz = Model.KVZS.Value;
                            event_k4.timeIn = Model.Time;
                            event_k4.Np = 2;
                            double mu = Model.servFlowGenerator[1].GenerateValue();
                            Model.PlanEvent(event_k4, mu);
                            //Model.Tracer.PlanEventTrace(event_k4, event_k4.Nz, event_k4.timeIn, "Flow №" + event_k4.Np);
                            Model.TZKO[1].Value += mu;
                        }
                        else
                        {
                            QueueRec record = new QueueRec();
                            record.NZ = Model.KVZS.Value;
                            record.timeIn = Model.Time;
                            record.NP = 2;
                            Model.queue[2].Add(record);
                            QueueRec record1 = Model.queue[2].Pop();
                            Event_K4 event_k4 = new Event_K4();
                            event_k4.Nz = record1.NZ;
                            event_k4.timeIn = record1.timeIn;
                            event_k4.Np = 2;
                            double mu = Model.servFlowGenerator[1].GenerateValue();
                            Model.PlanEvent(event_k4, mu);
                            //Model.Tracer.PlanEventTrace(event_k4, event_k4.Nz, event_k4.timeIn, "Flow №" + event_k4.Np);
                            Model.TZKO[1].Value += mu;
                        }
                    }
                    else
                    {
                        Model.TimeIn_SecondFlow.Value = Time - timeIn;
                        if (Model.queue[2].Count.Value != 0)
                        {
                            QueueRec record = Model.queue[2].Pop();
                            Event_K4 event_k4 = new Event_K4();
                            event_k4.Nz = record.NZ;
                            event_k4.timeIn = record.timeIn;
                            event_k4.Np = record.NP;
                            double mu = Model.servFlowGenerator[1].GenerateValue();
                            Model.PlanEvent(event_k4, mu);
                            //Model.Tracer.PlanEventTrace(event_k4, event_k4.Nz, event_k4.timeIn, "Flow №", event_k4.Np);
                            Model.TZKO[1].Value += mu;
                        }
                        else
                        {
                            if (Model.queue[1].Count.Value != 0)
                            {
                                QueueRec record = Model.queue[1].Pop();
                                Event_K4 event_k4 = new Event_K4();
                                event_k4.Nz = record.NZ;
                                event_k4.timeIn = record.timeIn;
                                event_k4.Np = record.NP;
                                double mu = Model.servFlowGenerator[1].GenerateValue();
                                Model.PlanEvent(event_k4, mu);
                                //Model.Tracer.PlanEventTrace(event_k4, event_k4.Nz, event_k4.timeIn, "Flow №", event_k4.Np);
                                Model.TZKO[1].Value += mu;
                            }
                            else
                            {
                                Model.KZ[1].Value = false;
                            }
                        }
                    }
                }
                if (Np == 1)
                {
                    Model.TimeIn_FirstFlow.Value = Time - timeIn;
                    if (Model.queue[2].Count.Value != 0)
                    {
                        QueueRec record = Model.queue[2].Pop();
                        Event_K4 event_k4 = new Event_K4();
                        event_k4.Nz = record.NZ;
                        event_k4.timeIn = record.timeIn;
                        event_k4.Np = record.NP;
                        double mu = Model.servFlowGenerator[1].GenerateValue();
                        Model.PlanEvent(event_k4, mu);
                        //Model.Tracer.PlanEventTrace(event_k4, event_k4.Nz, event_k4.timeIn, "Flow №", event_k4.Np);
                        Model.TZKO[1].Value += mu;
                    }
                    else
                    {
                        if (Model.queue[1].Count.Value != 0)
                        {
                            QueueRec record = Model.queue[1].Pop();
                            Event_K4 event_k4 = new Event_K4();
                            event_k4.Nz = record.NZ;
                            event_k4.timeIn = record.timeIn;
                            event_k4.Np = record.NP;
                            double mu = Model.servFlowGenerator[1].GenerateValue();
                            Model.PlanEvent(event_k4, mu);
                            //Model.Tracer.PlanEventTrace(event_k4, event_k4.Nz, event_k4.timeIn, "Flow №" + event_k4.Np);
                            Model.TZKO[1].Value += mu;
                        }
                        else
                        {
                            Model.KZ[1].Value = false;
                        }
                    }
                }
                Model.ModelStatus();
            }
        }
    }
}