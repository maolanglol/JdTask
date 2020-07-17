using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.Enums
{
    public partial class OptionEnums
    {
        /// <summary>
        /// 设置类型
        /// </summary>
        public enum SettingsType
        {
            /// <summary>
            /// 打印设置
            /// </summary>
            PrintSetting = 1,

            /// <summary>
            /// 读取金额
            /// </summary>
            ReadMoneySetting = 2,

            /// <summary>
            /// 收款设置
            /// </summary>
            ReceiveMoneySetting = 3,

            /// <summary>
            /// 快捷键
            /// </summary>
            HotKeySetting = 4,

            /// <summary>
            /// 自动入账
            /// </summary>
            AutoEntryAccount = 5
        }

        /// <summary>
        /// 读取金额类型
        /// </summary>
        public enum ReadMoneyType : uint
        {
            /// <summary>
            /// 串口读取
            /// </summary>
            SerialPort = 1,

            /// <summary>
            /// 虚拟串口
            /// </summary>
            VirSerial = 5,

            /// <summary>
            /// 窗口读取
            /// </summary>
            Form = 10,

            /// <summary>
            /// OCR读取
            /// </summary>
            Ocr = 15,
        }

        /// <summary>
        /// 窗口金额查找模式枚举
        /// </summary>
        public enum FormSearchType
        {
            /// <summary>
            /// 定位金额模式
            /// </summary>
            [Description("定位金额模式")]
            PositionMoney = 1,

            /// <summary>
            /// 查找金额模式
            /// </summary>
            [Description("查找金额模式")]
            SearchMoney = 2
        }

        /// <summary>
        /// 打印设置
        /// </summary>
        public enum PrintType
        {
            /// <summary>
            /// 驱动
            /// </summary>
            Drive = 1,

            /// <summary>
            /// 串口
            /// </summary>
            Serial = 10,

            /// <summary>
            /// 并口
            /// </summary>
            Parallel = 20,

            /// <summary>
            /// 网络
            /// </summary>
            Internet = 30
        }

        /// <summary>
        /// 入账规则
        /// </summary>
        public enum AccountRuleType
        {
            /// <summary>
            /// 统一入账方式
            /// </summary>
            UnityMode = 1,

            /// <summary>
            /// 按支付方式入账
            /// </summary>
            PayTypeMode = 2
        }

        /// <summary>
        /// 入账模式
        /// </summary>
        public enum AccountMode
        {
            /// <summary>
            /// 键盘模式
            /// </summary>
            Keyboard = 1,

            /// <summary>
            /// 鼠标模式
            /// </summary>
            Mouse = 2
        }


        /// <summary>
        /// 金额回填方式
        /// </summary>
        public enum AccountBackMoneyType
        {
            /// <summary>
            /// 键盘模式回填
            /// </summary>
            Keyboard = 1,

            /// <summary>
            /// 定位方式回填
            /// </summary>
            Point =2
        }

    }
}
