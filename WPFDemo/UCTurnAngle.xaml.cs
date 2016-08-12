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
    /// UCTurnAngle.xaml 的交互逻辑
    /// </summary>
    public partial class UCTurnAngle : UserControl
    {
        public UCTurnAngle()
        {
            InitializeComponent();


            MakeLine();



        }

        Line line = new Line();
        Line line1 = new Line();
        private void MakeLine()
        {
            Random random = new Random((int)DateTime.Now.Ticks);


            //var point1 = new Point(100, 100);
            //var point2 = new Point(200, 100);
            //var point3 = new Point(300, 100);


            var point1 = new Point(random.Next(400), random.Next(400));
            var point2 = new Point(random.Next(400), random.Next(400));
            var point3 = new Point(random.Next(400), random.Next(400));






            var startPoint = point1;
            var endPoint = point2;

            var startPoint1 = point2;
            var endPoint1 = point3;



            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;
            line.X2 = endPoint.X;
            line.Y2 = endPoint.Y;
            line.Stroke = Brushes.Red;
            if (!canvasPanel.Children.Contains(line))
                canvasPanel.Children.Add(line);

            line1.X1 = startPoint1.X;
            line1.Y1 = startPoint1.Y;
            line1.X2 = endPoint1.X;
            line1.Y2 = endPoint1.Y;
            line1.Stroke = Brushes.Red;
            if (!canvasPanel.Children.Contains(line1))
                canvasPanel.Children.Add(line1);

            byte dir;

            var result = MakeTurnParam(point1, point2, point3, ellipse.Width / 2, out startPoint, out endPoint1,out dir);
            if (result == false)
                Debug.WriteLine(DateTime.Now.Ticks + ":false ");

            //var angle = CalcAngle(startPoint, endPoint, endPoint, endPoint1);
        }

        /// <summary>
        /// 得到两点间的角度
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private double GetAngle(Point startPoint, Point endPoint, Point endPoint1)
        {
            ////得出两点间的角度  公式 angle = Math.Atan2((Y2 - Y1), (X2 - X2)) * 180 / Math.PI
            //double angle = Math.Atan2((endPoint.Y - startPoint.Y), (endPoint.X - startPoint.X)) * 180 / Math.PI;
            //return angle;




            var a = (startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X);
            var b = (endPoint.Y - endPoint1.Y) / (endPoint.X - endPoint1.X);
            var k = Math.Abs((a - b) / (1 + a * b));
            return Math.Atan(k) / (Math.PI / 180);

        }


        /// <summary>
        /// 根据余弦定理求两个线段夹角
        /// </summary>
        /// <param name="s">start点</param>
        /// <param name="o">端点</param>
        /// <param name="e">end点</param>
        /// <returns></returns>
        private double CalcAngle(Point s, Point o, Point e)
        {
            double cosfi = 0, fi = 0, norm = 0;
            double dsx = s.X - o.X;
            double dsy = s.Y - o.Y;
            double dex = e.X - o.X;
            double dey = e.Y - o.Y;

            cosfi = dsx * dex + dsy * dey;
            norm = (dsx * dsx + dsy * dsy) * (dex * dex + dey * dey);
            cosfi /= Math.Sqrt(norm);

            if (cosfi >= 1.0) return 0;
            if (cosfi <= -1.0) return Math.PI;
            fi = Math.Acos(cosfi);

            if (180 * fi / Math.PI < 180)
            {
                return 180 * fi / Math.PI;
            }
            else
            {
                return 360 - 180 * fi / Math.PI;
            }
        }

        /// <summary>
        /// 根据两条相交直线生成开始和结束拐弯点
        /// 原理:一个夹角必有一个指定半径的圆与两条线段相切
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="oPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="radius"></param>
        /// <param name="startTurnP"></param>
        /// <param name="endTrunP"></param>
        /// <param name="dir"></param>
        /// <returns>若为false则表示给定的圆太大、夹角太小、线段太短</returns>
        private bool MakeTurnParam(Point startPoint, Point oPoint, Point endPoint, double radius,
            out Point startTurnP, out Point endTrunP, out byte dir)
        {
            startTurnP = new Point();
            endTrunP = new Point();
            dir = 0;
            //得到两条直线的夹角
            var angle = CalcAngle(startPoint, oPoint, endPoint);

            //经观测角度为pi时，两条线为直线
            if (angle == Math.PI)
            {
                dir = 0;
                return true;
            }

            //线段夹角与圆相切点的长度
            var len = radius / Math.Tan(angle / 2 * Math.PI / 180);

            //得到第一条直线的长度
            var line1Len = CalcLineLen(startPoint, oPoint);

            var tmpX = oPoint.X + (startPoint.X - oPoint.X) / line1Len * len;
            var tmpY = oPoint.Y + (startPoint.Y - oPoint.Y) / line1Len * len;
            //得到第一条直线与圆的相切坐标
            var point1 = new Point(tmpX, tmpY);
            startTurnP = point1;

            //得到第二条直线的长度
            var line2Len = CalcLineLen(oPoint, endPoint);
            tmpX = oPoint.X + (endPoint.X - oPoint.X) / line2Len * len;
            tmpY = oPoint.Y + (endPoint.Y - oPoint.Y) / line2Len * len;
            //得到第二条直线与圆的相切坐标
            var point2 = new Point(tmpX, tmpY);
            endTrunP = point2;

            //线段夹角与圆相切点的长度大于其中一条边，则该圆不与这两条直线相切
            if (len > line1Len || len > line2Len)
                return false;


            //得到夹角与圆相切组成的三角形夹角对边的中间点
            var cPoint = new Point();
            cPoint.X = point1.X + (point2.X - point1.X) / 2;
            cPoint.Y = point1.Y + (point2.Y - point1.Y) / 2;


            //todo:可使用以下测试算法结果是否正确
            //得到圆心到两条线段相交点的距离
            var circleLen = Math.Sqrt(Math.Pow(len, 2) + Math.Pow(radius, 2));
            //得到cPoint点到两条线段相交点的距离
            var centerLen = CalcLineLen(cPoint, oPoint);
            //得到圆心点坐标
            tmpX = oPoint.X + (cPoint.X - oPoint.X) / centerLen * circleLen;
            tmpY = oPoint.Y + (cPoint.Y - oPoint.Y) / centerLen * circleLen;
            var circlePoint = new Point(tmpX, tmpY);

            ellipse.SetValue(Canvas.LeftProperty, circlePoint.X - ellipse.Width / 2);
            ellipse.SetValue(Canvas.TopProperty, circlePoint.Y - ellipse.Width / 2);

            return true;
        }





        private double CalcAngle(Point startPoint, Point second, Point third, Point fourth)
        {

            var angle = CalcAngle(startPoint, second, fourth);


            double r = ellipse.Width / 2; //夹角圆的半径长度
            var len = r / Math.Tan(angle / 2 * Math.PI / 180); //线段夹角与圆相切点的长度

            //var x = x1 + (x2 - x1) * L /√((x2 - x1)2 - (y2 - y1)2 
            //var y = y1 + (y2 - y1) * L /√((x2 - x1)2 - (y2 - y1)2

            //得到第一条边与圆相切的坐标
            double x1 = second.X, x2 = startPoint.X;
            double y1 = second.Y, y2 = startPoint.Y;

            var x = x1 + (x2 - x1) * len / Math.Sqrt(Math.Abs(Math.Pow((x2 - x1), 2) - Math.Pow((y2 - y1), 2)));
            var y = y1 + (y2 - y1) * len / Math.Sqrt(Math.Abs(Math.Pow((x2 - x1), 2) - Math.Pow((y2 - y1), 2)));
            var point1 = new Point(x, y);

            x1 = third.X;
            y1 = third.Y;
            x2 = fourth.X;
            y2 = fourth.Y;
            var value = Math.Sqrt(Math.Abs(Math.Pow((x2 - x1), 2) - Math.Pow((y2 - y1), 2)));
            if (value == 0)
            {
                x = x1;
                y = y1;
            }
            else
            {
                x = x1 + (x2 - x1) * len / value;
                y = y1 + (y2 - y1) * len / value;
            }
            var point2 = new Point(x, y);


            var line1Len = CalcLineLen(startPoint, second);
            var tmpX = second.X + (startPoint.X - second.X) / line1Len * len;
            var tmpY = second.Y + (startPoint.Y - second.Y) / line1Len * len;
            point1 = new Point(tmpX, tmpY);



            var line2Len = CalcLineLen(third, fourth);
            tmpX = third.X + (fourth.X - third.X) / line2Len * len;
            tmpY = third.Y + (fourth.Y - third.Y) / line2Len * len;
            point2 = new Point(tmpX, tmpY);







            //var line = new Line();
            //line.X1 = point1.X;
            //line.Y1 = point1.Y;
            //line.X2 = second.X;
            //line.Y2 = second.Y;
            //line.Stroke = Brushes.Blue;
            //canvasPanel.Children.Add(line);

            //var line1 = new Line();
            //line1.X1 = third.X;
            //line1.Y1 = third.Y;
            //line1.X2 = point2.X;
            //line1.Y2 = point2.Y;
            //line1.Stroke = Brushes.Blue;
            //canvasPanel.Children.Add(line1);



            var centerPoint = new Point();
            centerPoint.X = point1.X + (point2.X - point1.X) / 2;
            centerPoint.Y = point1.Y + (point2.Y - point1.Y) / 2;

            var circleLen = Math.Sqrt(Math.Pow(len, 2) + Math.Pow(r, 2));
            var centerLen = CalcLineLen(centerPoint, second);
            tmpX = second.X + (centerPoint.X - second.X) / centerLen * circleLen;
            tmpY = second.Y + (centerPoint.Y - second.Y) / centerLen * circleLen;
            var point3 = new Point(tmpX, tmpY);


            ellipse.SetValue(Canvas.LeftProperty, point3.X - ellipse.Width / 2);
            ellipse.SetValue(Canvas.TopProperty, point3.Y - ellipse.Width / 2);

            //line1 = new Line();
            //line1.X1 = third.X;
            //line1.Y1 = third.Y;
            //line1.X2 = point3.X;
            //line1.Y2 = point3.Y;
            //line1.Stroke = Brushes.Blue;
            //canvasPanel.Children.Add(line1);


            //var v = Math.Sqrt(Math.Abs((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y)));
            //var v2 = Math.Sqrt(Math.Abs((point2.X - third.X) * (point2.X - third.X) + (point2.Y - third.Y) * (point2.Y - third.Y)));
            //var v3 = Math.Sqrt(Math.Abs((point1.X - second.X) * (point1.X - second.X) + (point1.Y - second.Y) * (point1.Y - second.Y)));


            return angle;
        }
        /// <summary>
        /// 获取一条线段的长度
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private double CalcLineLen(Point startPoint, Point endPoint)
        {
            var len = Math.Sqrt(Math.Abs(Math.Pow((endPoint.X - startPoint.X), 2) + Math.Pow((endPoint.Y - startPoint.Y), 2)));
            return len;
        }


        private bool isDown = false;
        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDown = true;
        }

        private void ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDown = false;
        }

        private void canvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(null);
            if (isDown && e.LeftButton == MouseButtonState.Pressed)
            {
                ellipse.SetValue(Canvas.LeftProperty, point.X);
                ellipse.SetValue(Canvas.TopProperty, point.Y);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MakeLine();
        }
    }
}
