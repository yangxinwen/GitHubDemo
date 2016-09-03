using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.Controls
{
    public class UCButton : Button
    {
        /// <summary>
        ///     Button控件样式类型
        /// </summary>
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType", typeof(ButtonType), typeof(UCButton),
                new PropertyMetadata(ButtonType.Default));

        /// <summary>
        /// 获取或设置Button控件样式类型
        /// </summary>
        public ButtonType ButtonType
        {
            get
            {
                return (ButtonType)GetValue(ButtonTypeProperty);
            }
            set
            {
                SetValue(ButtonTypeProperty, value);
            }
        }

        /// <summary>
        ///     Button控件尺寸大小类型
        /// </summary>
        public static readonly DependencyProperty ButtonSizeTypeProperty =
            DependencyProperty.Register("ButtonSizeType", typeof(ButtonSizeType), typeof(UCButton),
                new PropertyMetadata(ButtonSizeType.Default));

        /// <summary>
        /// 获取或设置Button控件尺寸大小类型
        /// </summary>
        public ButtonSizeType ButtonSizeType
        {
            get
            {
                return (ButtonSizeType)GetValue(ButtonSizeTypeProperty);
            }
            set
            {
                SetValue(ButtonSizeTypeProperty, value);
            }
        }

        /// <summary>
        ///     
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UCButton));

        /// <summary>
        /// 获取或设置Button控件圆角大小
        /// </summary>
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        public UCButton()
        {
            DefaultStyleKey = typeof(UCButton);
        }
    }
}
