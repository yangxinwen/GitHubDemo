using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo.Controls
{
    public class UCDataGrid : DataGrid
    {
        public UCDataGrid()
        {
            this.Style = this.FindResource("UCDataGridStyle") as Style;
            //this.ColumnHeaderStyle = this.FindResource("UCDataGridColumnHeaderStyle") as Style;
            //this.RowStyle = this.FindResource("UCDataGridRowStyle") as Style;
            //this.CellStyle = this.FindResource("UCDataGridCellStyle") as Style;
            //this.RowHeaderStyle = this.FindResource("UCDataGridRowHeaderStyle") as Style;

            this.RowDetailsVisibilityChanged += UCDataGrid_RowDetailsVisibilityChanged;
            this.SelectionChanged += UCDataGrid_SelectionChanged;

            this.Loaded += UCDataGrid_Loaded;

            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(expanderButtonClickEventHandler));

        }

        public void expanderButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            var btn = e.OriginalSource as Button;
            if (btn == null ||
                (!"ExpenderButton".Equals(btn.Name)))
            {
                return;
            }

            var row = this.ItemContainerGenerator.ContainerFromItem(btn.Tag) as DataGridRow;
            if (row != null)
            {
                if (row.DetailsVisibility == Visibility.Visible)
                {
                    row.DetailsVisibility = Visibility.Collapsed;
                }
                else
                {
                    row.DetailsVisibility = Visibility.Visible;
                }
            }
        }

        private void UCDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as DataGridRow;
            if (row != null)
            {
                row.Background = Brushes.LightGray;
                UpdateRowBackground();
            }
        }

        private void UCDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRowBackground();
        }
        /// <summary>
        /// 更新行和行详情面板背景色
        /// </summary>
        private void UpdateRowBackground()
        {
            int i = 0;
            foreach (var item in this.Items)
            {
                var row = this.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    if (!row.IsSelected)
                    {
                        row.Background = (i % 2 == 0 ? this.RowBackground : this.AlternatingRowBackground);
                    }
                    i++;
                    if (row.DetailsVisibility == Visibility.Visible)
                    {
                        var border = FindVisualChildByName<Border>(row, "DGR_DetailBorder");
                        border.Background = (i % 2 == 0 ? this.RowBackground : this.AlternatingRowBackground);
                        i++;
                    }

                }
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();




        }

        private void UCDataGrid_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
        {
            UpdateRowBackground();
        }

        public T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i) as DependencyObject;
                    string controlName = child.GetValue(Control.NameProperty) as string;
                    if (controlName == name)
                    {
                        return child as T;
                    }
                    else
                    {
                        T result = FindVisualChildByName<T>(child, name);
                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }


    }
}
