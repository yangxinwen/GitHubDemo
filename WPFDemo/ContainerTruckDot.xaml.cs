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
    /// ContainerTruck.xaml 的交互逻辑
    /// </summary>
    public partial class ContainerTruckDot : UserControl
    {
        public double X
        {
            get
            {
                return (double)this.GetValue(Canvas.LeftProperty);
            }
            set
            {
                SetValue(Canvas.LeftProperty, value);
            }

        }
        public double Y
        {
            get
            {
                return (double)this.GetValue(Canvas.TopProperty);
            }
            set
            {
                SetValue(Canvas.TopProperty, value);
            }
        } 


        public ContainerTruckDot()
        {
            InitializeComponent();
        }
    }
}
