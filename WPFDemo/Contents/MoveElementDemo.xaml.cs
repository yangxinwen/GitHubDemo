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

namespace WPFDemo.Contents
{
    /// <summary>
    /// MoveElementDemo.xaml 的交互逻辑
    /// </summary>
    public partial class MoveElementDemo : UserControl
    {
        private FrameworkElement _ele = null;
        public MoveElementDemo()
        {
            InitializeComponent();

            thumb.DragStarted += Thumb_DragStarted;
            thumb.DragDelta += Thumb_DragDelta;
            
            _ele = VisualTreeHelper.GetParent(thumb) as  Grid;
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            _ele.SetValue(Canvas.LeftProperty, (double)_ele.GetValue(Canvas.LeftProperty) + e.HorizontalChange);
            _ele.SetValue(Canvas.TopProperty, (double)_ele.GetValue(Canvas.TopProperty) + e.VerticalChange);
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
           
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
