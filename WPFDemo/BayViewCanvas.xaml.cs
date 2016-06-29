using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WPFDemo
{
    /// <summary>
    /// BayViewCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class BayViewCanvas : UserControl
    {
        /// <summary>
        /// 保存箱区所有箱的上一次的状态
        /// </summary>
        private int[,] lastBoxs;

        /// <summary>
        /// 最大箱区
        /// </summary>
        private Point maxBoxAreaPoint = new Point(200, 200);
        /// <summary>
        /// 列数
        /// </summary>
        private int colCount = 5;
        /// <summary>
        /// 层数
        /// </summary>
        private int rowCount = 5;
        private bool leftToRight = true;
        private List<List<ContainerBox>> listBorder = new List<List<ContainerBox>>();
        /// <summary>
        /// 移动中的箱(一直存在，没有的时候状态是2，显示的时候状态是1)
        /// </summary>
        private ContainerBox movingBox = null;
        public BayViewCanvas()
        {
            InitializeComponent();
            var model = new BayViewModel();
            model.HaveLeftCart = true;
            model.Boxs = GetInitBoxData();
            UpdateUI(model);
            this.MouseMove += BayViewCanvas_MouseMove;
        }

        private void BayViewCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var point = new Point();
            point = e.GetPosition(rootCanvas);
            Debug.WriteLine(point);
        }

        /// <summary>
        /// 更新箱区UI
        /// </summary>
        private void UpdateBoxAreaUI(int[,] boxs)
        {

            foreach (var item1 in listBorder)
            {
                foreach (var item2 in item1)
                {
                    rootCanvas.Children.Remove(item2);
                }
            }
            listBorder.Clear();


            var startPoint = new Point(100, 100);
            //得出箱在界面上的长宽
            var width = 30;
            var height = 30;

            while (true)
            {
                maxBoxAreaPoint.X = width * (colCount + 2);
                maxBoxAreaPoint.Y = height * (rowCount + 1);

                startPoint.X = (rootCanvas.Width - maxBoxAreaPoint.X) / 2;
                startPoint.Y = rootCanvas.Height - maxBoxAreaPoint.Y;

                if (startPoint.X <= (leftCart.Margin.Left + leftCart.Width) || startPoint.Y < (minCart.Margin.Top + minCart.Height + 50))
                    width = height = width - 5;
                else
                    break;
            }

            //初始化箱控件
            for (int row = 0; row < rowCount + 1; row++)
            {
                for (int col = 0; col < colCount + 2; col++)
                {
                    ContainerBox box = new ContainerBox();
                    box.Width = width;
                    box.Height = height;
                    box.SetValue(Canvas.LeftProperty, startPoint.X + width * col);
                    box.SetValue(Canvas.TopProperty, startPoint.Y + height * row);
                    box.BorderThickness = new Thickness(1);
                    box.BorderBrush = Brushes.Black;
                    if (col == 0 || col == colCount + 1)
                    {
                        if (row == 0) continue;
                        box.Flag = true;
                        box.Text = rowCount - row + 1 + "";
                        if (col == 0)
                            box.HorizontalAlignment = HorizontalAlignment.Right;
                        else
                            box.HorizontalAlignment = HorizontalAlignment.Left;
                        box.BorderThickness = new Thickness(0);
                    }
                    else if (row == 0)
                    {
                        box.Flag = true;
                        box.Text = (leftToRight ? col : colCount - col + 1) + "";
                        box.VerticalAlignment = VerticalAlignment.Bottom;
                        box.BorderThickness = new Thickness(0);
                    }
                    else
                    {
                        if (listBorder.Count <= row)
                            listBorder.Add(new List<ContainerBox>());
                        listBorder[row - 1].Add(box);
                    }
                    rootCanvas.Children.Add(box);

                    //初始化一个移动中的箱
                    if (movingBox == null)
                    {
                        var tmpBox = box.Clone();
                        tmpBox.BoxStatus = 2;
                        rootCanvas.Children.Add(tmpBox);
                        movingBox = tmpBox;
                    }
                }
            }

            //更新箱状态
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    var keyValue = ConvertToAdvanPoint(row, col);
                    listBorder[keyValue.Key][keyValue.Value].BoxStatus = boxs[row, col];
                }
            }

            lastBoxs = boxs;
        }


        /// <summary>
        /// 贝位图坐标转换成标准坐标(起始坐标都是0开始)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private KeyValuePair<int, int> ConvertToAdvanPoint(int row, int col)
        {
            int x, y;
            x = this.rowCount - 1 - row;

            if (leftToRight)
                y = col;
            else
                y = this.colCount - 1 - col;

            return new KeyValuePair<int, int>(x, y);
        }

        private bool CompareBoxs(int[,] array1, int[,] array2)
        {
            if (array1 == null || array2 == null)
                return false;

            if (array1.GetUpperBound(0) != array2.GetUpperBound(0) ||
                array1.GetUpperBound(1) != array2.GetUpperBound(1))
                return false;

            for (int i = 0; i <= array1.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= array1.GetUpperBound(1); j++)
                {
                    if (array1[i, j] != array2[i, j])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 初始化测试数据-箱  
        /// </summary>
        private int[,] GetInitBoxData()
        {
            int colCount = 6, rowCount = 7; //列\层
            this.colCount = colCount;
            this.rowCount = rowCount;
            //列是否从左到右
            bool leftToRight = false;
            this.leftToRight = leftToRight;
            var array = new int[rowCount, colCount];  //1-有箱  2-无箱  3-不能放箱

            //设置初始数据和表格
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    array[i, j] = 1;
                }
            }

            var boxs = array;
            //不能放置
            boxs[6, 5] = 3;
            boxs[6, 4] = 3;
            boxs[6, 3] = 3;
            boxs[6, 2] = 3;
            boxs[6, 1] = 3;
            boxs[6, 0] = 3;
            boxs[5, 5] = 3;
            boxs[5, 4] = 3;
            boxs[4, 5] = 3;

            //空箱区
            boxs[5, 3] = 2;
            boxs[5, 2] = 2;
            boxs[5, 1] = 2;
            boxs[5, 0] = 2;
            boxs[4, 4] = 2;
            boxs[4, 3] = 2;
            boxs[4, 2] = 2;
            boxs[3, 5] = 2;
            boxs[3, 4] = 2;
            boxs[3, 2] = 2;
            boxs[2, 5] = 2;
            boxs[2, 4] = 2;
            boxs[1, 5] = 2;
            boxs[1, 4] = 2;
            boxs[0, 5] = 2;

            return array;
        }

        private void UpdateUI(BayViewModel model)
        {
            //更新拖链小车位置
            if (model.MinCartPoint != null && model.MinCartPoint.X != 0 && model.MinCartPoint.Y != 0)
            {
                minCart.SetValue(Canvas.LeftProperty, model.MinCartPoint.X);
                minCart.SetValue(Canvas.TopProperty, model.MinCartPoint.Y);
            }
            //更新箱区
            if (!CompareBoxs(lastBoxs, model.Boxs))
                UpdateBoxAreaUI(model.Boxs);

            //更新移动箱的位置
            if (model.MovingBoxPoint != null && model.MovingBoxPoint.X != 0 && model.MovingBoxPoint.Y != 0)
            {
                movingBox.SetValue(Canvas.LeftProperty, model.MovingBoxPoint.X);
                movingBox.SetValue(Canvas.TopProperty, model.MovingBoxPoint.Y);
                movingBox.BoxStatus = 1;
            }
            else
                movingBox.BoxStatus = 2;

            //更新集卡
            leftCart.ContainerBoxHeight = movingBox.Height;
            leftCart.ContainerBoxWidth = movingBox.Width;

            rightCart.ContainerBoxHeight = movingBox.Height;
            rightCart.ContainerBoxWidth = movingBox.Width;

            if (model.HaveLeftCart)
                leftCart.Visibility = Visibility.Visible;
            else
                leftCart.Visibility = Visibility.Collapsed;

            if (model.HaveRightCart)
                rightCart.Visibility = Visibility.Visible;
            else
                rightCart.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// 箱到左边集卡
        /// </summary>
        public void Test1()
        {
            var list = new List<BayViewModel>();
            var model = new BayViewModel();
            model.HaveLeftCart = true;
            model.Boxs = lastBoxs;
            list.Add(model);
            BayViewModel tmp = new BayViewModel();
            try
            {
                StreamReader sr = new StreamReader("Data/2,2ToCartpoint.txt");
                string str = string.Empty;
                while (!sr.EndOfStream)
                {
                    str = sr.ReadLine();
                    if (str != null)
                    {
                        var split = str.Split(',');
                        if (split != null && split.Length == 2)
                        {
                            tmp = model.Clone() as BayViewModel;
                            tmp.MovingBoxPoint = new Point(double.Parse(split[0]), double.Parse(split[1]));
                            tmp.MinCartPoint = new Point(tmp.MovingBoxPoint.X, (double)minCart.GetValue(Canvas.TopProperty));
                            tmp.Boxs[2, 2] = 2;
                            list.Add(tmp);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            Task.Factory.StartNew(() =>
            {
                foreach (var item in list)
                {
                    Thread.Sleep(1 * 100);
                    this.Dispatcher.Invoke(new Action(() => { UpdateUI(item); }), null);
                }

            });
        }

        /// <summary>
        /// 左边集卡到箱
        /// </summary>
        public void Test2()
        {
            var list = new List<BayViewModel>();
            var model = new BayViewModel();
            model.HaveLeftCart = true;
            model.Boxs = lastBoxs;
            list.Add(model);
            BayViewModel tmp = new BayViewModel();
            try
            {
                StreamReader sr = new StreamReader("Data/CartTo2,2point.txt");
                string str = string.Empty;
                while (!sr.EndOfStream)
                {
                    str = sr.ReadLine();
                    if (str != null)
                    {
                        var split = str.Split(',');
                        if (split != null && split.Length == 2)
                        {
                            tmp = model.Clone() as BayViewModel;
                            tmp.MovingBoxPoint = new Point(double.Parse(split[0]), double.Parse(split[1]));
                            tmp.MinCartPoint = new Point(tmp.MovingBoxPoint.X, (double)minCart.GetValue(Canvas.TopProperty));
                            //tmp.Boxs[2, 2] = 2;
                            list.Add(tmp);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            Task.Factory.StartNew(() =>
            {
                foreach (var item in list)
                {
                    Thread.Sleep(1 * 100);
                    this.Dispatcher.Invoke(new Action(() => { UpdateUI(item); }), null);
                }

            });
        }

        /// <summary>
        /// 箱到箱
        /// </summary>
        public void Test3()
        {
            var list = new List<BayViewModel>();
            var model = new BayViewModel();
            model.HaveLeftCart = true;
            model.Boxs = lastBoxs;
            list.Add(model);
            BayViewModel tmp = new BayViewModel();
            try
            {
                StreamReader sr = new StreamReader("Data/4,1To1,4point.txt");
                string str = string.Empty;
                while (!sr.EndOfStream)
                {
                    str = sr.ReadLine();
                    if (str != null)
                    {
                        var split = str.Split(',');
                        if (split != null && split.Length == 2)
                        {
                            tmp = model.Clone() as BayViewModel;
                            tmp.MovingBoxPoint = new Point(double.Parse(split[0]), double.Parse(split[1]));
                            tmp.MinCartPoint = new Point(tmp.MovingBoxPoint.X, (double)minCart.GetValue(Canvas.TopProperty));
                            tmp.Boxs[4, 1] = 2;
                            list.Add(tmp);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
           

            Task.Factory.StartNew(() =>
            {
                foreach (var item in list)
                {
                    Thread.Sleep(1 * 100);
                    this.Dispatcher.Invoke(new Action(() => { UpdateUI(item); }), null);
                }

            });
        }




        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Test1();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Test2();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Test3();
        }

    }



    /// <summary>
    /// 贝位视图model
    /// </summary>
    public class BayViewModel : ICloneable
    {
        /// <summary>
        /// 存在左边集卡
        /// </summary>
        public bool HaveLeftCart { get; set; }
        /// <summary>
        /// 存在右边集卡
        /// </summary>
        public bool HaveRightCart { get; set; }

        /// <summary>
        /// 拖链小车位置
        /// </summary>
        public Point MinCartPoint { get; set; }
        /// <summary>
        /// 正在被移动箱的(空中箱)位置（null 或者 0,0 代表没有箱被移动或被悬吊）
        /// </summary>
        public Point MovingBoxPoint { get; set; }

        public int[,] Boxs { get; set; }

        public object Clone()
        {
            var obj = this.MemberwiseClone() as BayViewModel;
            obj.Boxs = Boxs.Clone() as int[,];
            return obj;
        }
    }
}
