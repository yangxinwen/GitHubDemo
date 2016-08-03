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
    /// thisBorder.xaml 的交互逻辑
    /// </summary>
    public partial class UCRectBorder : UserControl
    {
        private Point _startPoint;
        /// <summary>
        /// 获取或设置控件的左上角坐标
        /// </summary>
        public Point StartPoint
        {
            set
            {
                _startPoint = value;
                this.SetValue(Canvas.LeftProperty, value.X);
                this.SetValue(Canvas.TopProperty, value.Y);
                EndPoint = _endPoint;
            }
            get
            {
                return _startPoint;
            }
        }
        private Point _endPoint;
        /// <summary>
        /// 获取或设置控件的右下角坐标
        /// </summary>
        public Point EndPoint
        {
            set
            {
                _endPoint = value;
                var width = value.X - StartPoint.X;
                if (width > 0)
                    this.Width = width;


                var height = value.Y - StartPoint.Y;
                if (height > 0)
                    this.Height = height;
            }
            get
            {
                return _endPoint;
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                var visibility = value ? Visibility.Visible : Visibility.Collapsed;
                border1.Visibility = border2.Visibility =
                    border3.Visibility = border4.Visibility =
                    border5.Visibility = border6.Visibility =
                    border7.Visibility = border8.Visibility = visibility;

            }
        }


        public UCRectBorder()
        {
            InitializeComponent();
            IsSelected = false;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsSelected)
            {
                this.Cursor = Cursors.Arrow;
                return;
            }

            //编辑模式时设置鼠标状态
            var point = e.GetPosition(this);
            var range = 5; //范围点距离
            if (point.X < range && point.Y < range ||
                point.X + range > this.ActualWidth && point.Y + range > this.ActualHeight)
                this.Cursor = Cursors.SizeNWSE; //左上 右下
            else if (point.X < range && point.Y + range > this.ActualHeight ||
                 point.X + range > this.ActualWidth && point.Y < range)
                this.Cursor = Cursors.SizeNESW; //左下 右上
            else if (point.X > range && point.X < this.ActualWidth - range &&
                (point.Y < range || point.Y > this.ActualHeight - range))
                this.Cursor = Cursors.SizeNS; //上下
            else if (point.X < range || point.X > this.ActualWidth - range)
                this.Cursor = Cursors.SizeWE; //左右
            else
                this.Cursor = Cursors.SizeAll;
        }

        private void AutoConvertPoint()
        {
            //if (EndPoint.X > StartPoint.X && EndPoint.Y > StartPoint.Y)
            //{

            //}
            //else if (EndPoint.X < StartPoint.X && EndPoint.Y < StartPoint.Y)
            //{
            //    var tmp = EndPoint;
            //    EndPoint = StartPoint;
            //    StartPoint = tmp;
            //}
        }
    }
}
