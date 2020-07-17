using Jlion.BrushClient.Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    /// <summary>
    /// 用户账户表
    /// </summary>
    public class AccountItemResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// 用户ID 包含代理商的Id
        /// </summary>
        public long UserId { set; get; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 支付宝提现账号
        /// </summary>
        public string AlipayAccount { set; get; }

        /// <summary>
        /// 可用余额
        /// </summary>
        public decimal Balance { set; get; }

        /// <summary>
        /// 冻结余额
        /// </summary>
        public decimal Freeze { set; get; }

        /// <summary>
        /// 提现用户类型
        /// </summary>
        public int Type { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ExpirDate { set; get; }

        public string ExpirDateName
        {
            get
            {
                return ExpirDate.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 是否是会员
        /// </summary>
        public string MemberName
        {
            get
            {
                return IsMember ? "会员" : "普通用户";
            }
        }

        public bool IsMember { set; get; }
    }
}
