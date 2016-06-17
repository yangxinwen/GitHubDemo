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
    public partial class ContainerTruck : UserControl
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


        public bool IsHaveBox
        {
            get
            {
                if (containerBox.Visibility == Visibility.Visible)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    containerBox.Visibility = Visibility.Visible;
                else
                    containerBox.Visibility = Visibility.Collapsed;
            }
        }

        public ContainerTruck()
        {
            InitializeComponent();
        }
    }
}
