using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model
{
    public class NotifyDataRequest
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }
    }
}
