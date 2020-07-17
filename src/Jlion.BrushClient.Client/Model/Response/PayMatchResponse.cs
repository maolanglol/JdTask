using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Jlion.BrushClient.Client.Enums.OptionEnums;

namespace Jlion.BrushClient.Client.Model.Response
{
    public class PayMatchResponse
    {
        /// <summary>
        /// 支付类型
        /// </summary>
        public EnumPayType PayType;

        /// <summary>
        /// 是否退款
        /// </summary>
        public bool IsRefund { set; get; }
    }
}
