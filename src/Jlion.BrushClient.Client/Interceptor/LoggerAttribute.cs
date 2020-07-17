using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Interceptor
{
    public class LoggerAttribute : Attribute
    {
        public string BusinessName;

        public LoggerAttribute()
        {
        }
    }
}
