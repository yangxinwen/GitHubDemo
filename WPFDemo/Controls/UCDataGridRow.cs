using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.Controls
{
    public partial class DataGridRowd: Control
    {
        /// <summary>
        /// 详情面板背景色
        /// </summary>
        public static readonly DependencyProperty RowDetailBackgroundProperty =
            DependencyProperty.Register("ButtonType", typeof(Brush), typeof(DataGridRow),
                new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 详情面板背景色
        /// </summary>
        public Brush RowDetailBackground
        {
            get
            {
                return (Brush)GetValue(RowDetailBackgroundProperty);
            }
            set
            {
                SetValue(RowDetailBackgroundProperty, value);
            }
        }

    
    }
}
