using System;
using System.Collections.Generic;
using System.Text;

namespace Jlion.BrushClient.Application.Model
{
    public class PagerRequest
    {
        public int Page { set; get; }

        public int Rows { set; get; }

        public bool IsResultTotal { set; get; }
    }
}
