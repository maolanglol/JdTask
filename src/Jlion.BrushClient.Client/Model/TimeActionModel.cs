using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model
{
    /// <summary>
    /// 队列定时器Action 模型
    /// </summary>
    public class TimeActionModel
    {
        /// <summary>
        /// 方法
        /// </summary>
        public Func<object,bool> Action { set; get; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int ExecuteCount { set; get; }

        /// <summary>
        /// 消息
        /// </summary>
        public object Message{ set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time { set; get; }
    }
}
