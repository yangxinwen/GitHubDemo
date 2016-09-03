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

namespace WPFDemo.Controls
{
    /// <summary>
    /// UCPager.xaml 的交互逻辑
    /// </summary>
    public partial class UCPager : UserControl
    {
        private List<Button> _pageIndexButtons = new List<Button>();

        public int PageCount { get; set; }
        public int CurrentPageIndex { get; private set; }
        /// <summary>
        /// 记录当前页的开始索引
        /// </summary>
        private int _currentPageStartIndex = 0;
        /// <summary>
        /// 可见索引数量
        /// </summary>
        public int VisibleIndexCount
        {
            private set;
            get;
        }
        /// <summary>
        /// 页索引数量
        /// </summary>
        private int _pageIndexCount = 5;


        public UCPager()
        {
            PageCount = 10;

            InitializeComponent();

            AddPageIndexButton();
        }

        /// <summary>
        /// 更改当前页
        /// </summary>
        /// <param name="index"></param>
        private void ChangeCurrentPage(int index)
        {
            if (index < 0 || index >= PageCount)
                return;

            if (index == CurrentPageIndex)
                return;

            CurrentPageIndex = index;
            if (PageCount <= _pageIndexCount)
            {
                SetSelectButton(index);
                return;
            }

            var range = _pageIndexCount / 2;
            var startIndex = index - range;
            var endIndex = index + range;

            while (startIndex < 0)
            {
                startIndex++;
                endIndex++;
            }

            while (endIndex > PageCount - 1)
            {
                endIndex--;
                startIndex--;
            }


            var tmp = startIndex;
            foreach (var item in _pageIndexButtons)
            {
                item.Content = tmp + 1;
                tmp++;
            }

            SetSelectButton(index);
        }

        private void SetSelectButton(int index)
        {
            foreach (var item in _pageIndexButtons)
            {
                if (item.Content.ToString().Equals(((index + 1).ToString())))
                {
                    item.Background = Brushes.Gray;
                }
                else
                {
                    item.Background = Brushes.Transparent;
                }
            }
        }

        private void NextPage()
        {
            //算法描述:首先求出最后一个可见索引可以向后移动的最大偏移量，然后根据偏移量整体移动
            //求出结束索引
            int endIndex = _currentPageStartIndex + _pageIndexCount - 1;
            var nextOffset = 0; //前移偏移量
            var offset = (PageCount - 1) - endIndex;
            if (offset >= _pageIndexCount)
                nextOffset = _pageIndexCount;
            else if (offset > 0)
                nextOffset = offset;
            else
                nextOffset = 0;

            if (nextOffset > 0)
            {
                var tmp = _currentPageStartIndex + nextOffset;
                _currentPageStartIndex = tmp;
                foreach (var item in _pageIndexButtons)
                {
                    item.Content = tmp + 1;
                    tmp++;
                }
            }
        }
        private void LastPage()
        {
            //算法描述:首先根据可见索引的起始位置可以得出可向前移动的最大偏移量，然后根据偏移量整体移动
            //起始索引
            int startIndex = _currentPageStartIndex;
            var lastOffset = 0; //后移偏移量
            var offset = startIndex - _pageIndexCount;
            if (offset >= 0)
                lastOffset = _pageIndexCount;
            else
                lastOffset = startIndex - 0;

            if (lastOffset > 0)
            {
                var tmp = _currentPageStartIndex - lastOffset;
                _currentPageStartIndex = tmp;
                foreach (var item in _pageIndexButtons)
                {
                    item.Content = tmp + 1;
                    tmp++;
                }
            }
        }


        private void AddPageIndexButton()
        {

            _pageIndexButtons.Clear();
            gridPageIndex.ColumnDefinitions.Clear();
            for (int i = 0; i < _pageIndexCount; i++)
            {
                gridPageIndex.ColumnDefinitions.Add(new ColumnDefinition());
                var btn = new Button();
                btn.Background = Brushes.Transparent;
                btn.SetValue(Grid.ColumnProperty, i);
                btn.Click += Btn_Click;
                gridPageIndex.Children.Add(btn);
                _pageIndexButtons.Add(btn);
            }

            SetPageIndexPanel(0);

        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            int index = 0;
            if (int.TryParse(btn.Content.ToString(), out index))
            {
                ChangeCurrentPage(index - 1);
            }
        }

        private void UpVisibleIndexCount()
        {
            var visibleIndexCount = 0;
            //若页面总数小于初始化的页索引按钮总数则隐藏掉多余的页索引按钮
            for (int i = 0; i < _pageIndexButtons.Count; i++)
            {
                if (i < PageCount)
                {
                    _pageIndexButtons[i].Visibility = Visibility.Visible;
                    gridPageIndex.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                    visibleIndexCount++;
                }
                else
                {
                    _pageIndexButtons[i].Visibility = Visibility.Collapsed;
                    gridPageIndex.ColumnDefinitions[i].Width = GridLength.Auto;
                }
            }
            VisibleIndexCount = visibleIndexCount;
        }

        private void SetPageIndexPanel(int startIndex)
        {
            _currentPageStartIndex = startIndex;
            UpVisibleIndexCount();
            if (VisibleIndexCount + startIndex <= PageCount)
            {
                for (int i = 0; i < VisibleIndexCount; i++)
                {
                    _pageIndexButtons[i].Content = ++startIndex;
                }
            }
            //var offset = PageCount - startIndex;



            //if (startIndex + pageIndexButtons.Count <= PageCount)
            //{
            //    foreach (var item in pageIndexButtons)
            //    {
            //        item.Content = ++startIndex;
            //    }
            //}
            //else if ()
            //{

            //}


        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            NextPage();
        }

        private void lastBtn_Click(object sender, RoutedEventArgs e)
        {
            LastPage();
        }
    }
}
