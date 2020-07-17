using Jlion.BrushClient.Application.Enums;
using Jlion.BrushClient.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class WithdrawRecordResponse
    {

        /// <summary>
        ///提现记录Id
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// 提现用户Id
        /// </summary>
        public long AccountId { set; get; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { set; get; }

        public string AmountString {
            get {
                if (Status == EnumWithdrawStatus.Auditing
                    ||Status== EnumWithdrawStatus.Pass
                    ||Status== EnumWithdrawStatus.Success)
                {
                    return $"-{Amount.ToString("0.00")}";
                }
                return $"+{Amount.ToString("0.00")}";
            }
        }

        /// <summary>
        /// 扣除的手续费
        /// </summary>
        public decimal HandleFee { set; get; }

        public string HandleFeeString
        {
            get
            {
                if (Status == EnumWithdrawStatus.Auditing
                    || Status == EnumWithdrawStatus.Pass
                    || Status == EnumWithdrawStatus.Success)
                {
                    return $"-{HandleFee.ToString("0.00")}";
                }
                return $"+{HandleFee.ToString("0.00")}";
            }
        }

        /// <summary>
        /// 提现用户类型
        /// </summary>
        public EnumAccountType Type { set; get; }

        /// <summary>
        /// 提现状态
        /// </summary>
        public EnumWithdrawStatus Status { set; get; }

        public string StatusName
        {
            get
            {
                return Status.GetDescription();
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        public string CreateTimeName
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { set; get; }
    }
}
