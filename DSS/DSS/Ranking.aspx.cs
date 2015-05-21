using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DSS.DSS.Classes;

namespace DSS.DSS
{
    public partial class Ranking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Разрешения
            int id, pagePermission = 0;
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_permission_Read", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.Cookies["TaskID"] != null)
                    Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                if (Context.Request.Cookies["UserID"] != null)
                    Command.Parameters.AddWithValue("PersonID", Context.Request.Cookies["UserID"].Value);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                Reader.Read(); // Пропускаем первую строчку с именем пользователя
                while (Reader.Read())
                {
                    id = Convert.ToInt32(Reader["permission_id"]);
                    switch (id)
                    {
                        case 41: pagePermission = id; break;
                        case 42: _IMG_Range.Visible = true; break;
                    }
                }
                if (pagePermission == 0)
                    Response.Redirect("Default.aspx");
            }
            
            if (!IsPostBack)
            {
                _TBL_Main_BindData();
            }
            _IMG_Range.Click += new ImageClickEventHandler(_IMG_Range_Click);
        }

        void _IMG_Range_Click(object sender, ImageClickEventArgs e)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.prc_calculate_task", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.Cookies["TaskID"] != null)
                    Command.Parameters.AddWithValue("@task_id", Context.Request.Cookies["TaskID"].Value);
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            _TBL_Main_BindData();
        }

        void _TBL_Main_BindData()
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                ArrayList alTable = new ArrayList();
                ArrayList alRow = new ArrayList();
                ArrayList alDraft = new ArrayList();
                Cell cell;

                // Формируем шапку
                SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Read_AllNames", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.Cookies["TaskID"] != null)
                    Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                // Копируем селект в черновой массив ячеек, попутно создавая первую строчку шапки в if'е
                try
                {
                    while (Reader.Read())
                    {
                        if (Reader["parent"] == DBNull.Value)
                        {
                            cell = new Cell();
                            cell.id = Convert.ToInt32(Reader["c_id"]);
                            cell.name = Reader["c_name"].ToString();
                            cell.description = Reader["description"].ToString();
                            cell.parent = 0;
                            cell.childCount = 0;
                            cell.parentCell = 0;
                            alRow.Add(cell);
                        }
                        else
                        {
                            cell = new Cell();
                            cell.id = Convert.ToInt32(Reader["c_id"]);
                            if (Reader["rank"].ToString() == "1")
                            {
                                cell.name = Reader["c_name"].ToString();
                                cell.description = "Значение агрегированного показателя.\nСвёртка по критерию: " + Reader["c_name"].ToString() + ".";
                            }
                            else
                            {
                                cell.name = Reader["c_name"].ToString();
                                cell.description = Reader["description"].ToString();
                            }
                            cell.parent = Convert.ToInt32(Reader["parent"]);
                            cell.childCount = 0;
                            alDraft.Add(cell);
                        }
                    }
                    alTable.Add(alRow);
                }
                finally
                {
                    Reader.Close();
                }

                int i, j, k;

                // Перебираем черновой массив ячеек, чтобы определится со структурой шапки
                for (i = 0; alDraft.Count > 0; i++)
                {
                    alRow = new ArrayList();
                    for (j = 0; j < ((ArrayList)alTable[i]).Count; j++)
                    {
                        for (k = 0; k < alDraft.Count; k++)
                        {
                            if (((Cell)alDraft[k]).parent == ((Cell)((ArrayList)alTable[i])[j]).id)
                            {
                                ((Cell)alDraft[k]).parentCell = j;
                                alRow.Add(alDraft[k]);
                                alDraft.RemoveAt(k);
                                k--;
                                ((Cell)((ArrayList)alTable[i])[j]).childCount++;
                            }
                        }
                    }
                    alTable.Add(alRow);
                }

                // Опять перебираем уже готовый массив для определения ColumnSpan
                for (i = alTable.Count - 3; i >= 0; i--)
                    for (j = 0; j < ((ArrayList)alTable[i]).Count; j++)
                        for (k = 0; k < ((ArrayList)alTable[i + 1]).Count; k++)
                            if (((Cell)((ArrayList)alTable[i])[j]).id == ((Cell)((ArrayList)alTable[i + 1])[k]).parent &&
                                ((Cell)((ArrayList)alTable[i + 1])[k]).childCount > 0)
                                ((Cell)((ArrayList)alTable[i])[j]).childCount += ((Cell)((ArrayList)alTable[i + 1])[k]).childCount - 1;

                // И снова перебираем готовый массив для определения порядковых номеров базовых столбцов
                int o, parentID;
                for (i = 0; i < alTable.Count; i++)
                {
                    o = 0;
                    parentID = -1;
                    for (j = 0; j < ((ArrayList)alTable[i]).Count; j++)
                    {
                        cell = ((Cell)((ArrayList)alTable[i])[j]);
                        if (cell.parentCell != parentID && i > 0)
                            o = ((Cell)((ArrayList)alTable[i - 1])[cell.parentCell]).offset;
                        cell.offset = o;
                        if (cell.childCount == 0)
                            o++;
                        else
                            o += cell.childCount;
                        parentID = cell.parentCell;
                    }
                }

                //
                // Формирование шапки
                //
                TableRow tRow = null;
                TableCell tCell = null;
                ArrayList cols = new ArrayList();
                DataTableCols dtc;
                int columnsNumber = 0;

                for (i = 0; i < alTable.Count; i++)
                {
                    tRow = new TableRow();
                    for (j = 0; j < ((ArrayList)alTable[i]).Count; j++)
                    {
                        cell = ((Cell)((ArrayList)alTable[i])[j]);
                        tCell = new TableCell();
                        tCell.Text = cell.name;
                        tCell.ToolTip = cell.description;
                        if (cell.id == cell.parent)
                            tCell.CssClass = "_TBL_HeadCellParent";
                        else
                            tCell.CssClass = "_TBL_HeadCell";
                        tCell.Wrap = false;
                        tCell.ColumnSpan = cell.childCount;
                        if (cell.childCount == 0)
                        {
                            tCell.RowSpan = alTable.Count - i;
                            tCell.Wrap = true;
                            tCell.ID = "_COL_" + cell.offset.ToString();
                            dtc = new DataTableCols();
                            dtc.id = cell.id;
                            dtc.offset = cell.offset;
                            cols.Add(dtc);
                            columnsNumber++;
                        }
                        tCell.HorizontalAlign = HorizontalAlign.Center;
                        tRow.Cells.Add(tCell);
                    }
                    _TBL_Head.Rows.Add(tRow);
                }

                //
                // Сортировка массива с номерами столбцов
                //
                cols.Sort(new myReverserClass());

                //
                // Формирование столбцов таблицы данных для основной части
                //
                DataTable dtMain = new DataTable();
                DataRow drMain = null;
                DataColumn dcMain;
                for (i = 0; i < columnsNumber; i++)
                {
                    dcMain = new DataColumn();
                    dcMain.DataType = System.Type.GetType("System.String");
                    dcMain.ColumnName = ((DataTableCols)cols[i]).id.ToString();
                    dtMain.Columns.Add(dcMain);
                }

                //
                // Создание столбцов левой таблицы под Репитер
                //
                string alternativeID = null;
                DataTable dt = new DataTable();
                DataRow dr;
                DataColumn dc;

                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.Int32");
                dc.ColumnName = "id";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.String");
                dc.ColumnName = "name";
                dt.Columns.Add(dc);

                //
                // Чтение из базы
                //
                Command = new SqlCommand("dbo.issdss_crit_value_Read_Ranks", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.Cookies["TaskID"] != null)
                    Command.Parameters.AddWithValue("@TaskID", Context.Request.Cookies["TaskID"].Value);
                Reader = Command.ExecuteReader();
                try
                {
                    while (Reader.Read())
                    {
                        if (alternativeID != Reader["a_id"].ToString())
                        {
                            if (alternativeID != null)
                                dtMain.Rows.Add(drMain);
                            dr = dt.NewRow();
                            dr["id"] = Convert.ToInt32(Reader["a_id"]);
                            dr["name"] = Reader["a_name"].ToString();
                            dt.Rows.Add(dr);
                            alternativeID = Reader["a_id"].ToString();
                            drMain = dtMain.NewRow();
                            //drMain["0"] = Helper.MyMath.Round(Try2Str(Reader["rank"]));
                        }
                        drMain[Reader["c_id"].ToString()] = Helper.MyMath.Round(Try2Str(Reader["value_for_view"]));
                    }
                    // Добавление последней строки, если Reader вытащил хоть что-то
                    if (drMain != null)
                        dtMain.Rows.Add(drMain);
                }
                finally
                {
                    Reader.Close();
                }

                //
                // Биндинг левой таблицы
                //
                _RP_Left.DataSource = dt;
                _RP_Left.DataBind();

                //
                // Заполнение основной таблицы _TBL_Main из dtMain
                //
                int alternativeRow = 1;
                for (i = 0; i < dtMain.Rows.Count; i++)
                {
                    tRow = new TableRow();
                    for (j = 0; j < dtMain.Columns.Count; j++)
                    {
                        tCell = new TableCell();
                        tCell.Text = dtMain.Rows[i][j].ToString();
                        if (alternativeRow == 1)
                            tCell.CssClass = "_TBL_MainCell";
                        else
                            tCell.CssClass = "_TBL_MainCellAlt";
                        tRow.Cells.Add(tCell);
                    }
                    _TBL_Main.Rows.Add(tRow);
                    alternativeRow *= -1;
                }
            }
        }

        string Try2Str(object Obj)
        {
            return ((Obj == null) || (Obj.ToString().Trim() == "")) ? "-" : Obj.ToString();
        }
    }
}