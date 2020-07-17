using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model
{
    /// <summary>
    /// 单品营销实体
    /// </summary>
    public class ProductMarketingsRequest
    {
        /// <summary>
        /// 商品条码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
