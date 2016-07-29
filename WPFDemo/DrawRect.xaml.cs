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
        private Point _startPoint;
        private Rectangle _rect = new Rectangle();
        /// <summary>
        /// 操作状态
        /// </summary>
        private OperateStatus _operateStatus;
        private Point _downPoint; //记录按下时的点的位置
        private Point _tmpStartPoint;
        private Point _tmpEndPoint;


        public DrawRect()
        {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _downPoint = e.GetPosition(canvasPanel);
            _startPoint = ucRect.StartPoint;
            if (ucRect.Cursor != Cursors.Arrow)
            {
                _tmpStartPoint = ucRect.StartPoint;
                _tmpEndPoint = ucRect.EndPoint;

                var point = e.GetPosition(ucRect);
                if (ucRect.Cursor == Cursors.SizeAll)
                {//移动
                    _operateStatus = OperateStatus.Move;
                }
                else if (ucRect.Cursor == Cursors.SizeNS)
                {//上下
                    if (point.Y < ucRect.ActualHeight / 2)
                        _operateStatus = OperateStatus.Top;
                    else
                        _operateStatus = OperateStatus.Bottom;
                }
                else if (ucRect.Cursor == Cursors.SizeWE)
                {//左右
                    if (point.X < ucRect.ActualWidth / 2)
                        _operateStatus = OperateStatus.Left;
                    else
                        _operateStatus = OperateStatus.Right;
                }
                else if (ucRect.Cursor == Cursors.SizeNWSE)
                {//左上 右下
                    if (point.X < ucRect.ActualWidth / 2)
                        _operateStatus = OperateStatus.LeftTop;
                    else
                        _operateStatus = OperateStatus.RightBottom;
                }
                else if (ucRect.Cursor == Cursors.SizeNESW)
                {//左下 右上
                    if (point.X < ucRect.ActualWidth / 2)
                        _operateStatus = OperateStatus.LeftBottom;
                    else
                        _operateStatus = OperateStatus.RightTop;
                }
            }
            else
                _operateStatus = OperateStatus.None;
            if (_isAdd)
            {

                canvasPanel.Children.Add(_rect);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isAdd = false;
        }




        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(canvasPanel);
            if (e.LeftButton == MouseButtonState.Released)
                return;




            if (_operateStatus != OperateStatus.None)
            {
                var width = point.X - _startPoint.X;
                var height = point.Y - _startPoint.Y;

                //是否反转
                bool isTurn = true;
                Point tmpPoint;


                switch (_operateStatus)
                {
                    case OperateStatus.None:
                        break;
                    case OperateStatus.Top:
                        {
                            if (point.Y < _tmpEndPoint.Y)
                                ucRect.StartPoint = new Point(ucRect.StartPoint.X, point.Y);
                            else
                                ucRect.EndPoint = new Point(ucRect.EndPoint.X, point.Y);
                        }
                        break;
                    case OperateStatus.Bottom:
                        {
                            if (point.Y > _tmpStartPoint.Y)
                                ucRect.EndPoint = new Point(ucRect.EndPoint.X, point.Y);
                            else
                                ucRect.StartPoint = new Point(ucRect.StartPoint.X, point.Y);
                        }
                        break;
                    case OperateStatus.Left:
                        {
                            if (point.X < _tmpEndPoint.X)
                                ucRect.StartPoint = new Point(point.X, ucRect.StartPoint.Y);
                            else
                                ucRect.EndPoint = new Point(point.X, ucRect.EndPoint.Y);
                        }
                        break;
                    case OperateStatus.Right:
                        {
                            if (point.X > _tmpStartPoint.X)
                            {
                                ucRect.EndPoint = new Point(point.X, ucRect.EndPoint.Y);

                                ucRect.StartPoint = new Point(_tmpEndPoint.X, _tmpStartPoint.Y);
                            }
                            else
                            {
                                ucRect.StartPoint = new Point(point.X, ucRect.StartPoint.Y);

                                ucRect.EndPoint = new Point(_tmpStartPoint.X, _tmpEndPoint.Y);
                            }
                        }
                        break;
                    case OperateStatus.LeftTop:
                        {
                            if (point.X < _tmpEndPoint.X && point.Y < _tmpEndPoint.Y)
                            {
                                ucRect.StartPoint = point;
                            }
                            else if (point.X > _tmpEndPoint.X && point.Y > _tmpEndPoint.Y)
                            {
                                ucRect.EndPoint = point;
                            }
                            else if (point.X > _tmpEndPoint.X)
                            {
                                ucRect.StartPoint = new Point(_tmpEndPoint.X,point.Y);
                                ucRect.EndPoint = new Point(point.X,_tmpEndPoint.Y);
                            }
                            else if (point.Y > _tmpEndPoint.Y)
                            {
                                ucRect.StartPoint = new Point(point.X, _tmpEndPoint.Y);
                                ucRect.EndPoint = new Point(_tmpEndPoint.X, point.Y);
                            }

                        }
                        break;
                    case OperateStatus.LeftBottom:
                        {
                            if (point.X < _tmpEndPoint.X && point.Y > _tmpStartPoint.Y)
                            {
                                ucRect.StartPoint = new Point(point.X,_tmpStartPoint.Y);
                                ucRect.EndPoint = new Point(_tmpEndPoint.X,point.Y);
                            }
                            else if (point.X > _tmpEndPoint.X && point.Y < _tmpStartPoint.Y)
                            {
                                ucRect.StartPoint = new Point(_tmpEndPoint.X, point.Y);
                                ucRect.EndPoint = new Point(point.X, _tmpStartPoint.Y);
                            }
                            else if (point.X > _tmpEndPoint.X)
                            {
                                ucRect.StartPoint = new Point(_tmpEndPoint.X, _tmpStartPoint.Y);
                                ucRect.EndPoint = point;
                            }
                            else if (point.Y < _tmpStartPoint.Y)
                            {
                                ucRect.StartPoint = point;
                                ucRect.EndPoint = new Point(_tmpEndPoint.X, _tmpStartPoint.Y);
                            }
                        }
                        break;
                    case OperateStatus.RightTop:
                        {
                            if (point.X > _tmpStartPoint.X && point.Y < _tmpEndPoint.Y)
                            {
                                ucRect.StartPoint = new Point(_tmpStartPoint.X, point.Y);
                                ucRect.EndPoint = new Point(point.X, _tmpEndPoint.Y);
                            }
                            else if (point.X < _tmpStartPoint.X && point.Y > _tmpEndPoint.Y)
                            {
                                ucRect.StartPoint = new Point(point.X, _tmpEndPoint.Y);
                                ucRect.EndPoint = new Point(_tmpStartPoint.X, point.Y);
                            }
                            else if (point.X < _tmpStartPoint.X)
                            {
                                ucRect.StartPoint = point;
                                ucRect.EndPoint = new Point(_tmpStartPoint.X, _tmpEndPoint.Y);
                            }
                            else if (point.Y > _tmpEndPoint.Y)
                            {
                                ucRect.StartPoint = new Point(_tmpStartPoint.X, _tmpEndPoint.Y);
                                ucRect.EndPoint = point;
                            }
                        }
                        break;
                    case OperateStatus.RightBottom:
                        {
                            if (point.X > _tmpStartPoint.X && point.Y > _tmpStartPoint.Y)
                            {
                                ucRect.EndPoint = point;
                            }
                            else if (point.X < _tmpStartPoint.X && point.Y < _tmpStartPoint.Y)
                            {
                                ucRect.StartPoint = point;
                            }
                            else if (point.X < _tmpStartPoint.X)
                            {
                                ucRect.StartPoint = new Point(point.X, _tmpStartPoint.Y);
                                ucRect.EndPoint = new Point(_tmpStartPoint.X, point.Y);
                            }
                            else if (point.Y < _tmpStartPoint.Y)
                            {
                                ucRect.StartPoint = new Point(_tmpStartPoint.X, point.Y);
                                ucRect.EndPoint = new Point(point.X, _tmpStartPoint.Y);
                            }

                        }
                        break;
                    case OperateStatus.Move:
                        {
                            var offsetX = point.X - _downPoint.X;
                            var offsetY = point.Y - _downPoint.Y;
                            ucRect.StartPoint = new Point(_tmpStartPoint.X + offsetX, _tmpStartPoint.Y + offsetY);
                            ucRect.EndPoint = new Point(_tmpEndPoint.X + offsetX, _tmpEndPoint.Y + offsetY);
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _isAdd = true;
        }

        private void SetCursor(Point point)
        {

        }

        private void ucRect_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }


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
