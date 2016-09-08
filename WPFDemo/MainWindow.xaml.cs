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

            dataGrid.AutoGenerateColumns = false;
            var list = new List<Student>();
            list.Add(new Student() { Name = "test1", Sex = "Male", Age = 18, Grade = 69, Detail=new DetailInfo() { Height=170,Weight=130, Address="中国广东省深圳市" } });
            list.Add(new Student() { Name = "tessdfdddddddddddddt2", Sex = "Female", Age = 20, Grade = 80, Detail = new DetailInfo() { Height = 176, Weight = 120, Address = "中国广东省深圳市" } });
            list.Add(new Student() { Name = "test3", Sex = "Female", Age = 17, Grade = 99, Detail = new DetailInfo() { Height = 160, Weight = 180, Address = "中国广东省深圳市" } });
            list.Add(new Student() { Name = "test4", Sex = "Male", Age = 19, Grade = 88, Detail = new DetailInfo() { Height = 175, Weight = 130, Address = "中国广东省深圳市" } });
            list.Add(new Student() { Name = "test5", Sex = "Male", Age = 18, Grade = 63, Detail = new DetailInfo() { Height = 170, Weight = 200, Address = "中国广东省深圳市" } });
            dataGrid.ItemsSource = list;

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
            Debug.WriteLine(dataGrid.SelectedItem);
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public double Grade { get; set; }
        public DetailInfo Detail { get; set; } = new DetailInfo();

    }

    public class DetailInfo
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Address { get; set; }
    }

}
