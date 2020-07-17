using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Jlion.BrushClient.Framework.Helper
{
    public class ProcessHelper
    {
        /// <summary>
        /// 唤起其他程序
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool StartProcess(string path, string content,bool isRoot=true)
        {
            Process process = new Process();//创建进程对象    
            ProcessStartInfo startInfo = new ProcessStartInfo(path, content); // 括号里是(程序名,参数)
            process.StartInfo = startInfo;
            if(isRoot)
            {
                process.StartInfo.Verb = "runas";
            }
            process.Start();
            return true;
        }

        /// <summary>
		/// 以管理员权限打开
		/// </summary>
		public static void Request(string path)
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    Verb = "runas"
                });
            }
        }
    }
}
