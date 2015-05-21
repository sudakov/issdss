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
    public class Cell
    {
        public int id;
        public string name;
        public string description;
        public int parent;
        public int parentCell;
        public int childCount;
        public int offset;
    }

    public class DataTableCols
    {
        public int id;
        public int offset;
    }

    public class myReverserClass : IComparer
    {
        // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        int IComparer.Compare(Object x, Object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(((DataTableCols)x).offset, ((DataTableCols)y).offset));
        }
    }

    public partial class Value : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Разрешения
            int id, pagePermission = 0;
            _PNL_NotEditValue.Visible = true;
            _PNL_EditValue.Visible = false;
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
                        case 31: pagePermission = id; break;
                        case 32: _PNL_NotEditValue.Visible = false; _PNL_EditValue.Visible = true; break;
                    }
                }
                if (pagePermission == 0)
                    Response.Redirect("Default.aspx");
            }
            
            if (!IsPostBack)
            {
                _TBL_Main_BindData();
            }
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
                SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Read_Names", Connection);
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
                        if (Reader["parent_crit_id"] == DBNull.Value)
                        {
                            cell = new Cell();
                            cell.id = Convert.ToInt32(Reader["id"]);
                            cell.name = Reader["name"].ToString();
                            cell.description = Reader["description"].ToString();
                            cell.parent = 0;
                            cell.childCount = 0;
                            cell.parentCell = 0;
                            alRow.Add(cell);
                        }
                        else
                        {
                            cell = new Cell();
                            cell.id = Convert.ToInt32(Reader["id"]);
                            cell.name = Reader["name"].ToString();
                            cell.description = Reader["description"].ToString();
                            cell.parent = Convert.ToInt32(Reader["parent_crit_id"]);
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
                Command = new SqlCommand("dbo.issdss_crit_value_Read_All", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                if (Context.Request.Cookies["TaskID"] != null)
                    Command.Parameters.AddWithValue("TaskID", Context.Request.Cookies["TaskID"].Value);
                if (Context.Request.Cookies["PersonID"] != null && Context.Request.Cookies["PersonID"].Value != "-1")
                    Command.Parameters.AddWithValue("PersonID", Context.Request.Cookies["PersonID"].Value);
                else
                    if (Context.Request.Cookies["UserID"] != null)
                        Command.Parameters.AddWithValue("PersonID", Context.Request.Cookies["UserID"].Value);
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
                _RP_Left_NotEdit.DataSource = dt;
                _RP_Left_NotEdit.DataBind();
                _RP_Left_Edit.DataSource = dt;
                _RP_Left_Edit.DataBind();

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