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

namespace WpfApplication1
{
    /// <summary>
    /// ContainerBox.xaml 的交互逻辑
    /// </summary>
    public partial class ContainerBox : UserControl
    {
        private bool flag = false;
        /// <summary>
        /// 是否是标记，标记的箱属于旁边的标记(1 2 3..n)
        /// </summary>
        public bool Flag
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;
                if (value)
                {
                    border.Background = Brushes.Transparent;
                }
            }
        }

        public string Text
        {
            get
            {
                return txt.Text;
            }
            set
            {
                txt.Text = value;
            }
        }

        public ContainerBox()
        {
            InitializeComponent();
        }



    }
}
