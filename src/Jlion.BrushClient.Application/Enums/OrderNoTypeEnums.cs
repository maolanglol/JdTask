using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Application.Enums
{
    /// <summary>
    /// 订单号类型
    /// </summary>
    public enum OrderNoTypeEnums
    {
        /// <summary>
        /// 业务订单号
        /// </summary>
        BussinessNo = 0,

        /// <summary>
        /// 子订单号
        /// </summary>
        SubOrderNo = 1,

        /// <summary>
        /// 商户订单号
        /// </summary>
        MchOrderNo = 2,

        /// <summary>
        /// 扩展订单号
        /// </summary>
        ExtendOrderNo = 3,

        /// <summary>
        /// 平台方订单号
        /// </summary>
        PlatformOrderNo = 4,

        /// <summary>
        /// 银行订单号
        /// </summary>
        BankOrderNo = 5,

        /// <summary>
        /// 原始支付订单号
        /// </summary>
        SourceOrderNo = 6
    }
}
