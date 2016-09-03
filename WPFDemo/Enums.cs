using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo
{
    /// <summary>
    /// button样式类型
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 信息
        /// </summary>
        Info,
        /// <summary>
        /// 告警
        /// </summary>
        Warning,
        /// <summary>
        /// 危险
        /// </summary>
        Danger,
        /// <summary>
        /// 链接
        /// </summary>
        Link
    }
    /// <summary>
    /// Button尺寸类型
    /// </summary>
    public enum ButtonSizeType
    {
        /// <summary>
        /// 自定义
        /// </summary>
        Custom,
        /// <summary>
        /// 小
        /// </summary>
        Small,
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 大
        /// </summary>
        Large
    }
}
