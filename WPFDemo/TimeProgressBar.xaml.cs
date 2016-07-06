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
    /// 时间进度条
    /// </summary>
    public partial class TimeProgressBar : UserControl
    {
        private int progressValue;  // 0  1   2   3
        public int ProgressValue
        {
            get
            {
                return progressValue;
            }
            set
            {
                progressValue = value;
                SetProgress();
            }
        }


        public TimeProgressBar()
        {
            InitializeComponent();
        }

        public void SetProgress()
        {
            switch (progressValue)
            {
                case 0:
                    progressBar.Value = 0;
                    break;
                case 1:
                    progressBar.Value = 100/3/2;
                    break;
                case 2:
                    progressBar.Value = 100/2;
                    break;
                case 3:
                    progressBar.Value = 100;
                    break;
                default:
                    break;
            }
        }
    }
}
