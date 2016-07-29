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

namespace WPFDemo
{
    /// <summary>
    /// MulCombox.xaml 的交互逻辑
    /// </summary>
    public partial class MulCombox : UserControl
    {
        public MulCombox()
        {
            InitializeComponent();



            //todo:测试数据，需从ecs获取
            var sizeList = new List<TSize>();
            sizeList.Add(new TSize() { SizeId = 1, Name = "1号尺寸" });
            sizeList.Add(new TSize() { SizeId = 2, Name = "2号尺寸" });
            sizeList.Add(new TSize() { SizeId = 3, Name = "3号尺寸" });
            cbxSize.ItemsSource = sizeList;
        }
    }

    public partial class TSize
    {
        public int SizeId { get; set; }
        public string Name { get; set; }
    }
}
