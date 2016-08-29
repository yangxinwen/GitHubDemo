using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WPFDemo.Controls
{
    [TemplatePart(Name = TbTemplateName, Type = typeof(TextBlock))]
    public class UCText : TextBox
    {
        private const string TbTemplateName = "PART_ContentHost";
        private FrameworkElement tb = null;

        public override void OnApplyTemplate()
        {
            tb = this.GetTemplateChild(TbTemplateName) as FrameworkElement;
            //if (tb != null)
            //    tb.Text = "success";
            base.OnApplyTemplate();
        }

        static UCText()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(UCText), new FrameworkPropertyMetadata(typeof(UCText)));

        }        
    }
}
