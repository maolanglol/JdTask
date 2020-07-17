using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Jlion.BrushClient.Application.Model;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client.Helper
{
    /// <summary>
    /// 键盘获取数据定时器
    /// </summary>
    public class TimerKeyboardHelper
    {
        #region 属性定义
        private KeyBoardResult _keyValue;

        private static readonly object _lock = new object();
        private Action<KeyBoardResult> action = null;

        public KeyBoardResult KeyValue
        {
            get
            {

                lock (_lock)
                {
                    if (_keyValue != null)
                        return _keyValue;

                    _keyValue = new KeyBoardResult();
                    return _keyValue;
                }
            }
        }
        #endregion

        public TimerKeyboardHelper(Action<KeyBoardResult> action)
        {
            this.action = action;
            Start();
        }

        public static TimerKeyboardHelper GetInstance(Action<KeyBoardResult> action)
        {
            return new TimerKeyboardHelper(action);
        }

        #region 内容处理公共方法
        /// <summary>
        /// 追加内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool InitValue(string value)
        {
            this.KeyValue.Value = value;
            this.KeyValue.StareMoney = value;
            this.KeyValue.DateTime = DateTime.Now;
            return true;
        }

        /// <summary>
        /// 插入内容
        /// </summary>
        /// <param name="value"></param>
        /// <param name="selectStart">9999表示最后追加</param>
        /// <returns></returns>
        public bool InsertValue(string value, int selectStart = 9999,int selectLength=1)
        {
            if (string.IsNullOrEmpty(this.KeyValue?.Value ?? ""))
            {
                this.KeyValue.Value = value;
                this.KeyValue.SelectStart = 1;
                return true;
            }
            if (this.KeyValue.Value.Equals("0"))
            {
                if (value.Equals("."))
                {
                    this.KeyValue.Value = this.KeyValue.Value + value;
                    this.KeyValue.SelectStart = 2;
                    return true;
                }
                this.KeyValue.Value = value;
                this.KeyValue.SelectStart = 1;
                return true;
            }
            if (selectStart == 9999)
            {
                this.KeyValue.Value = this.KeyValue.Value + value;
                this.KeyValue.SelectStart = (this.KeyValue.Value?.Length ?? 1) - 1;
                return true;
            }
            if (!this.KeyValue.IsComplete)
            {
                selectStart = this.KeyValue.SelectStart;
            }
            if ((this.KeyValue.Value?.Length ?? 0) >= selectStart)
            {
                this.KeyValue.Value = this.KeyValue.Value.Insert(selectStart, selectLength,value);
                //if (selectLength > 0)
                //{
                //    //this.KeyValue.Value=this.KeyValue.Value
                //}
                //this.KeyValue.Value = this.KeyValue.Value.Insert(selectStart, value);
            }
            else
            {
                this.KeyValue.Value = (this.KeyValue?.Value ?? "") + value;
                selectStart = 1;
            }
            this.KeyValue.IsComplete = false;
            this.KeyValue.DateTime = DateTime.Now;
            this.KeyValue.SelectStart = selectStart + 1;
            return true;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this.KeyValue.Value = this.KeyValue.StareMoney;
            this.KeyValue.SelectStart = 0;
            this.KeyValue.IsComplete = true;
        }

        public void Clear(string value)
        {
            this.KeyValue.Value = value;
            this.KeyValue.SelectStart = 0;
            this.KeyValue.IsComplete = true;
        }

        /// <summary>
        /// 删除,相当于按Delete键
        /// </summary>
        /// <param name="startIndex"></param>
        public void Delete(int startIndex)
        {
            if (string.IsNullOrEmpty(this.KeyValue?.Value ?? ""))
            {
                return;
            }
            if (startIndex == 9999)
            {
                this.KeyValue.Value = this.KeyValue.Value.Remove(this.KeyValue.Value.Length - 1);
                return;
            }
            if (this.KeyValue.Value.Length> startIndex)
            {
                this.KeyValue.Value = this.KeyValue.Value.Remove(startIndex, 1);
                var money = this.KeyValue.Value.ToDecimalOrDefault(0);
                if (money == 0)
                {
                    this.KeyValue.Value = "0";
                    this.KeyValue.SelectStart = 1;
                    return;
                }
                this.KeyValue.SelectStart = startIndex;
            }
        }

        /// <summary>
        /// 删除,相当于按Backspace键
        /// </summary>
        /// <param name="startIndex"></param>
        public void DeleteBack(int startIndex)
        {
            if (string.IsNullOrEmpty(this.KeyValue?.Value ?? ""))
            {
                return;
            }
            if (startIndex == 9999)
            {
                this.KeyValue.Value = this.KeyValue.Value.Remove(this.KeyValue.Value.Length - 1);
                return;
            }
            if (startIndex > 0 && this.KeyValue.Value.Length > (startIndex - 1))
            {
                this.KeyValue.Value = this.KeyValue.Value.Remove(startIndex - 1, 1);
                var money = this.KeyValue.Value.ToDecimalOrDefault(0);
                if (money == 0)
                {
                    this.KeyValue.Value = "0";
                    this.KeyValue.SelectStart = 1;
                    return;
                }
                this.KeyValue.SelectStart = startIndex - 1;
            }
        }
        #endregion

        #region 定时器
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
        private int interval = 500;
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
                this.action(this.KeyValue);
            }
            catch (Exception ex)
            {
                TextHelper.Error("TimerKeyboardHelper OnTimedEvent 异常", ex);
            }
        }
        #endregion
        #endregion
    }

    public class KeyBoardResult
    {
        public string Value { set; get; }

        /// <summary>
        /// 最初的金额
        /// </summary>
        public string StareMoney { set; get; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplete
        {
            set; get;
        }

        public DateTime DateTime { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int SelectStart { set; get; }

        /// <summary>
        /// 选中的长度
        /// </summary>
        public int SelectLength { set; get; }
    }

}
