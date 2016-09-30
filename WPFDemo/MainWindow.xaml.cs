using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Util;
using WPFDemo.Contents;

namespace WPFDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

            Debug.WriteLine("sdfsdfsdf");
            

            //var dic = new Dictionary<int, string>();

            //for (int i = 0; i < 10000; i++)
            //{
            //    dic.Add(i, (i+1).ToString());
            //}

            //cbx.DisplayMemberPath = "Value";
            //cbx.SelectedValuePath = "Key";
            //cbx.ItemsSource = dic;

            //menu.StaysOpen = false;

            //menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            //menu.PlacementTarget = btn;


        }



        private void Menu_Closed(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("sdfdsf");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(1 * 10);
            //    this.Dispatcher.Invoke(new Action(() => { ps.OptimizeAllLine(); }));

            //});



            //btn.ToolTip = new Controls.UCToolTip() { Content = (new Button() { Content = "sdfdfs" }) };


            //var dic = new Dictionary<int, string>();
            //for (int i = 1; i < 100000; i++)
            //{
            //    dic.Add(i, "test" + i);
            //}
            //cbx.DisplayMemberPath = "Value";
            //cbx.SelectedValuePath = "Key";
            //cbx.ItemsSource = dic;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //ps.SaveData();
        }

        //private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    var row = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem) as DataGridRow;
        //    row.DetailsVisibility = Visibility.Visible;
        //}

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void TreeView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if()
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tv.SelectedItem == null)
                return;
            var item = tv.SelectedItem as TreeViewItem;
            if (item == null || item.Tag == null)
                return;

            FrameworkElement ele = null;
            switch (item.Tag.ToString())
            {
                case "MoveElementDemo":
                    ele = new MoveElementDemo();
                    break;
                case "DataGridDemo":
                    ele = new DataGridDemo();
                    break;
                case "ButtonDemo":
                    ele = new ButtonDemo();
                    break;
                default:
                    break;
            }

            if (ele != null)
            {
                grid.Children.Clear();
                grid.Children.Add(ele);
            }

        }
    }
}
