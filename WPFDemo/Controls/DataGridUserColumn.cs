using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using WPFDemo.Converter;

namespace WPFDemo.Controls
{
    public class DataGridUserColumn : DataGridTemplateColumn
    {
        private FrameworkElementFactory _fefTextBlock;
        private string _bindingFiled;
        /// <summary>
        /// 获取或设置绑定字段
        /// </summary>
        public string BindingFiled
        {
            get
            {
                return _bindingFiled;
            }
            set
            {
                _bindingFiled = value;

                if (_fefTextBlock != null)
                {
                    //设置绑定字段
                    Binding txtBinding = new Binding();
                    txtBinding.Path = new PropertyPath(BindingFiled);
                    _fefTextBlock.SetBinding(TextBlock.TextProperty, txtBinding);
                }
            }
        }


        public DataGridUserColumn()
        {
            SetDataTemplate();
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var txt = Util.Util.FindVisualChildByName<TextBlock>(cell, "txt");
            return base.GenerateElement(cell, dataItem);
        }

        /// <summary>
        /// 设置数据模板
        /// </summary>
        private void SetDataTemplate()
        {
            DataTemplate dt = new DataTemplate();

            FrameworkElementFactory fefStackPanel = new FrameworkElementFactory(typeof(StackPanel));
            fefStackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            Binding marginBinding = new Binding();
            marginBinding.Path = new PropertyPath("Margin");
            fefStackPanel.SetBinding(StackPanel.MarginProperty, marginBinding);


            FrameworkElementFactory fefToggleButton = new FrameworkElementFactory(typeof(ToggleButton));
            fefToggleButton.SetValue(ToggleButton.ClickModeProperty, ClickMode.Press);
            fefToggleButton.SetValue(ToggleButton.StyleProperty, Application.Current.Resources["ExpandCollapseToggleStyle"]);
            fefStackPanel.AppendChild(fefToggleButton);
            Binding isCheckedBinding = new Binding();
            isCheckedBinding.Path = new PropertyPath("IsExpand");
            isCheckedBinding.Converter = new StringToBoolConvert();
            fefToggleButton.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);


            Binding visibilityBinding = new Binding();
            visibilityBinding.Path = new PropertyPath("IsHaveChild");
            visibilityBinding.Converter = new BoolToVisibilityHidConverter();
            fefToggleButton.SetBinding(ToggleButton.VisibilityProperty, visibilityBinding);


            FrameworkElementFactory fefTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            //Binding txtBinding = new Binding();
            //txtBinding.Path = new PropertyPath(BindingFiled);
            //fefTextBlock.SetBinding(TextBlock.TextProperty, txtBinding);
            fefStackPanel.AppendChild(fefTextBlock);
            _fefTextBlock = fefTextBlock;

            dt.VisualTree = fefStackPanel;
            this.CellTemplate = dt;
        }
    }
}
