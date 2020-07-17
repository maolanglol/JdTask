using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model
{
    public class PagerBaseRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 每页请求梳理
        /// </summary>
        public int Rows { set; get; }
    }
}
