using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace WPFDemo.Controls
{
    [ContentProperty("Items")]
    [TemplatePart(Name = PART_CONTENTBUTTON, Type = typeof(UCButton))]
    [TemplatePart(Name = PART_DOWNBUTTON, Type = typeof(UCButton))]
    public class UCSepButton : Control
    {
        #region Fileds
        private const string PART_CONTENTBUTTON = "PART_ContentButton";
        private const string PART_DOWNBUTTON = "PART_DownButton";
        private const string PART_ITEMS = "PART_Items";
        private UCButton _downButton = null;
        private ContextMenu _menu = new ContextMenu();
        #endregion

        #region Preperty
        private UCButton _contentButton = null;
        public Button ContentButton
        {
            get
            {
                return _contentButton;
            }
        }

        /// <summary>
        ///Button控件样式类型
        /// </summary>
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType", typeof(ButtonType), typeof(UCSepButton),
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
        /// Button控件尺寸大小类型
        /// </summary>
        public static readonly DependencyProperty ButtonSizeTypeProperty =
            DependencyProperty.Register("ButtonSizeType", typeof(ButtonSizeType), typeof(UCSepButton),
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UCSepButton));

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

                if (_contentButton != null && _downButton != null)
                {
                    _contentButton.CornerRadius = new CornerRadius(value.TopLeft, 0, 0, value.BottomLeft);
                    _downButton.CornerRadius = new CornerRadius(0, value.TopRight, value.BottomRight, 0);
                }
            }
        }

        private string _showText = string.Empty;
        public string ShowText
        {
            set
            {
                _showText = value;
                if (ContentButton != null)
                    ContentButton.Content = value;
            }
            get
            {
                return _showText;
            }
        }

        public event RoutedEventHandler _click;

        public event RoutedEventHandler Click
        {
            add
            {
                _click += value;
                if (ContentButton != null)
                    ContentButton.Click += value;
            }
            remove
            {
                _click -= value;
                if (ContentButton != null)
                    ContentButton.Click -= value;
            }

        }

        public ItemsCollection<UIElement> Items
        {
            get; private set;
        }
        #endregion

        #region Constructors
        public UCSepButton()
        {
            DefaultStyleKey = typeof(UCSepButton);

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            Loaded += UCSepButton_Loaded;

            Items = new ItemsCollection<UIElement>();
            Items.ItemsChange += Items_ItemsChange;
        }

        #endregion

        #region Events
        private void Items_ItemsChange(object sender, EventArgs e)
        {
            _menu.Items.Clear();
            foreach (var item in this.Items)
            {
                if (item is MenuItem)
                {
                    if (Application.Current.Resources.Contains("UCSepButton.MenuItem"))
                    {
                        (item as MenuItem).Style = Application.Current.Resources["UCSepButton.MenuItem"] as Style;
                    }
                }
                else if (item is Separator)
                {
                    (item as Separator).Margin = new Thickness(-25, 0, 5, 0);
                }
                _menu.Items.Add(item);
            }
        }

        private void UCSepButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (_downButton != null)
            {
                _downButton.Click -= _downButton_Click; //防止主题切换时注册两次，无法打开弹出菜单
                _downButton.Click += _downButton_Click;
            }
        }

        private void _downButton_Click(object sender, RoutedEventArgs e)
        {
            _menu.IsOpen = !_menu.IsOpen;
        }
        #endregion

        #region Override 
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentButton = GetTemplateChild(PART_CONTENTBUTTON) as UCButton;
            _downButton = GetTemplateChild(PART_DOWNBUTTON) as UCButton;
            InitContextMenu();

            if (_contentButton != null)
            {
                _contentButton.Content = ShowText;

                if (_click != null)
                    _contentButton.Click += _click;
            }

            if (_contentButton != null && _downButton != null)
            {
                _contentButton.CornerRadius = new CornerRadius(CornerRadius.TopLeft, 0, 0, CornerRadius.BottomLeft);
                _downButton.CornerRadius = new CornerRadius(0, CornerRadius.TopRight, CornerRadius.BottomRight, 0);
            }
        }
        #endregion

        #region Medthods
        private void InitContextMenu()
        {
            if (Application.Current.Resources.Contains("UCSepButton.ContextMenu"))
            {
                var style = Application.Current.Resources["UCSepButton.ContextMenu"] as Style;
                _menu.Style = style;
            }
            _menu.PlacementTarget = _contentButton;
            _menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            _menu.StaysOpen = false;
            //向下偏移一段距离
            _menu.VerticalOffset += 5;
        }

        #endregion
    }

    public class ItemsCollection<T> : Collection<T>
    {
        public event EventHandler<EventArgs> ItemsChange;

        public new void Add(T item)
        {
            base.Add(item);
            if (ItemsChange != null)
                ItemsChange(this, null);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            if (ItemsChange != null)
                ItemsChange(this, null);
        }
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            if (ItemsChange != null)
                ItemsChange(this, null);
        }
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            if (ItemsChange != null)
                ItemsChange(this, null);
        }
        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);
            if (ItemsChange != null)
                ItemsChange(this, null);
        }
    }
}
