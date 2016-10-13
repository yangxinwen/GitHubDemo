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
    /// HierarchicalDataTemplateDemo.xaml 的交互逻辑
    /// </summary>
    public partial class HierarchicalDataTemplateDemo : UserControl
    {
        public HierarchicalDataTemplateDemo()
        {
            InitializeComponent();

            List<Person> list = new  List<Person>();
            list.Add(new Person() { Name = "test1" });

            var person = new Person();
            person.Name = "test2";
            list[0].Childs.Add(person);
            person.Childs.Add(new Person() { Name = "test3" });


            treeView.ItemsSource = list;


        }
    }

    public class Person
    {
        public string Name { get; set; }

        public List<Person> Childs { get; set; } = new List<Person>();
    }
}
