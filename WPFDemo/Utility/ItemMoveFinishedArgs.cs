using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WPFDemo.Utility
{
    /// <summary>
    /// TreeViewItem移动完成事件参数
    /// </summary>
    public class ItemMoveFinishedArgs : EventArgs
    {
        public DataRow OldValue { get; set; }
        public DataRow NewValue { get; set; }
    }
}
