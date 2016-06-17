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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemo
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
            listBorder.Clear();
            int colCount = 6, rowCount = 7; //列\层
            this.colCount = colCount;
            this.rowCount = rowCount;
            //列是否从左到右
            bool leftToRight = true;
            this.leftToRight = leftToRight;
            var arry = new int[rowCount, colCount];  //1-有箱  2-无箱  3-不能放箱

            //设置初始数据和表格
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
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
                        //box.Flag = true;
                        //box.Text = row + "," + col;
                        if (listBorder.Count <= row)
                            listBorder.Add(new List<ContainerBox>());
                        listBorder[row - 1].Add(box);
                    }



                    grid.Children.Add(box);
                }
            }
            gridBoxArea.Children.Add(grid);

            SetBoxStatus(arry);
        }


        private void SetBoxStatus(int[,] boxs)
        {
            //不能放置
            boxs[6, 5] = 3;
            boxs[6, 4] = 3;
            boxs[6, 3] = 3;
            boxs[6, 2] = 3;
            boxs[6, 1] = 3;
            boxs[6, 0] = 3;
            boxs[5, 5] = 3;
            boxs[5, 4] = 3;
            boxs[4, 5] = 3;

            //空箱区
            boxs[5, 3] = 2;
            boxs[5, 2] = 2;
            boxs[5, 1] = 2;
            boxs[5, 0] = 2;
            boxs[4, 4] = 2;
            boxs[4, 3] = 2;
            boxs[4, 2] = 2;
            boxs[3, 5] = 2;
            boxs[3, 4] = 2;
            boxs[3, 2] = 2;
            boxs[2, 5] = 2;
            boxs[2, 4] = 2;
            boxs[1, 5] = 2;
            boxs[1, 4] = 2;
            boxs[0, 5] = 2;


            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    var keyValue = ConvertToAdvanPoint(row, col);
                    listBorder[keyValue.Key][keyValue.Value].BoxStatus = boxs[row, col];
                }
            }

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
            x1 = this.rowCount - x;

            if (leftToRight)
                y1 = this.colCount - y;
            else
                y1 = this.colCount;
            x1--;
            y1--;
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
            x = this.rowCount-1 - row;

            if (leftToRight)
                y = col;
            else
                y = this.colCount-1 - col;
            
            return new KeyValuePair<int, int>(x, y);
        }

        public void Refresh()
        {




            //ThicknessAnimation ta = new ThicknessAnimation();
            //ta.From = minCart.Margin;             //起始值
            //ta.To = new Thickness(gridDevice.ActualWidth-100, minCart.Margin.Top, minCart.Margin.Right, minCart.Margin.Bottom);        //结束值
            //ta.Duration = TimeSpan.FromSeconds(3);         //动画持续时间
            //this.minCart.BeginAnimation(TextBlock.MarginProperty, ta);//开始动画


            //InitBox();
            //var keyValue = ConvertToAdvanPoint(4, 0);
            //listBorder[keyValue.Key][keyValue.Value].BoxStatus = 3;

            return;
        }
    }
}
