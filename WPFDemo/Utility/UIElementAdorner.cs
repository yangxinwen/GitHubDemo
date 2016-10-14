using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFDemo.Utility
{
    /// <summary>
    ///  给UIElement 增加一个Border 装饰
    /// </summary>
    public class UIElementAdorner : Adorner
    {
        Border border;
        VisualCollection VisualChildren;
        public UIElementAdorner(UIElement adnoredElement)
            : base(adnoredElement)
        {
            VisualChildren = new VisualCollection(this);
            AddBorder(adnoredElement);

        }
        void AddBorder(UIElement element)
        {
            border = new Border();
            border.Background = Brushes.Red;
            VisualChildren.Add(border);

        }

        public void HideAdorner()
        {
            border.Visibility = Visibility.Collapsed;
        }

        public void ShowAdorner()
        {
            border.Visibility = Visibility.Visible;

        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }
        protected override Visual GetVisualChild(int index)
        {
            return border;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            border.Arrange(new Rect(0, this.DesiredSize.Height, this.DesiredSize.Width, 2));
            return base.ArrangeOverride(finalSize);
        }

    }
}
