using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model
{
    public class RefundContentRequest : BasePrinterContentRequest
    {
        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal RefundAmount { set; get; }
    }
}
