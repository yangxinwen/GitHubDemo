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
    /// ContainerBox.xaml 的交互逻辑
    /// </summary>
    public partial class ContainerBox : UserControl
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

        private bool flag = false;
        /// <summary>
        /// 是否是标记，标记的箱属于旁边的标记(1 2 3..n)
        /// </summary>
        public bool Flag
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;
                if (value)
                {
                    border1.Background = Brushes.Transparent;
                }
            }
        }

        public string Text
        {
            get
            {
                return txt.Text;
            }
            set
            {
                txt.Text = value;
            }
        }

        private int boxStatus = 1; //1-有箱  2-空箱  3-不可用

        /// <summary>
        /// 获取或设置箱状态
        /// </summary>
        public int BoxStatus
        {
            get
            {
                return boxStatus;
            }
            set
            {
                if (value < 1 || value > 3)
                    return;
                boxStatus = value;
                border1.Visibility = Visibility.Collapsed;
                border2.Visibility = Visibility.Collapsed;
                border3.Visibility = Visibility.Collapsed;
                switch (value)
                {
                    case 1:
                        border1.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        border2.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        border3.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }

            }
        }



        public ContainerBox()
        {
            InitializeComponent();


        }

        /// <summary>
        /// 拷贝一个新对象
        /// </summary>
        /// <returns></returns>
        public ContainerBox Clone()
        {
            var newBox = new ContainerBox();
            newBox.X = this.X;
            newBox.Y = this.Y;
            newBox.Width = this.Width;
            newBox.Height = this.Height;
            newBox.BorderThickness = this.BorderThickness;
            newBox.BorderBrush = this.BorderBrush;
            newBox.BoxStatus = this.BoxStatus;
            return newBox;
        }
    }
}
