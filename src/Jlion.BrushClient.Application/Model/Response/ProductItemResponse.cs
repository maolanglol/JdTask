using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class ProductItemResponse
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { set; get; }

        /// <summary>
        /// 天数
        /// </summary>
        public int Day { set; get; }

        public string Description
        {
            get
            {
                return $"享受{Day}天{Price}元会员权限";
            }
        }

        public string Color
        {
            set;get;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }
    }
}
