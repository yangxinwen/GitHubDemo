using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.UI
{
    public class Utils
    {
        public static void ApplyStyleResource(ThemeStyleEnum themeStyle, SizeStyleEnum sizeStyle = SizeStyleEnum.MidSize)
        {
            Application.Current.Resources.MergedDictionaries.Clear();

            var packUri = string.Empty;

            switch (themeStyle)
            {
                case ThemeStyleEnum.LightStyle:
                    packUri= @"/WPF.UI;component/Themes/LightStyle.xaml";
                    break;
                case ThemeStyleEnum.DeepStyle:
                    packUri = @"/WPF.UI;component/Themes/DeepStyle.xaml";
                    break;
                default:
                    break;
            }

            var resourceDir = Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDir);


            packUri = string.Empty;
            switch (sizeStyle)
            {
                case SizeStyleEnum.SmallSize:
                    packUri = @"/WPF.UI;component/Styles/SmallSizes.xaml";
                    break;
                case SizeStyleEnum.MidSize:
                    packUri = @"/WPF.UI;component/Styles/MidSizes.xaml";
                    break;
                case SizeStyleEnum.BigSize:
                    packUri = @"/WPF.UI;component/Styles/BigSizes.xaml";
                    break;
                default:
                    break;
            }
            resourceDir = Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDir);            
        }
    }

    public enum ThemeStyleEnum
    {
        LightStyle,
        DeepStyle
    }

    public enum SizeStyleEnum
    {
        SmallSize,
        MidSize,
        BigSize
    }
}
