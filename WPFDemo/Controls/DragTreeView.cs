using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WPFDemo.Model;
using WPFDemo.Utility;

namespace WPFDemo.Controls
{

    public class DragTreeView : TreeView
    {
        #region Fields
        /// <summary>
        /// 当前需要移动的项
        /// </summary>
        private object _selectedItem = null;
        /// <summary>
        /// 记录鼠标按下的坐标
        /// </summary>
        private Point _mouseDownPoint;
        //控制装饰器的显示与隐藏
        private TreeViewItem _tempTreeViewItem = null;
        private Dictionary<TreeViewItem, UIElementAdorner> _tempAdornerDic = new Dictionary<TreeViewItem, UIElementAdorner>();
        /// <summary>
        /// 保存鼠标真正位于哪项TreeViewItem上面
        /// </summary>
        private TreeViewItem _mouseOverItem = null;
        #endregion

        #region Preperty      

        #endregion

        #region Constructors
        public DragTreeView()
        {
            //DefaultStyleKey = typeof(DuiTreeView);
            this.MouseMove += tv_MouseMove;
        }


        #endregion

        #region Override

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var ele = element as TreeViewItem;
            if (ele != null)
            {
                ele.AddHandler(TreeViewItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.TreeViewItem_MouseLeftButtonDown), true);
                ele.AddHandler(TreeViewItem.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.TreeViewItem_MouseLeftButtonUp), true);
                //ele.AddHandler(TreeViewItem.MouseRightButtonDownEvent, new MouseButtonEventHandler(this.TreeViewItem_MouseRightButtonDown), true);
                //ele.MouseDown += TreeViewItem_MouseRightButtonDown;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 判断upper是否是lower的上级
        /// </summary>
        /// <param name="upper"></param>
        /// <param name="lower"></param>
        /// <returns></returns>
        private bool IsHaveUpperAndLowerRelation(object upper, object lower)
        {
            var tmpNode = lower;
            while (true)
            {
                tmpNode = GetParrent(tmpNode);

                if (tmpNode == null)
                    return false;
                if (tmpNode.Equals(upper))
                    return true;
            }
        }

