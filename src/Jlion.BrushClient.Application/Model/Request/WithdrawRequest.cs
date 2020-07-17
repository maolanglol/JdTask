using System;
using System.Collections.Generic;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class WithdrawRequest : BaseRequest
    {
        public string Name { set; get; }

        /// <summary>
        /// 提现支付宝账号
        /// </summary>
        public string AlipayAccount { set; get; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { set; get; }
    }
}
