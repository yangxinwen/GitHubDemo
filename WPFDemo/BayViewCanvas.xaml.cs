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

namespace WPFDemo
{
    /// <summary>
    /// BayViewCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class BayViewCanvas : UserControl
    {
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
        public BayViewCanvas()
        {
            InitializeComponent();
            InitBox();
        }

        private void InitBox()
        {
            listBorder.Clear();
            int colCount = 6, rowCount = 7; //列\层
            this.colCount = colCount;
            this.rowCount = rowCount;
            //列是否从左到右
            bool leftToRight = false;
            this.leftToRight = leftToRight;
            var arry = new int[rowCount, colCount];  //1-有箱  2-无箱  3-不能放箱

            //设置初始数据和表格
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    arry[i, j] = 1;
                }
            }

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

                if (startPoint.X <= (ct1.Margin.Left + ct1.Width) || startPoint.Y < (minCart.Margin.Top + minCart.Height + 50))
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
                        //box.Flag = true;
                        //box.Text = row + "," + col;
                        if (listBorder.Count <= row)
                            listBorder.Add(new List<ContainerBox>());
                        listBorder[row - 1].Add(box);
                    }
                    rootCanvas.Children.Add(box);
                }
            }

            SetBoxStatus(arry);
        }


        private void SetBoxStatus(int[,] boxs)
        {
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


            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    var keyValue = ConvertToAdvanPoint(row, col);
                    listBorder[keyValue.Key][keyValue.Value].BoxStatus = boxs[row, col];
                }
            }

        }



        /// <summary>
        /// 标准坐标转换成贝位图坐标(起始坐标都是0开始)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private KeyValuePair<int, int> ConvertToViewPoint(int x, int y)
        {
            int x1, y1;
            x1 = this.rowCount - x;

            if (leftToRight)
                y1 = this.colCount - y;
            else
                y1 = this.colCount;
            x1--;
            y1--;
            return new KeyValuePair<int, int>(x1, y1);
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

        public void Refresh()
        {

            var keyValue = ConvertToAdvanPoint(4, 0);
            var box = listBorder[keyValue.Key][keyValue.Value];     
            
            if (true)
            {
                //抓箱，放箱
                MoveBoxOut(box, 50, 50);
                var th = new Thread(new ThreadStart(
                    new Action(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(1 * 1000);
                            if (isMoveBoxCompleted)
                            {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    var keyValue1 = ConvertToAdvanPoint(3, 2);
                                    var box1 = listBorder[keyValue1.Key][keyValue1.Value];
                                    MoveBoxIn(operatingBox, box1);
                                }));
                                break;
                            }
                        }

                    })
                    ));

                th.Start();
            }
            else if (false)
            {
                //抓箱到集卡
                MoveBoxOut(box, 50, 50);
                var th = new Thread(new ThreadStart(
                    new Action(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(1 * 1000);
                            if (isMoveBoxCompleted)
                            {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    var keyValue1 = ConvertToAdvanPoint(3, 2);
                                    var box1 = listBorder[keyValue1.Key][keyValue1.Value];
                                    MoveBoxInCt(operatingBox);
                                }));
                                break;
                            }
                        }

                    })
                    ));

                th.Start();
            }
            else if (true)
            {
                //集卡到堆场
                var newBox = box.Clone();
                newBox.X = ct1.X;
                newBox.Y = rootCanvas.Height - ct1.Height;
                rootCanvas.Children.Add(newBox);
                MoveBoxOut(newBox, 50, 50);

                var th = new Thread(new ThreadStart(
                    new Action(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(1 * 1000);
                            if (isMoveBoxCompleted)
                            {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    var keyValue1 = ConvertToAdvanPoint(1, 4);
                                    var box1 = listBorder[keyValue1.Key][keyValue1.Value];
                                    MoveBoxIn(operatingBox, box1);
                                }));
                                break;
                            }
                        }

                    })
                    ));

                th.Start();
            }

            return;
        }
        /// <summary>
        /// 移动小车到指定位置
        /// </summary>
        /// <param name="x"></param>
        private void MoveMinCart(double x)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = minCart.Margin;             //起始值
            ta.To = new Thickness(x, minCart.Margin.Top, minCart.Margin.Right, minCart.Margin.Bottom);        //结束值
            ta.Duration = TimeSpan.FromSeconds(1);         //动画持续时间
            this.minCart.BeginAnimation(Border.MarginProperty, ta);//开始动画
        }

        /// <summary>
        /// 正被操作的箱子
        /// </summary>
        private ContainerBox operatingBox = null;
        private void MoveBoxOut(ContainerBox box, int x, int y)
        {
            var newBox = box.Clone();
            rootCanvas.Children.Add(newBox);
            box.BoxStatus = 2;
            MoveBox(newBox, x, y, null);
            operatingBox = newBox;

            Binding bind = new Binding();
            bind.Path = new PropertyPath(Canvas.LeftProperty);
            bind.Source = newBox;
            minCart.SetBinding(Canvas.LeftProperty, bind);
        }

        private void MoveBoxIn(ContainerBox sourceBox, ContainerBox objBox)
        {
            MoveBox(sourceBox, objBox.X, objBox.Y, new Action(() =>
            {
                rootCanvas.Children.Remove(sourceBox);
                objBox.BoxStatus = 1;
            }));
        }

        private void MoveBoxInCt(ContainerBox sourceBox)
        {
            MoveBox(sourceBox, ct1.X, rootCanvas.Height - ct1.Height, new Action(() =>
             {
                 rootCanvas.Children.Remove(sourceBox);
             }));
        }
        private bool isMoveBoxCompleted = true;
        private void MoveBox(ContainerBox box, double x, double y, Action action)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = box.Y;
            da.To = y;
            da.Duration = TimeSpan.FromSeconds(1);
            da.Completed += (s, e) =>
            {
                action?.Invoke();
                isMoveBoxCompleted = true;
            };
            box.BeginAnimation(Canvas.TopProperty, da);


            DoubleAnimation da1 = new DoubleAnimation();
            da1.From = box.X;
            da1.To = x;
            da1.Duration = TimeSpan.FromSeconds(1);
            da1.Completed += (s, e) => { action?.Invoke(); };
            box.BeginAnimation(Canvas.LeftProperty, da1);
            isMoveBoxCompleted = false;
        }

    }
}
