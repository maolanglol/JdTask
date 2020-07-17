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
    /// 支付订单查询定时器
    /// </summary>
    public class OnTimerQueuePlugins : OnBasePlugins
    {
        #region 属性定义
        ConcurrentDictionary<string, TimeActionModel> _queueMap;
        List<string> _removedKeys;

        /// <summary>
        /// 执行的阶梯毫秒数（后续可配置）
        /// </summary>
        private int[] initervalLadder = { 1000, 3000, 5000, 8000, 15000, 30000, 60000 };
        /// <summary>
        /// 最大执行次数(后续可配置方式)
        /// </summary>
        private int maxExecuteCount = 7;
        #endregion

        public OnTimerQueuePlugins()
        {
            _queueMap = new ConcurrentDictionary<string, TimeActionModel>();
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

        /// <summary>
        /// 添加到检查队列
        /// </summary>
        /// <param name="msgId"></param>
        public void Add(string key, Func<object,DateTime, bool> action, object message, DateTime dateTime, int executeCount = 0)
        {
            if (IsExist(key))
                return;

            var val = new TimeActionModel()
            {
                Action = action,
                ExecuteCount = executeCount,
                Message = message,
                Time = dateTime
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
                var list = _queueMap.Where(oo => oo.Value.ExecuteCount < maxExecuteCount);
                foreach (var item in list)
                {
                    if ((item.Value?.Action ?? null) == null)
                        continue;

                    var time = initervalLadder[item.Value.ExecuteCount];
                    if ((DateTime.Now - item.Value.Time).TotalMilliseconds < time)
                        continue;


                    var bl = item.Value.Action.Invoke(item.Value.Message,item.Value.Time);
                    if (bl)
                    {
                        _queueMap.TryRemove(item.Key, out TimeActionModel value);
                        continue;
                    }

                    item.Value.ExecuteCount++;
                    if (item.Value.ExecuteCount >= maxExecuteCount)
                    {
                        _queueMap.TryRemove(item.Key, out TimeActionModel value);
                    }
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error("OnTimerQueuePlugins OnTimedEvent 异常", ex);
            }
        }
        #endregion
    }


}
