using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using WP.Device.Plugins.Application.Enums;
using WP.Device.Plugins.Framework;
using WP.Device.Plugins.Framework.Helper;
using WP.Device.Plugins.Kernel;

namespace WP.Device.Plugins.Application.Plugins
{
    public class OnToolPlugins : OnBasePlugins
    {
        public OnToolPlugins()
        {

        }

        /// <summary>
        /// 获取本地所有打印机驱动名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetDriveNames()
        {
            var result = new List<string>();
            var list = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
            foreach (var item in list)
            {
                result.Add(item.ToString());
            }
            return result;
        }

        /// <summary>
        /// 获得系统串口列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSerialPortNames()
        {
            var array = SerialPort.GetPortNames();
            return array?.ToList();
        }

        /// <summary>
        /// 获得并口数据,这里不通过系统的api获取,由于系统的设备管理服务可能会被禁用，故写死几个并口数据
        /// Get the system devices information with windows api.
        /// </summary>
        /// <param name="hardType">Device type.</param>
        /// <param name="propKey">the property of the device.</param>
        /// <returns></returns>
        public List<string> GetParallelPortNames()
        {
            return new List<string>() {
                "LPT1",
                "LPT2",
                "LPT3",
                "LPT4",
                "LPT5"
            };
        }

        /// <summary>
        /// 获得串口波特率
        /// </summary>
        /// <returns></returns>
        public List<int> GetRates()
        {
            return new List<int>() {
                300,
                600,
                1200,
                2400,
                4800,
                9600,
                19200,
                38400,
                43000,
                56000,
                57600,
                115200
            };
        }

        /// <summary>
        /// 网络ping
        /// </summary>
        /// <returns></returns>
        public bool PingInternet()
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send("www.baidu.com", 1000);
                return pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success;
            }
            catch (Exception ex)
            {
                TextHelper.Error("PingInternet 异常", ex);
                return false;
            }
        }

        /// <summary>
        /// 检测网络
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public bool PingInternet(string host)
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send(host, 1000);
                return pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success;
            }
            catch (Exception ex)
            {
                TextHelper.Error("PingInternet 异常", ex);
                return false;
            }
        }

        /// <summary>
        /// 获得设备的唯一编码
        /// </summary>
        /// <returns></returns>
        public string GetDeviceKey()
        {
            var machineGuid = Win32RegistryHelper.ReadLocalMachineKey(ConstDefintion.WindowMachinePath, ConstDefintion.WindowMachineName);
            return machineGuid.RemoveChar('-');
        }
    }
}
