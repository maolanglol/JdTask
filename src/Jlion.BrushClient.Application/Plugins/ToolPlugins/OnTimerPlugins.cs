using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Jlion.BrushClient.Application.Model;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Application.Plugins
{
    /// <summary>
    /// 定时执行
    /// </summary>
    public class OnTimerPlugins : OnBasePlugins
    {
        #region 属性定义
        ConcurrentDictionary<string, TimeAcionSecondModel> _queueMap;
        List<string> _removedKeys;

        #endregion

        public OnTimerPlugins()
        {
            _queueMap = new ConcurrentDictionary<string, TimeAcionSecondModel>();
            _removedKeys = new List<string>();

            Start();
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExist(string key)
        {
            return _queueMap.Keys.Contains(key);
        }

        public void ClearAll()
        {
            _queueMap?.Clear();
        }

        /// <summary>
        /// 添加到检查队列
        /// </summary>
        /// <param name="msgId"></param>
        public void Add(string key, Func<bool> action, int millisecond, DateTime dateTime,int count=0)
        {
            if (IsExist(key))
                return;

            var val = new TimeAcionSecondModel()
            {
                Action = action,
                Millisecond = millisecond,
                Time = dateTime,
                Count = count,
            };

            _queueMap.AddOrUpdate(key, val, (k, v) => val);
        }

        /// <summary>
        /// 启动
        /// </summary>
        private void Start()
        {
            // 已经初始化过，就直接返回
            if (!SetTimer())
                return;

            aTimer.Start();
        }

        #region 定时方法
        private Timer aTimer;
        private int interval = 1000;
        private readonly object loker = new object();

        /// <summary>
        /// 设置定时器
        /// </summary>
        private bool SetTimer()
        {
            if (aTimer != null)
                return false;
            lock (loker)
            {
                if (aTimer != null)
                    return false;
                aTimer = new Timer
                {
                    Interval = interval
                };
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.AutoReset = true;//每到指定时间Elapsed事件是到时间就触发
                aTimer.Enabled = true; //指示 Timer 是否应引发 Elapsed 事件。
                return true;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                foreach (var item in _queueMap)
                {
                    if ((item.Value?.Action ?? null) == null)
                        continue;

                    var time = item.Value.Millisecond;
                    if ((DateTime.Now - item.Value.Time).TotalMilliseconds < time && item.Value.Count > 0)
                        continue;

                    var bl = item.Value.Action.Invoke();
                    item.Value.Count = 1;
                    item.Value.Time = DateTime.Now;//重置时间
                    if (bl)
                    {
                        _queueMap.TryRemove(item.Key, out TimeAcionSecondModel value);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error("OnTimerPlugins OnTimedEvent 异常", ex);
            }
        }
        #endregion
    }


}
