using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NPoco;

namespace NPocoDemo
{
    /// <summary>
    /// NPoco辅助类
    /// 使用KeepConnectionAlive=true后需要手动调用KeepConnectionAlive=false然后再执行销毁方法
    /// </summary>
    public class NPocoHelper
    {
        /// <summary>
        /// 获取一个NPoco连接
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <param name="type">数据库类型</param>
        /// <param name="isolationLevel">事务所行为</param>
        /// <returns></returns>
        public static NPoco.Database GetNewInstance(string connStr, DatabaseType type = null, System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted)
        {
            if (type == null)
                type = DatabaseType.SqlServer2012;
            return new Database(connStr, DatabaseType.SqlServer2012, isolationLevel);
        }

        ///// <summary>
        ///// 获取一个NPoco连接
        ///// </summary>
        ///// <param name="connectionStringName">数据库连接配置名称</param>
        ///// <param name="isolationLevel">事务所行为</param>
        ///// <returns></returns>
        //public static NPoco.Database GetNewInstance(string connectionStringName)
        //{
        //    return new Database(connectionStringName);
        //}

        /// <summary>
        /// 获取一个NPoco连接
        /// </summary>
        /// <param name="connect">数据库连接</param>
        /// <returns></returns>
        public static NPoco.Database GetNewInstance(DbConnection connect)
        {
            return new Database(connect);
        }
    }
}
