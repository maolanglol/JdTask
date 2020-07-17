using Jlion.BrushClient.Application.Enums;
using Jlion.BrushClient.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    /// <summary>
    /// 资金流水记录表
    /// </summary>
    public class AccountRecordResponse
    {
        /// <summary>
        ///资金记录Id
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// 提现用户Account Id
        /// </summary>
        public long AccountId { set; get; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { set; get; }

        public string AmountString
        {
            get
            {
                if (Type == EnumAccountRecordType.Commission || Type == EnumAccountRecordType.Task||Type== EnumAccountRecordType.WithdrawFail)
                    return $"+{Amount.ToString("0.00")}";
                else
                    return $"-{Amount.ToString("0.00")}";
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
                if (Type == EnumAccountRecordType.Commission || Type == EnumAccountRecordType.Task)
                    return $"+{HandleFee.ToString("0.00")}";
                else
                    return $"-{HandleFee.ToString("0.00")}";
            }
        }

        /// <summary>
        /// 原始金额
        /// </summary>
        public decimal OriginalAmount { set; get; }

        /// <summary>
        /// 流水记录类型
        /// </summary>
        public EnumAccountRecordType Type { set; get; }

        public string TypeName
        {
            get
            {
                return Type.GetDescription();
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        public string CreateTimeString
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
