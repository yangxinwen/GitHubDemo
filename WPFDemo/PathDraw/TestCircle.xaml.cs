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



            var e = new Ellipse();
            e.Width = 100;
            e.Height = 100;
            e.Stroke = Brushes.Black;
            e.StrokeThickness = 1;
            canvas.Children.Add(e);

            var point = new Point(5,5);

            double angle = Math.Atan2((point.Y - 50), (point.X - 50));// * 180 / Math.PI;
            Debug.WriteLine(angle);
            var x = 50 + 50 * Math.Cos(angle);
            var y = 50 + 50 * Math.Sin(angle);


            var line = new Line();
            line.X1 = 50;
            line.Y1 = 50;
            line.X2 = x;
            line.Y2 = y;
            line.Stroke = Brushes.Red;
            canvas.Children.Add(line);

            line = new Line();
            line.X1 = 50;
            line.Y1 = 50;
            line.X2 = point.X;
            line.Y2 = point.Y;
            line.Stroke = Brushes.Blue;
            canvas.Children.Add(line);



        }
    }
}
