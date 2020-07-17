using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class AccountRecordRequest: BaseRequest
    {
        public int Page { set; get; }

        public int Rows { set; get; }

        public int Type { set; get; }

        public DateTime GteTime { set; get; }

        public DateTime LteTime { set; get; }

    }
}
