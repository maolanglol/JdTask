using System;
using System.Collections.Generic;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class TaskComplateResponse
    {
        /// <summary>
        /// 当前任务获得多少金额
        /// </summary>
        public decimal Amount { set; get; }

        /// <summary>
        /// 目前总余额
        /// </summary>
        public decimal TotalAmount { set; get; }
    }
}
