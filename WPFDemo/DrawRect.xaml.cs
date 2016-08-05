using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// DrawRect.xaml 的交互逻辑
    /// </summary>
    public partial class DrawRect : UserControl
    {
        private bool _isAdd = false;
        /// <summary>
        /// 操作状态
        /// </summary>
        private OperateStatus _operateStatus;
        private Point _downPoint;
        //记录按下时的点的位置
        //private Point _tmpStartPoint;
        //private Point _tmpEndPoint;
        /// <summary>
        /// 保存编辑之前的矩形数据
        /// </summary>
        private Rect _tmpRect;

        private UCRectBorder _selectedRect;
        /// <summary>
        /// 当前选择的矩形
        /// </summary>
        private UCRectBorder SelectedRect
        {
            get
            {
                return _selectedRect;
            }
            set
            {
                if (_selectedRect != null)
                    _selectedRect.IsSelected = false;

                _selectedRect = value;
                //使用时设置，防止新建时出现编辑框不好看
                //_selectedRect.IsSelected = true;
            }
        }

        public DrawRect()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 判断界面上的一个点是否在矩形内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="uc"></param>
        /// <returns></returns>
        private bool IsPointInRect(Point point, UCRectBorder uc)
        {
            if (point.X >= uc.StartPoint.X &&
                point.X <= uc.EndPoint.X &&
                point.Y >= uc.StartPoint.Y &&
                point.Y <= uc.EndPoint.Y
                )
                return true;
            else
                return false;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var sPoint = e.GetPosition(canvasPanel);
            if (_isAdd)
            { //添加矩形
                var uc = new UCRectBorder();
                //默认长宽为20的矩形，否则用户按下鼠标左键后看不到矩形
                uc.StartPoint = new Point(sPoint.X - 20, sPoint.Y - 20);
                uc.EndPoint = new Point(sPoint.X, sPoint.Y);
                uc.MouseDoubleClick += (o, s) =>
                {
                    //若已选择则取消选择
                    if (SelectedRect == uc && SelectedRect.IsSelected == true)
                        SelectedRect.IsSelected = false;
                    else
                    {
                        SelectedRect = uc;
                        SelectedRect.IsSelected = true;
                    }
                };
                canvasPanel.Children.Add(uc);
                SelectedRect = uc;
                //刚添加时默认移动矩形右下角
                _operateStatus = OperateStatus.RightBottom;
                _tmpRect = new Rect(SelectedRect.StartPoint, SelectedRect.EndPoint);
            }
            else if (SelectedRect != null && IsPointInRect(sPoint, SelectedRect))
            {//编辑矩形
                _downPoint = sPoint;

                if (SelectedRect.Cursor != Cursors.Arrow)
                {
                    _tmpRect = new Rect(SelectedRect.StartPoint, SelectedRect.EndPoint);

                    var point = e.GetPosition(SelectedRect);
                    if (SelectedRect.Cursor == Cursors.SizeAll)
                    {//移动
                        _operateStatus = OperateStatus.Move;
                    }
                    else if (SelectedRect.Cursor == Cursors.SizeNS)
                    {//上下
                        if (point.Y < SelectedRect.ActualHeight / 2)
                            _operateStatus = OperateStatus.Top;
                        else
                            _operateStatus = OperateStatus.Bottom;
                    }
                    else if (SelectedRect.Cursor == Cursors.SizeWE)
                    {//左右
                        if (point.X < SelectedRect.ActualWidth / 2)
                            _operateStatus = OperateStatus.Left;
                        else
                            _operateStatus = OperateStatus.Right;
                    }
                    else if (SelectedRect.Cursor == Cursors.SizeNWSE)
                    {//左上 右下
                        if (point.X < SelectedRect.ActualWidth / 2)
                            _operateStatus = OperateStatus.LeftTop;
                        else
                            _operateStatus = OperateStatus.RightBottom;
                    }
                    else if (SelectedRect.Cursor == Cursors.SizeNESW)
                    {//左下 右上
                        if (point.X < SelectedRect.ActualWidth / 2)
                            _operateStatus = OperateStatus.LeftBottom;
                        else
                            _operateStatus = OperateStatus.RightTop;
                    }
                }
                else
                {
                    _operateStatus = OperateStatus.None;

                }
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isAdd = false;
            _operateStatus = OperateStatus.None;
        }


        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(canvasPanel);
            if (e.LeftButton == MouseButtonState.Released)
                return;

            var startPoint = _tmpRect.TopLeft;
            var endPoint = _tmpRect.BottomRight;
            var ucRect = SelectedRect;
            if (_operateStatus != OperateStatus.None && ucRect != null)
            {
                switch (_operateStatus)
                {
                    case OperateStatus.None:
                        break;
                    case OperateStatus.Top:
                        {
                            if (point.Y < endPoint.Y)
                            {
                                ucRect.StartPoint = new Point(ucRect.StartPoint.X, point.Y);
                                ucRect.EndPoint = _tmpRect.BottomRight;
                            }
                            else
                            {
                                ucRect.EndPoint = new Point(ucRect.EndPoint.X, point.Y);
                                ucRect.StartPoint = _tmpRect.BottomLeft;
                            }
                        }
                        break;
                    case OperateStatus.Bottom:
                        {
                            if (point.Y > startPoint.Y)
                            {
                                ucRect.EndPoint = new Point(ucRect.EndPoint.X, point.Y);
                                ucRect.StartPoint = _tmpRect.TopLeft;
                            }
                            else
                            {
                                ucRect.StartPoint = new Point(ucRect.StartPoint.X, point.Y);
                                ucRect.EndPoint = _tmpRect.TopRight;
                            }
                        }
                        break;
                    case OperateStatus.Left:
                        {
                            if (point.X < endPoint.X)
                            {
                                ucRect.StartPoint = new Point(point.X, ucRect.StartPoint.Y);
                                ucRect.EndPoint = _tmpRect.BottomRight;
                            }
                            else
                            {
                                ucRect.EndPoint = new Point(point.X, ucRect.EndPoint.Y);
                                ucRect.StartPoint = _tmpRect.TopRight;
                            }
                        }
                        break;
                    case OperateStatus.Right:
                        {
                            if (point.X > startPoint.X)
                            {
                                ucRect.EndPoint = new Point(point.X, ucRect.EndPoint.Y);

                                ucRect.StartPoint = _tmpRect.TopLeft;
                            }
                            else
                            {
                                ucRect.StartPoint = new Point(point.X, ucRect.StartPoint.Y);

                                ucRect.EndPoint = _tmpRect.BottomLeft;
                            }
                        }
                        break;
                    case OperateStatus.LeftTop:
                        {
                            if (point.X < endPoint.X && point.Y < endPoint.Y)
                            {
                                ucRect.StartPoint = point;
                                ucRect.EndPoint = _tmpRect.BottomRight;
                            }
                            else if (point.X > endPoint.X && point.Y > endPoint.Y)
                            {
                                ucRect.EndPoint = point;
                                ucRect.StartPoint = _tmpRect.BottomRight;
                            }
                            else if (point.X > endPoint.X)
                            {
                                ucRect.StartPoint = new Point(endPoint.X, point.Y);
                                ucRect.EndPoint = new Point(point.X, endPoint.Y);
                            }
                            else if (point.Y > endPoint.Y)
                            {
                                ucRect.StartPoint = new Point(point.X, endPoint.Y);
                                ucRect.EndPoint = new Point(endPoint.X, point.Y);
                            }

                        }
                        break;
                    case OperateStatus.LeftBottom:
                        {
                            if (point.X < endPoint.X && point.Y > startPoint.Y)
                            {
                                ucRect.StartPoint = new Point(point.X, startPoint.Y);
                                ucRect.EndPoint = new Point(endPoint.X, point.Y);
                            }
                            else if (point.X > endPoint.X && point.Y < startPoint.Y)
                            {
                                ucRect.StartPoint = new Point(endPoint.X, point.Y);
                                ucRect.EndPoint = new Point(point.X, startPoint.Y);
                            }
                            else if (point.X > endPoint.X)
                            {
                                ucRect.StartPoint = new Point(endPoint.X, startPoint.Y);
                                ucRect.EndPoint = point;
                            }
                            else if (point.Y < startPoint.Y)
                            {
                                ucRect.StartPoint = point;
                                ucRect.EndPoint = new Point(endPoint.X, startPoint.Y);
                            }
                        }
                        break;
                    case OperateStatus.RightTop:
                        {
                            if (point.X > startPoint.X && point.Y < endPoint.Y)
                            {
                                ucRect.StartPoint = new Point(startPoint.X, point.Y);
                                ucRect.EndPoint = new Point(point.X, endPoint.Y);
                            }
                            else if (point.X < startPoint.X && point.Y > endPoint.Y)
                            {
                                ucRect.StartPoint = new Point(point.X, endPoint.Y);
                                ucRect.EndPoint = new Point(startPoint.X, point.Y);
                            }
                            else if (point.X < startPoint.X)
                            {
                                ucRect.StartPoint = point;
                                ucRect.EndPoint = new Point(startPoint.X, endPoint.Y);
                            }
                            else if (point.Y > endPoint.Y)
                            {
                                ucRect.StartPoint = new Point(startPoint.X, endPoint.Y);
                                ucRect.EndPoint = point;
                            }
                        }
                        break;
                    case OperateStatus.RightBottom:
                        {
                            if (point.X > startPoint.X && point.Y > startPoint.Y)
                            {
                                ucRect.EndPoint = point;
                                ucRect.StartPoint = _tmpRect.TopLeft;
                            }
                            else if (point.X < startPoint.X && point.Y < startPoint.Y)
                            {
                                ucRect.StartPoint = point;
                                ucRect.EndPoint = _tmpRect.TopLeft;
                            }
                            else if (point.X < startPoint.X)
                            {
                                ucRect.StartPoint = new Point(point.X, startPoint.Y);
                                ucRect.EndPoint = new Point(startPoint.X, point.Y);
                            }
                            else if (point.Y < startPoint.Y)
                            {
                                ucRect.StartPoint = new Point(startPoint.X, point.Y);
                                ucRect.EndPoint = new Point(point.X, startPoint.Y);
                            }

                        }
                        break;
                    case OperateStatus.Move:
                        {
                            var offsetX = point.X - _downPoint.X;
                            var offsetY = point.Y - _downPoint.Y;
                            ucRect.StartPoint = new Point(startPoint.X + offsetX, startPoint.Y + offsetY);
                            ucRect.EndPoint = new Point(endPoint.X + offsetX, endPoint.Y + offsetY);
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //右击取消新建操作
            _isAdd = true;
        }

        private void SetCursor(Point point)
        {

        }

        private void ucRect_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void canvasPanel_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isAdd = false;
        }
    }

    /// <summary>
    /// 矩形编辑时的操作状态
    /// </summary>
    public enum OperateStatus
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom,
        Move
    }
}
