using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Framework.Helper
{
    /// <summary>
    /// 注册表操作帮助类
    /// </summary>
    public class Win32RegistryHelper
    {
        ///// <summary>
        ///// 读取注册表信息数据
        ///// </summary>
        ///// <param name="path">注册表路径</param>
        ///// <param name="name">注册表中的项</param>
        ///// <returns></returns>
        //public static string ReadLocalMachineKey(string path, string name)
        //{

        //    try
        //    {
        //        var rk = Registry.LocalMachine.OpenSubKey(path, true);
        //        var info = rk.GetValue(name);
        //        return info.ToString();
        //    }
        //    catch(Exception ex)
        //    {
        //        TextHelper.Error("ReadLocalMachineKey 异常",ex);
        //        return "";
        //    }
        //}
        
        /// <summary>
        /// 写入用户注册表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void WriteUserKey(string path, string name, string value)
        {

            var rk = Registry.CurrentUser.OpenSubKey(path, true);
            var info = rk.GetValue(name);
            if (info.ToInt32OrDefault(0)<=5000)
            {
                rk.SetValue(name, value);
            }
        }

        public static string ReadUserKey(string path, string name)
        {
            var rk = Registry.CurrentUser.OpenSubKey(path, true);
            var info = rk.GetValue(name);
            return info?.ToString()??"";
        }
    }
}
