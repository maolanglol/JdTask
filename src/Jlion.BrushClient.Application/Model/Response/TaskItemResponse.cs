using System;
using System.Collections.Generic;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class TaskItemResponse
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public int TaskId { set; get; }

        /// <summary>
        /// 任务关键字
        /// </summary>
        public string SearchKey { set; get; }
    }
}
