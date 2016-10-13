using System;
using System.Windows;
using System.Globalization;
namespace WPFDemo.Converter
{
    /// <summary>
    /// 布尔值转Visibility
    /// </summary>
    public class BoolToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)

        {
            return "1".Equals(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter Members
    }

    /// <summary>
    /// 布尔值转Visibility
    /// </summary>
    public class BoolToVisibilityHidConverter : System.Windows.Data.IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)

        {
            return "1".Equals(value) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter Members
    }
}