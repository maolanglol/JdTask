using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model
{
    public class CardOrderRequest : PagerBaseRequest
    {
        public int CashId { set; get; }

        public int Type { set; get; }

        public DateTime StartTime { set; get; } = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");

        public DateTime EndTime { set; get; } = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");

        public string OrderNo { set; get; }

        public string PhoneOrCardNo { set; get; }

        public int StoresId { set; get; }
    }
}
