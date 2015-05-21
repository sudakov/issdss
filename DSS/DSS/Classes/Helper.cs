using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DSS.DSS.Classes
{
    /*
     * сюда стоит выкладывать процедуры, переменные и тп, которые используются или могут использоваться в нескольких местах
     * 
     */


    public class Helper
    {

        // Моя математика
        public class MyMath
        {
            public static string Round(string s)
            {
                if ((s.Contains(".") || s.Contains(",")) && (s[s.Length - 1].Equals('0') || s[s.Length - 1].Equals('.') || s[s.Length - 1].Equals(',')))
                {
                    s = s.Remove(s.Length - 1);
                    return Round(s);
                }
                else
                    return s;
            }
        }

        // класс дерева
        public class CTreeNode
        {
            public string id;
            public string idFather;
            public string name;

            /// <summary>
            /// максимальная длина отображаемых элементов дерева
            /// </summary>
            public int max_length_node_name
            {
                get
                {
                    return 100;
                }
            }

            /// <summary>
            /// возвращает коллекцию неупорядоченных элкментов дерева ввиде списка
            /// </summary>
            /// <param name="TaskID"></param>
            /// <returns></returns>
            public List<CTreeNode> GetTreeItem(string TaskID)
            {
                    List<CTreeNode> TreeList = new List<CTreeNode>();
                    using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
                    {

                        SqlCommand Command = new SqlCommand("dbo.issdss_criteria_Read_Names", Connection);
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@TaskID", TaskID);
                        Connection.Open();
                        SqlDataReader Reader = Command.ExecuteReader();


                        // пока есть такие элементы, вызываем каждый такой элемент
                        while (Reader.Read())
                        {
                            TreeList.Add(new Helper.CTreeNode()
                            {
                                id = Reader["id"].ToString(),
                                idFather = Reader["parent_crit_id"].ToString(),
                                name = Reader["name"].ToString()
                            });
                        }

                        Reader.Close();
                        Connection.Close();
                    }
                    return TreeList;
            }

            /// <summary>
            /// рекурсивно обходит дерево TreeList, начиная с узла idDirectSon("13") в поисках дочернего узла idDirectFather ("4")
            /// idDirectSon, idDirectFather - номера id направледний исследований, а не номера списка в дереве TreeList
            /// возвращает "-1" - дерево составлено неправильно, либо узел отца или сына не существует, "true" - нашли узел, 
            /// "false" - дошли до корня дерева и не встретили узел.
            /// </summary>
            public string IsFather(string TaskID, IEnumerable<Helper.CTreeNode> TreeList, string idDirectFather, string idDirectSon)
            {
                try
                {
                    // легче идти от сына вверх, рассматривая одну ветку, чем от отца и рассматривать все ветки.
                    // найдём индексы отца в списке дерева
                    // поскольку индексы не посторяются, то можно найти первый попавшийся индекс (он и будет единственный)
                    var TreeIndexF = TreeList.First(x => x.id == idDirectFather);
                    // найдём индексы отца в списке дерева
                    var TreeIndexS = TreeList.First(x => x.id == idDirectSon);

                    // если не нашли такого изла в дереве вообще, то ОШИБКА данных
                    if (TreeIndexF == null || TreeIndexS == null)
                        return "-1";

                    // дерево закончилось?
                    if (TreeIndexS.idFather == null || TreeIndexS.idFather == "")
                        return "false";
                    else
                        // у сына это отец?
                        if (TreeIndexS.idFather == TreeIndexF.id)
                            return "true";
                        else
                            // ищем дальше
                            return IsFather(TaskID, TreeList, idDirectFather, TreeIndexS.idFather);
                }
                catch
                {
                    /// если один из параметров null или, к примеру, TreeList.First(x => x.id == idDirectFather); не был найден
                    return "-1";
                }
            }
            /// <summary>
            /// рекурсивно обходит дерево TreeList, начиная с узла idDirectSon("13") в поисках дочернего узла idDirectFather ("4")
            /// idDirectSon, idDirectFather - номера id направледний исследований, а не номера списка в дереве TreeList
            /// возвращает "-1" - дерево составлено неправильно, либо узел отца или сына не существует, "true" - нашли узел, 
            /// "false" - дошли до корня дерева и не встретили узел.
            /// 
            /// ВНИМАНИЕ! Список элементов подгружается из БД!
            /// </summary>
            /// <param name="TaskID"></param>
            /// <param name="idDirectFather"></param>
            /// <param name="idDirectSon"></param>
            /// <returns></returns>
            public string IsFather(string TaskID, string idDirectFather, string idDirectSon)
            {
                List<CTreeNode> TreeList = GetTreeItem(TaskID);
                return IsFather(TaskID, TreeList, idDirectFather, idDirectSon);
            }
            /// <summary>
            /// рекурсивно заполняет дерево в TreeView.
            /// </summary>
            /// <param name="TreeNodeCollection">это все отцы и братья на этом уровне</param>
            /// <param name="parentNode">id отца для поиска в бд (по сути это аналог глубины)</param>
            /// <param name="selectItem">выбранный id узла, который должен быть раскрыт</param>
            /// <param name="TreeView"></param>
            public void BindTree(string TaskID, 
                IEnumerable<Helper.CTreeNode> TreeNodeCollection,
                System.Web.UI.WebControls.TreeNode parentNode,
                string selectItem,
                System.Web.UI.WebControls.TreeView TreeView)
            {
                var nodes = TreeNodeCollection.Where(x => parentNode == null ? x.idFather == "" : x.idFather == parentNode.Value);
                foreach (var node in nodes)
                {
                    System.Web.UI.WebControls.TreeNode newNode = new System.Web.UI.WebControls.TreeNode(
                        node.name.Length >= max_length_node_name ? node.name.Remove(max_length_node_name) : node.name,
                        node.id.ToString());
                    // выберем узел, если он и есть number
                    if (selectItem == node.id || IsFather(TaskID, TreeNodeCollection, node.id, selectItem) == "true")
                    {
                        newNode.Expanded = true;
                        newNode.Select();
                    }

                    if (parentNode == null)
                        TreeView.Nodes.Add(newNode);
                    else
                        parentNode.ChildNodes.Add(newNode);

                    BindTree(TaskID, TreeNodeCollection, newNode, selectItem, TreeView);
                }
            }
        }

        // класс парных шкал для критериев
        public class CriteriaPair
        {
            private List<Item> _items = new List<Item>();

            public class Item
            {
                string _id1;
                string _id2;
                string _id;

                string _name1;
                string _name2;
                string _name;

                decimal _rank;


                public Item()
                {
                    _id1 = String.Empty;
                    _id2 = String.Empty;
                    _id = String.Empty;
                    _rank = 0;
                }
                public Item(string id, decimal rank)
                {
                    _id1 = String.Empty;
                    _id2 = String.Empty;
                    _id = id;
                    _rank = rank;
                }
                public Item(string id1, string name1)
                {
                    _name1 = name1;
                    _id1 = id1;
                }
                public Item(string id1, string id2, decimal rank)
                {
                    _id1 = id1;
                    _id2 = id2;
                    _id = String.Empty;
                    _rank = rank;
                }
                public Item(string id1, string id2, string id, decimal rank)
                {
                    _id1 = id1;
                    _id2 = id2;
                    _id = id;
                    _rank = rank;
                }
                public Item(string id1, string name1, string id2, string name2, decimal rank)
                {
                    _id1 = id1;
                    _id2 = id2;
                    _name1 = name1;
                    _name2 = name2;
                    _rank = rank;
                }
                public string id
                {
                    set
                    {
                        _id = value;
                    }
                    get
                    {
                        return _id;
                    }
                }
                public string id1
                {
                    set
                    {
                        _id1 = value;
                    }
                    get
                    {
                        return _id1;
                    }
                }
                public string id2
                {
                    set
                    {
                        _id2 = value;
                    }
                    get
                    {
                        return _id2;
                    }
                }
                public string name1
                {
                    set
                    {
                        _name1 = value;
                    }
                    get
                    {
                        return _name1;
                    }
                }
                public string name2
                {
                    set
                    {
                        _name2 = value;
                    }
                    get
                    {
                        return _name2;
                    }
                }
                public string name
                {
                    set
                    {
                        _name = value;
                    }
                    get
                    {
                        return _name;
                    }
                }
                public decimal rank
                {
                    set
                    {
                        _rank = value;
                    }
                    get
                    {
                        return _rank;
                    }
                }
            }

            public void Add(string id, decimal rank)
            {
                _items.Add(new Item(id, rank));
            }
            public void Add(string id1, string name1)
            {
                _items.Add(new Item(id1, name1));
            }
            public void Add(string id1, string id2, decimal rank)
            {
                _items.Add(new Item(id1, id2, rank));
            }
            public void Add(string id1, string id2, string id, decimal rank)
            {
                _items.Add(new Item(id1, id2, id, rank));
            }
            public void Add(string id1, string name1, string id2, string name2, decimal rank)
            {
                _items.Add(new Item(id1, name1, id2, name2, rank));
            }
            public int Count
            {
                get
                {
                    return _items.Count;
                }
            }

            public Item Get(int index)
            {
                    return _items[index];
            }

            public int IndexOf(Item Elem)
            {
                for (int i = 0; i < _items.Count; i++)
                    if (Elem == _items[i])
                        return i;
                return -1;
            }
            public int IndexOf(string id)
            {
                for (int i = 0; i < _items.Count; i++)
                    if (id == _items[i].id)
                        return i;
                return -1;
            }
            public int IndexOf(string id1, string id2)
            {
                for (int i = 0; i < _items.Count; i++)
                    if (id1 == _items[i].id1 && id2 == _items[i].id2)
                        return i;
                return -1;
            }
            public void Delete(int index)
            {
                _items.RemoveAt(index);
            }
            public void Delete(int index, int count)
            {
                _items.RemoveRange(index, count);
            }
        }
    }
}