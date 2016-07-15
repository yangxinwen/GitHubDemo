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

namespace WPFDemo.PathDraw
{
    /// <summary>
    /// RunLine.xaml 的交互逻辑
    /// </summary>
    public partial class RunLine : UserControl
    {
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

        /// <summary>
        /// 设置或获取箭头控件背景颜色
        /// </summary>
        public new Brush Background
        {
            get
            {
                return border.Background;
            }
            set
            {
                startPath.Fill = value;
                endPath.Fill = value;
                border.Background = value;
            }
        }
        /// <summary>
        /// 是否显示起始箭头
        /// </summary>
        public bool IsShowStartArrow
        {
            get
            {
                if (startPath.Visibility == Visibility.Visible)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    startPath.Visibility = Visibility.Visible;
                else
                {
                    startPath.Visibility = Visibility.Collapsed;
                }
                AutoSetBorderMargin();
            }
        }
        /// <summary>
        /// 是否显示结束箭头
        /// </summary>
        public bool IsShowEndArrow
        {
            get
            {
                if (endPath.Visibility == Visibility.Visible)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    endPath.Visibility = Visibility.Visible;
                else
                    endPath.Visibility = Visibility.Collapsed;
                AutoSetBorderMargin();
            }
        }

        private Point _startPoint;
        /// <summary>
        /// 箭头起始点
        /// </summary>
        public Point StartPoint
        {
            get
            {                
                return _startPoint;
            }
            set
            {
                _startPoint = value;
                _startPoint.Y=_startPoint.Y - this.Height / 2;
                this.SetValue(Canvas.LeftProperty, _startPoint.X);
                this.SetValue(Canvas.TopProperty, _startPoint.Y );
                EndPoint = _endPoint;
            }
        }

        private Point _endPoint;
        /// <summary>
        /// 箭头终点
        /// </summary>
        public Point EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                try
                {
                    _endPoint = value;
                    double a = Math.Abs(value.X - StartPoint.X);
                    double b = Math.Abs(value.Y - StartPoint.Y - this.Height / 2);
                    double c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));  //width=斜边c=√(a^2+b^2)
                    this.Width = c;
                    //得出两点间的角度  公式 angle = Math.Atan2((Y2 - Y1), (X2 - X2)) * 180 / Math.PI
                    double angle = Math.Atan2((value.Y- StartPoint.Y-this.Height/2), (value.X - StartPoint.X)) * 180 / Math.PI;   
                    //this.RenderTransformOrigin = new Point(0, 0);
                    var f = new RotateTransform(angle);
                    //设置中心点，否则选择会产生偏移
                    f.CenterX = 0;
                    f.CenterY = this.Height / 2;
                    this.RenderTransform = f;
                }
                catch (Exception)
                {

                }
            }
        }
        /// <summary>
        /// 绑定的model属性
        /// </summary>
        public RunLineModel Model { get; set; }

        public RunLine()
        {
            InitializeComponent();
            IsShowStartArrow = false;
        }

        private void uc_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            startPath.Height = this.ActualHeight * 1.5;
            endPath.Height = this.ActualHeight * 1;
            border.Height = this.ActualHeight / 4;
        }
        /// <summary>
        /// 自动设置border的margin，保持与箭头的无缝衔接
        /// </summary>
        private void AutoSetBorderMargin()
        {
            var margin = new Thickness();
            if (startPath.Visibility == Visibility.Visible)
                margin.Left = -5;
            if (endPath.Visibility == Visibility.Visible)
                margin.Right = -2;
            border.Margin = margin;
        }
    }
}
