using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model.Response
{
    /// <summary>
    /// 商户信息
    /// </summary>
    public class MerchantResponse
    {
        /// <summary>
        /// 收银员id
        /// </summary>
        public int CashId { get; set; }

        /// <summary>
        /// 收银员名字
        /// </summary>
        public string CashName { get; set; }

        /// <summary>
        /// 门店id
        /// </summary>
        public int StoresId { get; set; }

        /// <summary>
        /// 门店名字
        /// </summary>
        public string StoresName { get; set; }

        /// <summary>
        /// 商户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 商户名字
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否是OEM商户
        /// </summary>
        public bool IsOem { get; set; }
    }
}
