using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo.Controls
{
    public class ItemsVirtualizingPanel: VirtualizingPanel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            //return base.MeasureOverride(availableSize);

            var size = new Size();
            size.Width = 100;
            size.Height = 500;
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = new Size();
            size.Width = 100;
            size.Height = 500;
            return size;
        }
    }
}
