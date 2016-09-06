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
    /// UCBusy.xaml 的交互逻辑
    /// </summary>
    public  class UCBusy : ContentControl
    {

        private Grid _gridBusy = null;
        private Grid _gridContent = null;


        static UCBusy()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(UCBusy), new FrameworkPropertyMetadata(typeof(UCBusy)));

        }

        public UCBusy()
        {
            DefaultStyleKey = typeof(UCBusy);

            this.Loaded += UCBusy_Loaded;
        }

        private void UCBusy_Loaded(object sender, RoutedEventArgs e)
        {
            if (_gridBusy != null)
            {
                if (IsBusy)
                {
                    _gridBusy.Visibility = Visibility.Visible;
                }
                else
                {
                    _gridBusy.Visibility = Visibility.Collapsed;
                }
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                if (_gridBusy != null)
                {
                    if (value)
                    {
                        _gridBusy.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _gridBusy.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }


        public object Container
        {
            get; set;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _gridBusy = GetTemplateChild("gridBusy") as Grid;
            //_gridContent = GetTemplateChild("gridContent") as Grid;


            //if (_gridContent != null)
            //{
            //    _gridContent.Children.Clear();
            //    _gridContent.Children.Add(Container as UIElement);
            //}     
        }
    }
}
