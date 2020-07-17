using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Model.Request
{
    public class HotKeyProcRequest
    {
        public int Msg { set; get; }

        public IntPtr WParam { set; get; }
    }
}
