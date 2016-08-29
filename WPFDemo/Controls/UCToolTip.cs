using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFDemo.Controls
{
    /// <summary>
    /// 气泡提示框
    /// </summary>
    public class UCToolTip : ToolTip
    {
        #region Property
        /// <summary>
        ///  设置显示方向,请不要手动设置该参数，若需要设置弹出面板的显示位置请使用 PlacementMode，支持方向和鼠标
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Dock), typeof(UCToolTip), new FrameworkPropertyMetadata(Dock.Top));
        /// <summary>
        /// 获取或设置显示方向
        /// </summary>
        public Dock Direction
        {
            get
            {
                return (Dock)GetValue(DirectionProperty);
            }
            set
            {
                SetValue(DirectionProperty, value);
            }
        }
        #endregion

        #region 获取鼠标在屏幕中的位置

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        #endregion

        #region Constructors

        static UCToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UCToolTip), new FrameworkPropertyMetadata(typeof(UCToolTip)));
        }

        public UCToolTip()
        {
            this.Opened += UCToolTip_Opened;
        }

        #endregion

        #region EventHandles
        private void UCToolTip_Opened(object sender, RoutedEventArgs e)
        {
            switch (Placement)
            {
                case PlacementMode.Bottom:
                case PlacementMode.Right:
                case PlacementMode.Left:
                case PlacementMode.Top:
                    DealDirectionPosion();
                    break;
                case PlacementMode.MousePoint:
                case PlacementMode.Mouse:
                    DealMousePosion();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 处理方向定置
        /// </summary>
        private void DealDirectionPosion()
        {
            var popPoint = PointToScreen(new Point(0, 0)); //弹出面板屏幕坐标位置
            var targetPoint = PlacementTarget.PointToScreen(new Point(0, 0)); //目标控件屏幕坐标位置

            if (Placement == PlacementMode.Top || Placement == PlacementMode.Bottom)
            {
                this.HorizontalOffset = 0 - (this.RenderSize.Width - this.PlacementTarget.RenderSize.Width) / 2; //水平居中

                if (this.Placement == PlacementMode.Top)
                {
                    if (popPoint.Y < targetPoint.Y)
                        this.Direction = Dock.Bottom;
                    else
                        this.Direction = Dock.Top;

                }
                else if (this.Placement == PlacementMode.Bottom)
                {
                    if (popPoint.Y > targetPoint.Y)
                        this.Direction = Dock.Top;
                    else
                        this.Direction = Dock.Bottom;
                }
            }
            else if (Placement == PlacementMode.Left || Placement == PlacementMode.Right)
            {
                this.VerticalOffset = 0 - (this.RenderSize.Height - this.PlacementTarget.RenderSize.Height) / 2; //垂直居中
                if (this.Placement == PlacementMode.Left)
                {
                    if (popPoint.X > targetPoint.X)
                        this.Direction = Dock.Left;
                    else
                        this.Direction = Dock.Right;
                }
                if (this.Placement == PlacementMode.Right)
                {
                    if (popPoint.X < targetPoint.X)
                        this.Direction = Dock.Right;
                    else
                        this.Direction = Dock.Left;
                }
            }
        }
        /// <summary>
        /// 处理鼠标定位
        /// </summary>
        private void DealMousePosion()
        {
            var popPoint = PointToScreen(new Point(0, 0)); //弹出面板屏幕坐标位置
            var point = new POINT();
            GetCursorPos(out point);            //得到鼠标相对于屏幕的坐标位置
            var targetPoint = new Point(point.X, point.Y);

            this.HorizontalOffset = 0 - this.PlacementTarget.RenderSize.Width; //水平居中

            if (popPoint.Y > targetPoint.Y)
                this.Direction = Dock.Top;
            else
                this.Direction = Dock.Bottom;

        }
        #endregion
    }
}
