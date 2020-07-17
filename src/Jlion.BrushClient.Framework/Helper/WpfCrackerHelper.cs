using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jlion.BrushClient.Framework.Helper
{
    public class WpfCrackerHelper
    {
        private const string Data = "SOFTWARE\\DevExpress\\Components";
        private const string keys = "LastAboutShowedTime";
        private const string formate = "MM/dd/yyyy HH:mm:ss";
        private const string disable = "DisableSmartTag";
        private const string SmartTagWidth = "SmartTagWidth";

        private void SetDate()
        {
            this.CreateKey();
            var currentUser = Registry.CurrentUser;
            var subKey = currentUser.OpenSubKey("SOFTWARE\\DevExpress\\Components", true);
            subKey.GetValue("LastAboutShowedTime");
            string value = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            subKey.SetValue("LastAboutShowedTime", value);
            currentUser?.Dispose();
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        private void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        /// <summary>
        /// 创建键值
        /// </summary>
        private void CreateKey()
        {
            RegistryKey currentUser = Registry.CurrentUser;
            if (currentUser.OpenSubKey("SOFTWARE\\DevExpress\\Components", true) == null)
            {
                var registryKey = currentUser.CreateSubKey("SOFTWARE\\DevExpress\\Components");
                registryKey.CreateSubKey("LastAboutShowedTime").SetValue("LastAboutShowedTime", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                registryKey.CreateSubKey("DisableSmartTag").SetValue("LastAboutShowedTime", false);
                registryKey.CreateSubKey("SmartTagWidth").SetValue("LastAboutShowedTime", 350);
            }
            currentUser.Dispose();
        }

        /// <summary>
        /// 开始压缩内存，并执行黑科技
        /// </summary>
        /// <param name="sleepSpan"></param>
        public void Cracker(int sleepSpan = 30)
        {
            Task.Factory.StartNew(delegate
            {
                while (true)
                {
                    try
                    {
                        this.SetDate();
                        this.FlushMemory();
                        Thread.Sleep(TimeSpan.FromSeconds((double)sleepSpan));
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }

        ///// <summary>
        ///// 复制启动
        ///// </summary>
        ///// <param name="folderName">目标文件夹名称</param>
        ///// <param name="argument">启动参数</param>
        ///// <param name="SkipExtension">不复制的文件类型</param>
        //public void CopyRun(string folderName, string argument, params string[] SkipExtension)
        //{
        //    string currentDirectory = Directory.GetCurrentDirectory();
        //    Directory.EnumerateDirectories(currentDirectory);
        //    Directory.EnumerateFiles(currentDirectory);
        //    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    if (currentDirectory.Contains(folderPath) || argument != null)
        //    {
        //        return;
        //    }
        //    this.skipextension = SkipExtension;
        //    this.CopyDirectory(currentDirectory, folderPath, folderName);
        //    Process.Start(Path.Combine(folderPath, folderName, "预付费水表管理系统.exe"), "Argument");
        //    Process.GetCurrentProcess().Kill();
        //}

        //private void CopyDirectory(string srcdir, string desdir, string foldername = null)
        //{
        //    string text = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);
        //    if (foldername != null)
        //    {
        //        text = foldername;
        //    }
        //    string text2 = desdir + "\\" + text;
        //    if (desdir.LastIndexOf("\\") == desdir.Length - 1)
        //    {
        //        text2 = desdir + text;
        //    }
        //    string[] fileSystemEntries = Directory.GetFileSystemEntries(srcdir);
        //    for (int i = 0; i < fileSystemEntries.Length; i++)
        //    {
        //        string text3 = fileSystemEntries[i];
        //        if (Directory.Exists(text3))
        //        {
        //            string path = text2 + "\\" + text3.Substring(text3.LastIndexOf("\\") + 1);
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            this.CopyDirectory(text3, text2, null);
        //        }
        //        else
        //        {
        //            string text4 = text3.Substring(text3.LastIndexOf("\\") + 1);
        //            text4 = text2 + "\\" + text4;
        //            if (!Directory.Exists(text2))
        //            {
        //                Directory.CreateDirectory(text2);
        //            }
        //            FileInfo fileInfo = new FileInfo(text3);
        //            if (this.skipextension != null && this.skipextension.Contains(fileInfo.Extension))
        //            {
        //                if (!File.Exists(text4))
        //                {
        //                    File.Copy(text3, text4, true);
        //                }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    File.Copy(text3, text4, true);
        //                }
        //                catch (Exception)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
