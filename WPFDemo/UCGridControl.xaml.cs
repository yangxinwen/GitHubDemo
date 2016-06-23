using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;
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

namespace WPFDemo
{
    /// <summary>
    /// PageGridControl.xaml 的交互逻辑
    /// </summary>
    public partial class UCGridControl : GridControl
    {
        private Visibility showOrderNum = Visibility.Collapsed;
        /// <summary>
        /// 是否显示序号
        /// </summary>
        public Visibility ShowOrderNum
        {
            get
            {
                return showOrderNum;
            }
            set
            {
                showOrderNum = value;
                if (value == Visibility.Visible)
                {
                    tbv.IndicatorWidth = 30;
                    var dt = this.FindResource("DataTemplate1") as DataTemplate;
                    if (dt != null)
                        tbv.RowIndicatorContentTemplate = dt;
                }
            }
        }
        /// <summary>
        /// 序号列宽度
        /// </summary>
        public double OrderNumWidth
        {
            get
            {
                return tbv.IndicatorWidth;
            }
            set
            {
                tbv.IndicatorWidth = value;
            }
        }


        public UCGridControl()
        {
            InitializeComponent();

        }


    }

    /// <summary>
    /// 行序号转换器
    /// </summary>
    public class RowOrderNumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var rowHandle = value as RowHandle;
            if (rowHandle != null)
            {
                int num = rowHandle.Value;
                return (num + 1);
            }
            return "2";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
}
