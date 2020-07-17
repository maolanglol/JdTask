using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jlion.BrushClient.Framework.Model
{
    public class BaseThread
    {
        public Thread thread;

        /// <summary>
        /// 开启线程
        /// </summary>
        public void Start()
        {
            if (thread == null)
                return;

            IsStop = false;
            if (thread.IsAlive)
            {
                return;
            }
            if (thread.ThreadState == ThreadState.Unstarted || thread.ThreadState == (ThreadState.Background | ThreadState.Unstarted))
            {
                thread.Start();
            }
            IsStop = false;
        }

        /// <summary>
        /// 停止线程
        /// </summary>
        public void Stop()
        {
            if (thread == null)
                return;

            thread.Abort();
            while (thread.ThreadState != System.Threading.ThreadState.Stopped && thread.ThreadState != System.Threading.ThreadState.Aborted)
            {
                Thread.Sleep(1);
            }
            thread = null;
            IsStop = true;
        }

        /// <summary>
        /// 中止线程
        /// </summary>
        public void Suspend()
        {
            IsStop = true;
        }


        public volatile bool IsStop;

        public bool IsAbort
        {
            get
            {
                if (thread == null)
                    return true;
                return thread.ThreadState == ThreadState.AbortRequested || thread.ThreadState == ThreadState.Aborted || thread.ThreadState == ThreadState.Stopped;
            }
        }
    }
}
