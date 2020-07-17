using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeActionModel
    {
        /// <summary>
        /// 方法
        /// </summary>
        public Func<object, DateTime, bool> Action { set; get; }

        /// <summary>
        /// 执行梳理
        /// </summary>
        public int ExecuteCount { set; get; }

        /// <summary>
        /// 消息
        /// </summary>
        public object Message { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time { set; get; }
    }

    public class TimeAcionSecondModel
    {
        /// <summary>
        /// 方法
        /// </summary>
        public Func<bool> Action { set; get; }

        /// <summary>
        /// (毫秒级别执行)
        /// </summary>
        public int Millisecond { set; get; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime Time { set; get; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public long Count { set; get; }
    }
}
