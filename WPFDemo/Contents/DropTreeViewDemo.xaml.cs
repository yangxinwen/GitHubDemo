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
using WPFDemo.Model;

namespace WPFDemo.Contents
{
    /// <summary>
    /// DropTreeViewDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DropTreeViewDemo : UserControl
    {
        public DropTreeViewDemo()
        {
            InitializeComponent();
            InitData();
        }


        private void InitData()
        {
            var list = new System.Collections.ObjectModel.ObservableCollection<Person>();
            list.Add(new Person() { Name = "test1" });

            var person = new Person();
            person.Name = "test2";
            list[0].Childs.Add(person);
            person.Childs.Add(new Person() { Name = "test3" });
            person.Childs.Add(new Person() { Name = "test4" });
            person = new Person();
            person.Name = "test5";
            list[0].Childs.Add(person);


            treeView.ItemsSource = list;
        }
    }
}
