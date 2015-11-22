using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonModel.StatisticsCollecting;
using CommonModel.RandomStreamProducing;
using CommonModel.Collections;
using CommonModel.Kernel;
using System.Runtime.Serialization;
using System.IO;


namespace DSS.PSS.VS
{
    public partial class VS : Model
    {


        //Событие Вход заявки в ВС
        public class Event_1_Vhod_VS : TimeModelEvent<VS>
        {

            #region Атрибуты
            public Zayavka Z;
            #endregion

            #region Алгоритм события
            protected override void HandleEvent(ModelEventArgs args)
            {
                Model.KVZ.Value++; //Увеличить количество вошедших заявок
                // Z.Num = Model.KVZ.Value; //Номер заявки совпадает с количеством поступивших заявок
                Z.TimeVhod = Time; //Время входа заявки текущее
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + " " + Z.UzelVhoda.Name + "</br>"; //Выводим в трассировку сообщение о совершившемся событии
                //Планируем событие Вход заявки в ВС
                var ev1 = new Event_1_Vhod_VS();
                Zayavka z1 = new Zayavka(); //Создаем новую заявку
                z1.Num = Model.KVZ.Value + 3; //Даём номер заявке
                z1.UzelVhoda = Z.UzelVhoda; //Передаем узел старой заявки
                z1.RazmerVvod = z1.UzelVhoda.Gener_RazmerVvod.GenerateValue();
                z1.RazmerVyvod = z1.UzelVhoda.Gener_RazmerVyvoda.GenerateValue();
                ev1.Z = z1; //Передаем новую заявку в следующее событие
                double dt1 = z1.UzelVhoda.Gener_Vhod.GenerateValue(); //Генерируем время через которое произойдет событие 
                Model.PlanEvent(ev1, dt1); //Планируем событие 
                DSS.Modeling.TraceString += "Заявка:" + z1.Num + " " + Model.Name + " " + z1.UzelVhoda.Name + "</br>"; //Выводим в трассировку сообщение о запланированном событии

                //Вызываем событие входа заявки для 
                var ev1VU = new VU.Event_Vhod_VU();
                ev1VU.Z = Z; //Передаём заявку в событие
                Z.UzelVhoda.RunEventHandlerNow(ev1VU); //Вызываем
            }
            #endregion
        }
        //Событие Конец ввода данных
        public class Event_2_KonecVvoda_VS : TimeModelEvent<VS>
        {
            #region Атрибуты
            public Zayavka Z;
            #endregion

            #region Алгоритм события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>";
                bool flPered = false; //true-если заявку передали по каналу, false-если не передали заявку
                //Найдем узел в который можно передать заявку
                Model.VariantUslov(Model.NumVar, Z, ref flPered);

                if (flPered == false) //Если заявку не передали в другой узел
                {
                    //Вызываем событие начало обработки для ВУ
                    var ev3 = new VU.Event_StartRabota_VU();
                    ev3.Z = Z; //Передаем заявку в событие 
                    Z.UzelVhoda.RunEventHandlerNow(ev3); //Вызываем
                }
            }
            #endregion
        }
        //Событие Завершение обработки
        public class Event_3_KonecRabota_VS : TimeModelEvent<VS>
        {
            #region Атрибуты
            public Zayavka Z;
            public UstroystvoRabota U;
            #endregion

            #region Алгоритм события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>";// Выводим в трассировку информацию о событии

                if (Z.UzelVhoda.UR == U) //Если устройство обработки узла входа совпадает с утройством обработки полученным через атрибут события
                {
                    //То вызываем вывод в текущем узле
                    var ev = new VU.Event_Vyvod_VU();
                    ev.Z = Z; //Передаем заявку в событие
                    Z.UzelVhoda.RunEventHandlerNow(ev); //Вызываем
                }
                //Иначе
                else
                {
                    //Производим поиск соединяющего узлы канала
                    foreach (var kan in Model.KANAL)
                    {
                        if (((kan.VU_1 == Z.UzelVhoda) || (kan.VU_2 == Z.UzelVhoda)) && ((kan.VU_1 == U.parentVU) || (kan.VU_2 == U.parentVU)))//Если такой канал найден
                        {
                            //То передаем заявку по каналу
                            var evK = new KPD.Event_Vhod_KPD();

                            evK.Z = Z; //Передаем заявку в событие
                            evK.UzelPered = Z.UzelVhoda.Name; //Передаём название узла, в котором обрабатывалась заявка
                            kan.RunEventHandlerNow(evK); //Вызываем
                        }
                    }
                }
            }
            #endregion
        }

        //Событие Завершение вывода данных
        public class Event_4_KonecVyvod_VS : TimeModelEvent<VS>
        {
            #region Атрибуты
            public Zayavka Z;
            #endregion

            #region Алгоритм события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>"; //Выводим в трассировку сообщение о совершившемся событии
                // Вызываем событие "Выход заявки из ВС"
                var ev6 = new Event_5_Vihod_VS();
                ev6.Z = Z; //Передаем заявку в событие
                Model.RunEventHandlerNow(ev6); //Вызываем
            }
            #endregion
        }
        //Событие Выход заявки из ВС
        public class Event_5_Vihod_VS : TimeModelEvent<VS>
        {
            #region Атрибуты
            public Zayavka Z;
            #endregion

            #region Алгоритм события
            protected override void HandleEvent(ModelEventArgs args)
            {
                DSS.Modeling.TraceString += "Заявка:" + Z.Num + " " + Model.Name + "</br>"; //Выводим в трассировку сообщение о совершившемся событии
                Model.KOZ.Value++; //Увеличиваем количество обработанных заявок
                Model.TOZ.Value = Time - Z.TimeVhod; //Время обработки заявки в системе
            }
            #endregion
        }
    }
}