        private void TreeViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _selectedItem = (e.OriginalSource as FrameworkElement).DataContext;
            _mouseDownPoint = Mouse.GetPosition(null);
        }

        /// <summary>
        /// 判断目标是否可放置
        /// </summary>
        /// <param name="source"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool IsCanMove(object source, object obj)
        {
            if (source == null || obj == null)
                return false;

            if (source == null || source.Equals(obj))
                return false;

            //不能移动到自己的节点下 无意义
            if (source.Equals(obj))
                return false;

            //不能移动到自己的父节点 无意义
            var parrentNode = GetParrent(source);
            if (obj.Equals(parrentNode))
                return false;

            //父节点不能直接移动到子节点上
            if (IsHaveUpperAndLowerRelation(source, obj))
                return false;

            return true;
        }

        private void TreeViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseDownPoint == Mouse.GetPosition(null))
                return;
            var obj = (e.OriginalSource as FrameworkElement).DataContext;

            if (IsCanMove(_selectedItem, obj) == false)
                return;

            //移除原来的位置
            var oldParrent = GetParrent(_selectedItem);
            if (oldParrent != null)
            {
                var oldParrentModel = oldParrent as IDropable;
                if (oldParrentModel != null)
                {
                    oldParrentModel.Remove(_selectedItem);
                    //添加到新位置
                    var model = obj as IDropable;
                    if (model != null)
                    {
                        model.Add(_selectedItem);    
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定对象的父节点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object GetParrent(object obj)
        {
            foreach (var item in this.Items)
            {
                //若自己为根节点，则返回null
                if (item.Equals(obj))
                    return null;

                var tvi = this.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                var result = SearchParrentItem(obj, tvi);
                if (result != null)
                    return result;
            }
            return null;
        }


        /// <summary>
        /// 递归遍历所有项,并返回找到项TreeViewItem的父节点
        /// </summary>
        /// <param name="searchDataContext">需要查找的数据项</param>
        /// <param name="treeViewItem"></param>
        /// <returns></returns>
        private object SearchParrentItem(object searchDataContext, TreeViewItem treeViewItem)
        {
            if (searchDataContext == null || treeViewItem == null)
                return null;
            //if (searchDataContext.Equals(treeViewItem.DataContext))
            //{
            //    return treeViewItem;
            //}

            foreach (object item in treeViewItem.Items)
            {
                if (item.Equals(searchDataContext))
                    return treeViewItem.DataContext;

                var tvi = treeViewItem.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                var tv = SearchParrentItem(searchDataContext, tvi);
                if (tv == null)
                    continue;
                else
                    return tv;
            }
            return null;
        }


        private void tv_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDownPoint == Mouse.GetPosition(null))
                return;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Mouse.SetCursor(Cursors.UpArrow);
                //Mouse.SetCursor(new Cursor("C:\\Windows\\Cursors\\arrow_rm.cur"));

            }
            else
            {
                if (_tempTreeViewItem != null)
                {
                    HideAdorner(_tempTreeViewItem);
                }
                return;
            }

            _mouseOverItem = null;
            foreach (object item in this.Items)
            {
                var treeViewItem = this.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                SearchItem(treeViewItem);
            }
            MouseDragDeal(_mouseOverItem);
        }

        /// <summary>
        /// 拖拽item处理，设置各种鼠标操作状态和TreeViewItem显示状态
        /// </summary>
        private void MouseDragDeal(TreeViewItem tvi)
        {
            if (tvi != null && tvi.IsMouseOver)
            {
                var objRow = tvi.DataContext;
                if (objRow == null)
                    return;

                if (IsCanMove(_selectedItem, objRow) == false)
                    Mouse.SetCursor(Cursors.No);

                //为目标TreeViewItem添加装饰控件
                AddAdorner(tvi);
                if (_tempTreeViewItem != null && tvi.Equals(_tempTreeViewItem) == false)
                {
                    HideAdorner(_tempTreeViewItem);
                }
                _tempTreeViewItem = tvi;
            }
        }


        /// <summary>
        /// 遍历所有项，若已找到鼠标所在的项则设置_mouseOverItem,
        /// 最后一个设置的_mouseOverItem才是真正鼠标所在位置，
        /// 前面找到的可能只是父节点
        /// </summary>
        /// <param name="viewItem"></param>
        /// <returns></returns>
        private void SearchItem(TreeViewItem viewItem)
        {
            if (viewItem == null)
                return;
            if (viewItem.IsMouseOver)
            {
                _mouseOverItem = viewItem;
            }

            foreach (object item in viewItem.Items)
            {
                var treeViewItem = viewItem.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                SearchItem(treeViewItem);
            }
        }


        /// <summary>
        /// 遍历所有项,并返回找到项TreeViewItem
        /// </summary>
        /// <param name="viewItem"></param>
        /// <returns></returns>
        private TreeViewItem SearchItem(object dataContext, TreeViewItem treeViewItem)
        {
            if (dataContext == null || treeViewItem == null)
                return null;
            if (dataContext.Equals(treeViewItem.DataContext))
            {
                return treeViewItem;
            }

            foreach (object item in treeViewItem.Items)
            {
                var tvi = treeViewItem.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                var tv = SearchItem(dataContext, tvi);
                if (tv == null)
                    continue;
                else
                    return tv;
            }
            return null;
        }



        /// <summary>
        /// 显示装饰器
        /// </summary>
        /// <param name="viewItem"></param>
        private void AddAdorner(TreeViewItem viewItem)
        {
            if (_tempAdornerDic.ContainsKey(viewItem))
            {
                _tempAdornerDic[viewItem].ShowAdorner();
            }
            else
            {
                var ele = Util.Util.GetChildObject<ContentPresenter>(viewItem, "PART_Header");
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(ele);
                if (adornerLayer != null)
                {
                    var adorner = new UIElementAdorner(ele);
                    adornerLayer.Add(adorner);
                    _tempAdornerDic.Add(viewItem, adorner);
                }
            }
        }
        /// <summary>
        /// 隐藏装饰器
        /// </summary>
        /// <param name="viewItem"></param>
        private void HideAdorner(TreeViewItem viewItem)
        {
            if (_tempAdornerDic.ContainsKey(viewItem))
            {
                _tempAdornerDic[viewItem].HideAdorner();
            }
        }

        #endregion
    }
}
