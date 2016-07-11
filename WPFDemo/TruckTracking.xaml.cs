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
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace WPFDemo
{
    /// <summary>
    /// TruckTracking.xaml 的交互逻辑
    /// </summary>
    public partial class TruckTracking : UserControl
    {
        /// <summary>
        /// 是否正在播放
        /// </summary>
        private bool isPlaying = false;
        public string TimeInfoStr
        {
            set
            {
                if (!tbTimeInfo.Text.Equals(value))
                    tbTimeInfo.Text = value;
            }
        }

        /// <summary>
        /// 是否显示轨迹
        /// </summary>
        public bool ShowTracking { get; set; } = true;

        private List<KeyValuePair<DateTime, Line>> Lines = new List<KeyValuePair<DateTime, Line>>();
        public TruckTracking()
        {
            InitializeComponent();
            //this.MouseUp += TruckTracking_MouseUp;
            //this.MouseMove += TruckTracking_MouseMove;

            this.Loaded += TruckTracking_Loaded;

            ucSlider.PreviewMouseLeftButtonUp += Slider_MouseLeftButtonUp;
            ucSlider.Slider.ValueChanged += Slider_ValueChanged;
            //slider
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(ucSlider);
            progressCurrentNum = ucSlider.Slider.Value = point.X / ucSlider.Slider.ActualWidth * progressMaxnum;
            ChangeProgress();
        }

        private void TruckTracking_Loaded(object sender, RoutedEventArgs e)
        {
            canvasRoot.SizeChanged += TruckTracking_SizeChanged;
            MarkTracking();
        }

        private void TruckTracking_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (FrameworkElement item in canvasRoot.Children)
            {
                var ct = item as ContainerTruckDot;
                if (ct != null)
                {
                    ct.X = ct.X / e.PreviousSize.Width * e.NewSize.Width;
                    ct.Y = ct.Y / e.PreviousSize.Height * e.NewSize.Height;
                }
            }

            foreach (var line in Lines)
            {
                var item = line.Value;
                item.X1 = item.X1 / e.PreviousSize.Width * e.NewSize.Width;
                item.Y1 = item.Y1 / e.PreviousSize.Height * e.NewSize.Height;
                item.X2 = item.X2 / e.PreviousSize.Width * e.NewSize.Width;
                item.Y2 = item.Y2 / e.PreviousSize.Height * e.NewSize.Height;
            }
        }

        private void TruckTracking_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isUp) return;
            //记录鼠标移动轨迹(x,y为canvasRoot的比例)
            var point = e.GetPosition(canvasRoot);
            Debug.WriteLine(point.X / canvasRoot.ActualWidth + "," + point.Y / canvasRoot.ActualHeight);
        }

        bool isUp = false;

        private void TruckTracking_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isUp = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            StartNewTask();
        }

        private int currentLine = -1;
        /// <summary>
        /// 进度条最大值
        /// </summary>
        private double progressMaxnum;
        /// <summary>
        /// 进度条当前值
        /// </summary>
        private double progressCurrentNum;

        /// <summary>
        /// 集卡轨迹model
        /// </summary>
        private TruckTrackingModel truckTrackingModel;

        /// <summary>
        /// 上一个时间点
        /// </summary>
        private DateTime lastPostion;
        private void MonitorSliderProcess()
        {
            while (true)
            {
                Thread.Sleep(10);
                if (lastPostion.Ticks == ucSlider.Slider.Value)
                    continue;





            }
        }

        /// <summary>
        /// 当前显示的集卡
        /// </summary>
        private ContainerTruckDot ct;


        /// <summary>
        /// 设置集卡位置
        /// </summary>
        /// <param name="ct">集卡</param>
        /// <param name="point">比例坐标</param>
        private void SetCTPostion(ContainerTruckDot ct, Point point)
        {
            var newX = point.X * canvasRoot.ActualWidth - ct.ActualWidth / 2;
            var newY = point.Y * canvasRoot.ActualHeight - ct.ActualHeight / 2;
            ct.X = newX;
            ct.Y = newY;
        }

        /// <summary>
        /// 生成一条线
        /// </summary>
        /// <param name="startPoint">比例坐标</param>
        /// <param name="endPoint">比例坐标</param>
        private Line MakeLine(Point startPoint, Point endPoint)
        {
            var line = new Line();
            line.X1 = startPoint.X * canvasRoot.ActualWidth;
            line.Y1 = startPoint.Y * canvasRoot.ActualHeight;
            line.X2 = endPoint.X * canvasRoot.ActualWidth;
            line.Y2 = endPoint.Y * canvasRoot.ActualHeight;
            line.Stroke = Brushes.Red;
            line.StrokeThickness = 1;
            return line;
        }

        /// <summary>
        /// 生成轨迹记录
        /// </summary>
        private void MarkTracking()
        {
            var model = new TruckTrackingModel();
            model.TruckNo = "001";
            model.TruckNum = "sdfsdfdfs";
            model.ListPoint = GetCTLine();
            truckTrackingModel = model;

            ContainerTruckDot ct = new ContainerTruckDot();
            canvasRoot.Children.Add(ct);
            this.ct = ct;

            //生成所有的轨迹线，先隐藏
            Line line = null;
            for (int i = 1; i < model.ListPoint.Count; i++)
            {
                line = MakeLine(model.ListPoint[i - 1].Value, model.ListPoint[i].Value);
                line.Visibility = Visibility.Collapsed;
                canvasRoot.Children.Add(line);
                Lines.Add(new KeyValuePair<DateTime, Line>(model.ListPoint[i].Key, line));
            }

            ucSlider.Slider.Maximum = progressMaxnum = (truckTrackingModel.EndTime - truckTrackingModel.StartTime).TotalSeconds;
        }


        /// <summary>
        /// 播放状态 1-播放中 2-暂停 3-停止
        /// </summary>
        private int playStatus = 3;

        /// <summary>
        /// 播放
        /// </summary>
        private void Play()
        {
            playStatus = 1;
            this.Dispatcher.Invoke(new Action(() =>
            {
                SetCTPostion(ct, truckTrackingModel.ListPoint[0].Value);
            }));
            Thread.Sleep(truckTrackingModel.ListPoint[1].Key - truckTrackingModel.ListPoint[0].Key);

            for (int i = 0; i < Lines.Count; i++)
            {
                //暂停
                while (playStatus == 2)
                {
                    Thread.Sleep(50);
                }

                //停止
                if (playStatus == 3)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        ucSlider.Slider.Value = progressCurrentNum = 0;
                        ChangeProgress();
                    }));
                    break;
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    SetCTPostion(ct, truckTrackingModel.ListPoint[i + 1].Value);
                    Lines[i].Value.Visibility = Visibility.Visible;
                    truckTrackingModel.CurrentTime = Lines[i].Key;
                }));
                if (i + 1 < Lines.Count)
                    Thread.Sleep((truckTrackingModel.ListPoint[i + 1].Key - truckTrackingModel.ListPoint[i].Key));

                if (currentLine > 0)
                {
                    i = currentLine;
                    currentLine = -1;
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    progressCurrentNum = ucSlider.Slider.Value = (truckTrackingModel.CurrentTime - truckTrackingModel.StartTime).TotalSeconds;
                    TimeInfoStr = truckTrackingModel.StartTime.ToString("yyyy-MM-dd  ") + truckTrackingModel.CurrentTime.ToString("HH:mm:ss") + "/" + truckTrackingModel.EndTime.ToString("HH:mm:ss");
                }));
            }
            playStatus = 3;
        }

        private void ChangeProgress()
        {
            int index = 0;
            var dt = truckTrackingModel.StartTime.AddSeconds(progressCurrentNum);
            foreach (var item in Lines)
            {
                if (dt >= item.Key)
                {
                    item.Value.Visibility = Visibility.Visible;
                    currentLine = index;
                }
                else
                    item.Value.Visibility = Visibility.Collapsed;
                index++;
            }

            TimeInfoStr = truckTrackingModel.StartTime.ToString("yyyy-MM-dd  ") + dt.ToString("HH:mm:ss") + "/" + truckTrackingModel.EndTime.ToString("HH:mm:ss");

            var temp = truckTrackingModel.ListPoint[0].Value;
            foreach (var item in truckTrackingModel.ListPoint)
            {
                if (dt < item.Key)
                {
                    break;
                }
                temp = item.Value;
            }
            SetCTPostion(ct, temp);
        }




        private void StartNewTask()
        {

            ContainerTruckDot ct = new ContainerTruckDot();
            canvasRoot.Children.Add(ct);
            Task.Factory.StartNew(() =>
            {
                var model = new TruckTrackingModel();
                model.TruckNo = "001";
                model.TruckNum = "sdfsdfdfs";
                model.ListPoint = GetCTLine();
                var listPath = model.ListPoint;
                Point tempPoint = new Point();       

                for (int i = 0; i < listPath.Count; i++)
                {
                    var item = listPath[i].Value;

                    if (tempPoint.X == 0 || tempPoint.Y == 0)
                        tempPoint = item;

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        var newX = item.X * canvasRoot.ActualWidth - ct.ActualWidth / 2;
                        var newY = item.Y * canvasRoot.ActualHeight - ct.ActualHeight / 2;

                        //if (ShowTracking && tempPoint != item)
                        //{
                        //    var line = new Line();
                        //    line.X1 = tempPoint.X * canvasRoot.ActualWidth;
                        //    line.Y1 = tempPoint.Y * canvasRoot.ActualHeight;
                        //    line.X2 = item.X * canvasRoot.ActualWidth;
                        //    line.Y2 = item.Y * canvasRoot.ActualHeight;
                        //    line.Stroke = Brushes.Red;
                        //    line.StrokeThickness = 1;
                        //    {
                        //        canvasRoot.Children.Add(line);
                        //        Lines.Add(new KeyValuePair<DateTime, Line>(listPath[i].Key, line));
                        //    }
                        //}
                        ct.X = newX;
                        ct.Y = newY;
                    }));
                    Thread.Sleep(1 * 30);
                    tempPoint = item;
                }
            });
        }



        /// <summary>
        /// 获取集卡路线点集合(集卡位置与canvasRoot的比例)   时间,位置
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<DateTime, Point>> GetCTLine()
        {
            var listPath = new List<string> { "Data/trucktracking1.txt", "Data/trucktracking2.txt" };
            Random random = new Random();

            var listPoint = new List<KeyValuePair<DateTime, Point>>();
            DateTime dt = DateTime.Now;
            using (var sr = new StreamReader(listPath[random.Next(listPath.Count)]))
            {
                while (!sr.EndOfStream)
                {
                    var str = sr.ReadLine();
                    var split = str.Split(',');
                    if (split != null && split.Length == 2)
                    {
                        dt = dt.AddMilliseconds(50);
                        listPoint.Add(new KeyValuePair<DateTime, Point>(dt, new Point(double.Parse(split[0]), double.Parse(split[1]))));
                    }
                }
            }
            return listPoint;
        }

        private void Pause()
        {
            playStatus = 2;
        }
        private void Stop()
        {
            playStatus = 3;
        }
        private void Last()
        {
            //不允许快退到0
            if (progressCurrentNum - 5 <= 0) return;        
            ucSlider.Slider.Value -= 5;
            progressCurrentNum = ucSlider.Slider.Value;
            ChangeProgress();
        }
        private void Next()
        {
            ucSlider.Slider.Value += 5;
            progressCurrentNum = ucSlider.Slider.Value;
            ChangeProgress();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            Last();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (playStatus == 1) return;
            else if (playStatus == 2)
                playStatus = 1;
            else if (playStatus == 3)
            {
                progressCurrentNum = ucSlider.Slider.Value=0;
                ChangeProgress();
                Task.Factory.StartNew(() =>
                {
                    Play();
                });
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }
    }

    /// <summary>
    /// 集卡跟踪Model
    /// </summary>
    public class TruckTrackingModel
    {
        /// <summary>
        /// 车次
        /// </summary>
        public string TruckNo { get; set; }
        /// <summary>
        /// 集卡车牌号
        /// </summary>
        public string TruckNum { get; set; }

        /// <summary>
        /// 时间与相应坐标点
        /// </summary>
        public List<KeyValuePair<DateTime, Point>> ListPoint { get; set; } = new List<KeyValuePair<DateTime, System.Windows.Point>>();

        /// <summary>
        /// 当前所在时间点
        /// </summary>
        public DateTime CurrentTime { get; set; }

        /// <summary>
        /// 获得开始时间
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                DateTime dt = DateTime.MinValue;
                if (ListPoint != null && ListPoint.Count > 0)
                    dt = ListPoint[0].Key;
                return dt;
            }
        }

        /// <summary>
        /// 获得结束时间
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                DateTime dt = DateTime.MinValue;
                if (ListPoint != null && ListPoint.Count > 0)
                    dt = ListPoint[ListPoint.Count - 1].Key;
                return dt;
            }
        }

    }

}
