using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client.OnRender
{
    public class TimeKeyStatics
    {
        public static string PaySuccessResultBox(string outTradeNo)
        {
            return $"paySuccessResult_{outTradeNo}";
        }

        public static string PayingResultBox(string outTradeNo)
        {
            return $"payingResult_{outTradeNo}";
        }

        public static string TipKey()
        {
            return $"tip_{Guid.NewGuid().ToString().RemoveChar('-')}";
        }

        public static string InternetKey(string key)
        {
            return $"internet_{key}";
        }

        public static string TaskKey(string key)
        {
            return $"task_{key}";
        }

        public static string ToolKey()
        {
            return $"tool_page_check";
        }

        public static string TopMostKey(string key)
        {
            return $"most_{key}";
        }

        public static string Printer(string key)
        {
            return $"printer_{key}";
        }
    }
}
