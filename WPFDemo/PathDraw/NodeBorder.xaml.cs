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
using WPFDemo.PathDraw;

namespace WPFDemo
{
    /// <summary>
    /// NodeBorder.xaml 的交互逻辑
    /// </summary>
    public partial class NodeBorder : UserControl
    {
        /// <summary>
        /// 获取圆半径长度
        /// </summary>
        public double CircleR
        {
            get
            {
                return this.Width / 2;
            }
        }
        /// <summary>
        /// 获取圆心位置
        /// </summary>
        public Point CircleCenterPoint
        {
            get
            {
                var point = new Point();
                point.X = X + this.Width / 2;
                point.Y = Y + this.Height - border.Height / 2;
                return point;
            }
        }

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

        private bool isSelected = false;
        /// <summary>
        /// 是否已选择
        /// </summary>

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    if (value)
                        border.BorderBrush = this.FindResource("selectedBorderBrush") as LinearGradientBrush;
                    else
                        border.BorderBrush = null;

                }
            }
        }

        private NodeModel model;
        public NodeModel Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                if (model != null)
                    tbName.Text = model.Name;
            }
        }

        public NodeBorder()
        {
            InitializeComponent();
        }
    }
}
