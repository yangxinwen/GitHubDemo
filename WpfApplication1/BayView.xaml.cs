using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// BayView.xaml 的交互逻辑
    /// </summary>
    public partial class BayView : UserControl
    {
        /// <summary>
        /// 列数
        /// </summary>
        private int colCount = 5;
        /// <summary>
        /// 层数
        /// </summary>
        private int rowCount = 5;
        private bool leftToRight = true;
        private List<List<ContainerBox>> listBorder = new List<List<ContainerBox>>();
        public BayView()
        {
            InitializeComponent();
            InitBox();
        }

        private void InitBox()
        {
            gridBoxArea.Children.Clear();
            int colCount = 5, rowCount = 5; //列\层
            this.colCount = colCount;
            this.rowCount = rowCount;
            //列是否从左到右
            bool leftToRight = false;
            this.leftToRight = leftToRight;
            var arry = new int[colCount, rowCount];  //1-有箱  2-无箱  3-不能放箱

            //设置初始数据和表格
            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    arry[i, j] = 1;
                }
            }

            Grid grid = new Grid();
            for (int i = 0; i < colCount + 2; i++)
            {
                var tmp = new ColumnDefinition();
                if (i > 0 && i < colCount + 1)
                    tmp.Width = GridLength.Auto;
                grid.ColumnDefinitions.Add(tmp);
            }

            for (int i = 0; i < rowCount + 1; i++)
            {
                var tmp = new RowDefinition();
                if (i > 0)
                    tmp.Height = GridLength.Auto;
                grid.RowDefinitions.Add(tmp);
            }

            //初始化箱控件
            for (int row = 0; row < rowCount + 1; row++)
            {
                listBorder.Add(new List<ContainerBox>());
                for (int col = 0; col < colCount + 2; col++)
                {
                    ContainerBox box = new ContainerBox();
                    box.Width = 50;
                    box.Height = 50;
                    box.SetValue(Grid.RowProperty, row);
                    box.SetValue(Grid.ColumnProperty, col);
                    box.BorderThickness = new Thickness(1);
                    box.BorderBrush = Brushes.Black;
                    if (col == 0 || col == colCount + 1)
                    {
                        if (row == 0) continue;
                        box.Flag = true;
                        box.Text = rowCount - row + 1 + "";
                        if (col == 0)
                            box.HorizontalAlignment = HorizontalAlignment.Right;
                        else
                            box.HorizontalAlignment = HorizontalAlignment.Left;
                        box.BorderThickness = new Thickness(0);
                    }
                    else if (row == 0)
                    {
                        box.Flag = true;
                        box.Text = (leftToRight ? col : colCount - col + 1) + "";
                        box.VerticalAlignment = VerticalAlignment.Bottom;
                        box.BorderThickness = new Thickness(0);
                    }
                    else
                    {
                        box.Flag = true;
                        box.Text = row + "," + col;
                        listBorder[row].Add(box);
                    }



                    grid.Children.Add(box);
                }
            }
            gridBoxArea.Children.Add(grid);
        }


      


        /// <summary>
        /// 标准坐标转换成贝位图坐标(起始坐标都是0开始)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private KeyValuePair<int, int> ConvertToViewPoint(int x, int y)
        {
            int x1, y1;
            x1 = this.rowCount - x - 1;

            if (leftToRight)
                y1 = this.colCount - y - 1;
            else
                y1 = this.colCount;
            return new KeyValuePair<int, int>(x1, y1);
        }
        /// <summary>
        /// 贝位图坐标转换成标准坐标(起始坐标都是0开始)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private KeyValuePair<int, int> ConvertToAdvanPoint(int row, int col)
        {
            int x, y;
            x = this.rowCount - row - 1;

            if (leftToRight)
                y = this.colCount - col - 1;
            else
                y = this.colCount;
            return new KeyValuePair<int, int>(x, y);
        }

        public void Refresh()
        {
            InitBox();
        }
    }
}
