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

namespace WPFDemo.Contents
{
    /// <summary>
    /// DataGridDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridDemo : UserControl
    {
        public DataGridDemo()
        {
            InitializeComponent();

            dataGrid.AutoGenerateColumns = false;
            var list = new List<Student>();
            list.Add(new Student() { Name = "test1", Sex = "Male", Age = 18, Grade = 69, Detail = new DetailInfo() { Height = 170, Weight = 130, Address = "中国广东省深圳市" } });
            list.Add(new Student() { Name = "tessdfdddddddddddddt2", Sex = "Female", Age = 20, Grade = 80, Detail = new DetailInfo() { Height = 176, Weight = 120, Address = "中国广东省深圳市" } });
            list.Add(new Student() { Name = "test3", Sex = "Female", Age = 17, Grade = 99, Detail = new DetailInfo() { Height = 160, Weight = 180, Address = "中国广东省深圳市" } });
            list.Add(new Student() { Name = "test4", Sex = "Male", Age = 19, Grade = 88, Detail = new DetailInfo() { Height = 175, Weight = 130, Address = "中国广东省深圳市" } });
            list.Add(new Student() { Name = "test5", Sex = "Male", Age = 18, Grade = 63, Detail = new DetailInfo() { Height = 170, Weight = 200, Address = "中国广东省深圳市" } });
            dataGrid.ItemsSource = list;
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
