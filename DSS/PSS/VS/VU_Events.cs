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
        //Событие 1 "Вход заявки в ВУ"
        public class Event_Vhod_VU : TimeModelEvent<VU>
        {
            #region Атрибуты
            public VS.Zayavka Z;
            #endregion

            #region Алгоритм обработки события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>";
                Model.KVZ.Value++; //Увеличить количество вошедших заявок

                var ev2 = new Event_StartVvod_VU();    //Создаем объект события
                ev2.Z = Z;                          //Передаем заявку в следующее событие
                Model.RunEventHandlerNow(ev2);    //Вызываем событие
            }
            #endregion
        }
        //Событие 2 "Начало ввода данных заявки"
        public class Event_StartVvod_VU : TimeModelEvent<VU>
        {
            #region Атрибуты
            public VS.Zayavka Z;
            #endregion

            #region Алгоритм обработки события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>"; //Выводим в трассировку сообщение о совершившемся событии
                //Вызываем событие начала ввода данных для УВД
                var ev3 = new UstroystvoVvoda.Event_StartVvod_UVD(); //Создаем объект события
                ev3.Z = Z; //Передаем заявку в следующее событие 
                Model.UVD.RunEventHandlerNow(ev3); //Вызываем
            }
            #endregion
        }
        //Событие 3 "Конец ввода данных"
        public class Event_FinishVvod_VU : TimeModelEvent<VU>
        {
            #region Атрибуты
            public VS.Zayavka Z;
            #endregion

            #region Алгоритм обработки события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>"; //Выводим в трассировку сообщение о совершившемся событии

                //Вызываем событие 2 модели ВС Окончание ввода данных
                var ev2VS = new VS.Event_2_KonecVvoda_VS();
                ev2VS.Z = Z; //Передаем заявку
                Model.ParentVS.RunEventHandlerNow(ev2VS); //Вызываем
            }
            #endregion
        }
        //Событие 4 "Начало обработки данных"
        public class Event_StartRabota_VU : TimeModelEvent<VU>
        {
            #region Атрибуты
            public VS.Zayavka Z;
            #endregion

            #region Алгоритм обработки события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>";

                Model.KOZ.Value++; //Собираем статистику по количеству обработанных заявок

                var ev5 = new UstroystvoRabota.Event_StartRabota_UOD();
                ev5.Z = Z; //Передаем заявку в следующее событие
                Model.UR.RunEventHandlerNow(ev5);
            }
            #endregion
        }
        //Событие 5 "Конец обработки данных"
        public class Event_FinishRabota_VU : TimeModelEvent<VU>
        {
            #region Атрибуты
            public VS.Zayavka Z;
            #endregion

            #region Алгоритм обработки события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>"; //Выводим в трассировку сообщение о совершившемся событии

                //Вызываем событие 3 ВС Завершение обработки
                var ev3VS = new VS.Event_3_KonecRabota_VS();//Создаем лбъект события
                ev3VS.Z = Z;//передаем заявку в следующее событие
                ev3VS.U = Model.UR; //Передаём информацию о УОД
                Model.ParentVS.RunEventHandlerNow(ev3VS); //Вызываем
            }
            #endregion
        }
        //Событие 6 "Начало вывода данных"
        public class Event_Vyvod_VU : TimeModelEvent<VU>
        {
            #region Атрибуты
            public VS.Zayavka Z;
            #endregion

            #region Алгоритм обработки события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>"; //Выводим в трассировку сообщение о совершившемся событии

                //Планируем событие 7 Конец вывода данных
                var ev7 = new UstroystvoVyvoda.Event_StartVyvod_UVR();
                ev7.Z = Z;//Передаем заявку в следующее событие
                Z.UzelVhoda.UV.RunEventHandlerNow(ev7); //Вызываем
            }
            #endregion
        }
        //Событие 7 Конец вывода данных
        public class Event_FinishtVyvod_VU : TimeModelEvent<VU>
        {
            #region Атрибуты
            public VS.Zayavka Z;
            #endregion

            #region Алгоритм обработки события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>";

                //Вызываем событие 5 ВС "Завершение вывода данных"
                var ev5VS = new VS.Event_4_KonecVyvod_VS();
                ev5VS.Z = Z;//Передаем заявку в следующее событие
                Model.ParentVS.RunEventHandlerNow(ev5VS); //Вызываем событие
                Model.TOZ.Value = Time - Z.TimeVhod; //Обновляем время обработки заявки
            }
            #endregion
        }
    }
}