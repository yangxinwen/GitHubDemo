using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemo.Contents
{
    /// <summary>
    /// TreeDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class TreeDataGridDemo : UserControl
    {
        public TreeDataGridDemo()
        {
            InitializeComponent();
            InitData();


            this.AddHandler(ToggleButton.ClickEvent, new RoutedEventHandler(Expander_Click));
            //dataGrid.Columns[0].
        }



        private void InitData()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("ParrentId");
            dt.Columns.Add("Name");
            dt.Columns.Add("Sex");
            dt.Columns.Add("Age");

            var row = dt.NewRow();
            row["Id"] = 1;
            row["ParrentId"] = 0;
            row["Name"] = "test1";
            row["Sex"] = "男";
            row["Age"] = 26;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Id"] = 2;
            row["ParrentId"] = 1;
            row["Name"] = "test2";
            row["Sex"] = "男";
            row["Age"] = 28;
            dt.Rows.Add(row);


            row = dt.NewRow();
            row["Id"] = 5;
            row["ParrentId"] = 0;
            row["Name"] = "test5";
            row["Sex"] = "男";
            row["Age"] = 21;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Id"] = 3;
            row["ParrentId"] = 1;
            row["Name"] = "test3";
            row["Sex"] = "女";
            row["Age"] = 22;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Id"] = 4;
            row["ParrentId"] = 3;
            row["Name"] = "test4";
            row["Sex"] = "男";
            row["Age"] = 30;
            dt.Rows.Add(row);



            var newDt = SortDataTable(dt);
            _newDt = newDt;
            SetRowCanExpand();
            dataGrid.ItemsSource = newDt.DefaultView;
        }

        private DataTable _newDt = null;
        /// <summary>
        /// 绑定的父节点字段
        /// </summary>
        private string ParrentIdField { get; set; } = "ParrentId";
        /// <summary>
        /// 绑定的id字段
        /// </summary>
        private string IdField { get; set; } = "Id";



        private DataTable SortDataTable(DataTable dt)
        {
            List<string> ids = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                var id = item[IdField].ToString();
                if (ids.Contains(id))
                    continue;
                ids.Add(id);
            }

            var newDt = dt.Clone();
            newDt.Columns.Add("IsExpand");
            newDt.Columns.Add("IsVisible");
            newDt.Columns.Add("IsHaveChild");
            newDt.Columns.Add("Margin");

            //根节点标识，可自动推算出，父节点为null或者是不存在的节点为根节点
            var rootId = "0";

            SortData(dt, newDt, rootId, new Thickness(0));
            return newDt;

        }
        /// <summary>
        /// 使用递归方式排列行，使子数据行紧跟父数据行
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newDt"></param>
        /// <param name="id"></param>
        private void SortData(DataTable dt, DataTable newDt, string id, Thickness margin)
        {
            if (newDt.Rows.Count >= dt.Rows.Count)
                return;

            //与子节点间的间距
            margin.Left += 10;

            foreach (DataRow item in dt.Rows)
            {
                if (id.Equals(item[ParrentIdField]))
                {
                    var row = newDt.Rows.Add(item.ItemArray);
                    if (id.Equals("0"))
                        row["IsVisible"] = 1;
                    else
                        row["IsVisible"] = 0;
                    row["Margin"] = margin;
                    SortData(dt, newDt, item[IdField].ToString(), margin);
                }
            }
        }

        private void SetRowCanExpand()
        {
            foreach (DataRow item in _newDt.Rows)
            {
                if (IsRowHaveChild(item))
                    item["IsHaveChild"] = 1;
            }
        }

        private bool IsRowHaveChild(DataRow row)
        {
            var id = row[IdField].ToString();
            var rows = _newDt.Select(ParrentIdField + "=" + id);
            //foreach (var item in collection)
            //{

            //}
            if (rows != null && rows.Length > 0)
                return true;
            else
                return false;
        }

        private void CollapsedRow(string id)
        {
            foreach (DataRow item in _newDt.Rows)
            {
                if (id.Equals(item[ParrentIdField]))
                {
                    item["IsVisible"] = 0;
                    CollapsedRow(item[IdField].ToString());
                }
            }
        }
        private void ExpandRow(DataRow row)
        {
            string id = row[IdField].ToString();
            foreach (DataRow item in _newDt.Rows)
            {
                if (id.Equals(item[ParrentIdField]))
                {
                    if ("1".Equals(row["IsExpand"]))
                    {
                        item["IsVisible"] = 1;
                        ExpandRow(item);
                    }
                }
            }
        }

        private void Expander_Click(object sender, RoutedEventArgs e)
        {
            var row = (e.OriginalSource as FrameworkElement).DataContext as DataRowView;
            if (row == null)
                return;
            var selectRow = row;
            if ("1".Equals(selectRow["IsExpand"]))
            {
                selectRow["IsExpand"] = 0;
                CollapsedRow(selectRow[IdField].ToString());
            }
            else
            {
                selectRow["IsExpand"] = 1;
                ExpandRow(selectRow.Row);
            }
        }
    }
}
