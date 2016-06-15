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

        private List<List<ContainerBox>> listBorder = new List<List<ContainerBox>>();
        public BayView()
        {
            InitializeComponent();
            InitBox();
        }

        private void InitBox()
        {
            gridBoxArea.Children.Clear();
            int x = 3, y = 5; //列\层

            var arry = new int[x, y];  //1-有箱  2-无箱  3-不能放箱

            //设置初始数据和表格
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    arry[i, j] = 1;
                }
            }

            Grid grid = new Grid();
            for (int i = 0; i < x + 2; i++)
            {
                var tmp = new ColumnDefinition();
                if (i > 0 && i < x + 1)
                    tmp.Width = GridLength.Auto;
                grid.ColumnDefinitions.Add(tmp);
            }

            for (int i = 0; i < y + 1; i++)
            {
                var tmp = new RowDefinition();
                if (i > 0)
                    tmp.Height = GridLength.Auto;
                grid.RowDefinitions.Add(tmp);
            }

            //初始化箱控件
            for (int row = 0; row < y + 1; row++)
            {
                listBorder.Add(new List<ContainerBox>());
                for (int col = 0; col < x + 2; col++)
                {
                    ContainerBox box = new ContainerBox();
                    box.Width = 50;
                    box.Height = 50;
                    box.SetValue(Grid.RowProperty, row);
                    box.SetValue(Grid.ColumnProperty, col);
                    box.BorderThickness = new Thickness(1);
                    box.BorderBrush = Brushes.Black;
                    if (col == 0 || col == x + 1)
                    {
                        if (row == 0) continue;
                        box.Flag = true;
                        box.Text =y-row+1 + "";
                        if (col == 0)
                            box.HorizontalAlignment = HorizontalAlignment.Right;
                        else
                            box.HorizontalAlignment = HorizontalAlignment.Left;
                        box.BorderThickness = new Thickness(0);
                    }
                    else if (row == 0)
                    {
                        box.Flag = true;
                        box.Text = col + "";
                        box.VerticalAlignment = VerticalAlignment.Bottom;
                        box.BorderThickness = new Thickness(0);
                    }
                    else
                    {
                        listBorder[row].Add(box);
                    }
                    grid.Children.Add(box);
                }
            }
            gridBoxArea.Children.Add(grid);
        }

        public void Refresh()
        {
            InitBox();
        }
    }
}
