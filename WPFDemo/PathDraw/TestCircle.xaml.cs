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

namespace WPFDemo.PathDraw
{
    /// <summary>
    /// TestCircle.xaml 的交互逻辑
    /// </summary>
    public partial class TestCircle : UserControl
    {
        public TestCircle()
        {
            InitializeComponent();



            var centerPoint = new Point(20, 20);
            var e = new Ellipse();
            e.Width = 100;
            e.Height = 100;
            e.SetValue(Canvas.LeftProperty, centerPoint.X-e.Width/2);
            e.SetValue(Canvas.TopProperty, centerPoint.Y - e.Height / 2);
            e.Stroke = Brushes.Black;
            e.StrokeThickness = 1;
            canvas.Children.Add(e);

            var point = new Point(150, 150);

            //double angle = Math.Atan2((point.Y - 50), (point.X - 50));// * 180 / Math.PI;
            //Debug.WriteLine(angle);
            //var x = 50 + 50 * Math.Cos(angle);
            //var y = 50 + 50 * Math.Sin(angle);


            //var line = new Line();
            //line.X1 = 50;
            //line.Y1 = 50;
            //line.X2 = x;
            //line.Y2 = y;
            //line.Stroke = Brushes.Red;
            //canvas.Children.Add(line);

            var line = new Line();
            line.X1 = centerPoint.X;
            line.Y1 = centerPoint.Y;
            line.X2 = point.X;
            line.Y2 = point.Y;
            line.Stroke = Brushes.Blue;
            canvas.Children.Add(line);


            var angle = -137.23117460803127;
            var border = new Border();
            border.SetValue(Canvas.LeftProperty, centerPoint.X);
            border.SetValue(Canvas.TopProperty, centerPoint.Y-5);
            border.Height = 10;
            border.Width = 100;
            border.Background = Brushes.Blue;
            border.Opacity = 0.2;
            var f = new RotateTransform(angle);
            f.CenterX = 0;//centerPoint.X;
            f.CenterY = 5;//centerPoint.Y;
            border.RenderTransform = f;
            canvas.Children.Add(border);

            //border.SetValue(Canvas.LeftProperty, centerPoint.X - 2.5);
            //border.SetValue(Canvas.TopProperty, centerPoint.Y + 5);

            

            var runLine = new UCLine();
            runLine.Height = 15;
            runLine.StartPoint = new Point(centerPoint.X, centerPoint.Y-runLine.Height/2);
            runLine.EndPoint = point;
            runLine.Cursor = Cursors.Hand;
            canvas.Children.Add(runLine);



        }
    }
}
