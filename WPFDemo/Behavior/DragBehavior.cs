using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace WPFDemo.Behavior
{
    public class DragBehavior : Behavior<UIElement>
    {
        private Canvas _canvas = null;
        private bool _isDown = false;
        protected override void OnAttached()
        {
            base.OnAttached();

            _canvas = VisualTreeHelper.GetParent(this.AssociatedObject) as Canvas;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
            _canvas.PreviewMouseMove += AssociatedObject_MouseMove;
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AssociatedObject.ReleaseMouseCapture();
            _isDown = false;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AssociatedObject.CaptureMouse();
            _isDown = true;
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isDown)
            {
                var point = e.GetPosition(_canvas);
                AssociatedObject.SetValue(Canvas.LeftProperty, point.X);
                AssociatedObject.SetValue(Canvas.TopProperty, point.Y);
            }
        } 
    }
}
