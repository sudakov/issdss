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
        #region Описание событий модели КПД
        //Событие входа заявки в канал
        public class Event_Vhod_KPD : TimeModelEvent<KPD>
        {
            #region Атрибуты события
            public VS.Zayavka Z; // Заявка поступившая в канал

            public string UzelPered; //Узел в который или из которого передаётся заявка
            #endregion

            #region Алгоритм события
            protected override void HandleEvent(ModelEventArgs args)
            {
                Model.Tracer.EventTraceLevel(Model.trace, this, "Заявка:" + Z.Num, Model.Name, Z.UzelVhoda.Name, UzelPered); // Сообщение в трассировку о случившемся событии

                //Если канал передачи свободен
                if (Model.Zanyatost.Value == false)
                {
                    //Планируем окончание передачи данных по каналу
                    var ev = new Event_Final_KPD(); //Создаем объект события
                    ev.Z = Z; //Передаем заявку в событие

                    ev.UzelPered = UzelPered; //Передача имени узла в который или из которого передаётся заявка
                    double dt; //Время через которое произойдет событие
                    if (Z.UzelVhoda.Name != UzelPered) dt = Model.KOEF * Z.RazmerVvod; // Если заявка передается на обработку, то время передачи пропорционально объему работы
                    else dt = Model.KOEF * Z.RazmerVyvod; // Если на выход, то пропорционально объёму данных для вывода
                    Model.PlanEvent(ev, dt); //Планируем событие
                    Model.Tracer.PlanEventTraceLevel(Model.trace, ev, "Заявка:" + ev.Z.Num, Model.Name, ev.Z.UzelVhoda.Name, ev.UzelPered); // Выводим трассировку о запланированном событии
                    Model.Zanyatost.Ref = Z; //Занимаем канал заявкой
                }
                else //Если канал занят
                {
                    if (Z.UzelVhoda.Name != UzelPered)//Если зявка поступила на вход
                    {
                        var zayrec = new QRec();//Создаем элемент очереди
                        zayrec.Z = Z;//Передаем заявку в элемент
                        zayrec.UzelPered = UzelPered; //Передача имени узла в который или из которого передаётся заявка
                        Model.Q_Vhod.Add(zayrec);//Добавляем заявку в очередь
                    }
                    else //Если заявка поступила на вывод
                    {
                        var zayrec = new QRec();//Создаем элемент очереди
                        zayrec.Z = Z;//Передаем заявку в элемент
                        zayrec.UzelPered = UzelPered; //Передача имени узла в который или из которого передаётся заявка
                        Model.Q_Vozvrat.Add(zayrec); //Добавляем заявку в очередь
                    }
                }
            }
            #endregion
        }
        //Завершение передачи заявки
        public class Event_Final_KPD : TimeModelEvent<KPD>
        {
            #region Атрибуты события
            public VS.Zayavka Z; // Заявка поступившая в канал

            public string UzelPered; //Узел в который или из которого передаётся заявка
            #endregion

            #region Алгоритм события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + " " + Z.UzelVhoda.Name + " " + UzelPered + "</br>"; // Сообщение в трассировку о случившемся событии
                Model.KOZ.Value++; //увеличиваем количество обработанных заявок
                Model.Zanyatost.Ref = null; //Освобождаем канал
                if (Z.UzelVhoda.Name != UzelPered) //Если зявка поступила на вход
                {
                    var ev = new VU.Event_StartRabota_VU();
                    ev.Z = Z;
                    //Проверяем из какого узла пришла заявка и отправляем её в другой узел для обработки
                    if (Z.UzelVhoda == Model.VU_1)
                        Model.VU_2.RunEventHandlerNow(ev);
                    else
                        Model.VU_1.RunEventHandlerNow(ev);
                }
                else //Если заявка поступила на выход
                {
                    var ev = new VU.Event_Vyvod_VU();
                    ev.Z = Z;
                    //Проверяем из какого канала пришла заявка и отправляем её туда для вывода
                    if (Z.UzelVhoda == Model.VU_1)
                        Model.VU_1.RunEventHandlerNow(ev);
                    else
                        Model.VU_2.RunEventHandlerNow(ev);
                }

                if (Model.Q_Vozvrat.Count != 0) //Если очередь для возврата заявок не пуста
                {
                    //Берем заявку из очереди
                    var rec = new QRec();
                    rec = Model.Q_Vozvrat.Pop();
                    //Планируем окончание передачи данных по каналу
                    var evf = new Event_Final_KPD();
                    evf.Z = rec.Z;//Передаем заявку в событие

                    evf.UzelPered = rec.UzelPered; //Передача имени узла в который или из которого передаётся заявка
                    double dt = Model.KOEF * Z.RazmerVyvod; //Время передачи по каналу пропорционально объёму выводимой информации
                    Model.PlanEvent(evf, dt);//Планируем событие
                    DSS.Modeling.TraceString += "Заявка:" + rec.Z.Num + " " + Model.Name + " " + evf.Z.UzelVhoda.Name + " " + evf.UzelPered + "</br>"; //Выводим в трассировку информацию о запланированном событии
                    Model.Zanyatost.Ref = evf.Z;//Занимаем канал заявкой
                }
                else//Если очередь для возврата заявок пуста
                {
                    if (Model.Q_Vhod.Count != 0) //Если очередь для входа заявок не пуста
                    {
                        //Берем заявку из очереди
                        var rec = new QRec();
                        rec = Model.Q_Vhod.Pop();
                        //Планируем окончание передачи данных по каналу
                        var evf = new Event_Final_KPD();
                        evf.Z = rec.Z;//Передаем заявку в событие
                        evf.UzelPered = rec.UzelPered; //Передача имени узла в который или из которого передаётся заявка

                        double dt = Model.KOEF * Z.RazmerVvod;//Время передачи по каналу пропорционально объёму работы
                        Model.PlanEvent(evf, dt);//Планируем событие
                        DSS.Modeling.TraceString += "Заявка:" + rec.Z.Num + " " + Model.Name + " " + evf.Z.UzelVhoda.Name + " " + evf.UzelPered + "</br>";//Выводим в трассировку информацию о запланированном событии
                        Model.Zanyatost.Ref = evf.Z;//Занимаем канал заявкой
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}