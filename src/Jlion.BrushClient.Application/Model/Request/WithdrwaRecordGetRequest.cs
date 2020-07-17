using Jlion.BrushClient.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class WithdrwaRecordGetRequest:BaseRequest
    {
        public int Page { set; get; }

        public int Rows { set; get; }

        public EnumAccountType Types { set; get; }

        public EnumWithdrawStatus Status { set; get; }

        public DateTime GteTime { set; get; }

        public DateTime LteTime { set; get; }

    }
}
