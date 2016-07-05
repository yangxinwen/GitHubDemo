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

            //var list = new List<object>();
            //for (int i = 0; i < 100; i++)
            //{
            //    list.Add(new { Name="test"+(i+1)});
            //}
            //gc.ItemsSource = list;
            //gc.ShowOrderNum = Visibility.Visible;

            var floyd = new Floyd();
            floyd.Test();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ct.IsHaveBox = !ct.IsHaveBox;

            //bayView.Refresh();
            bayView.Test1();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bayView.Test2();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bayView.Test3();
        }
    }
}
