using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class UserItemResponse
    {
        public string RealName { set; get; }

        public string Mobile { set; get; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { set; get; }

        /// <summary>
        /// 冻结余额
        /// </summary>
        public decimal Freeze { set; get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirDate { set; get; }
    }
}
