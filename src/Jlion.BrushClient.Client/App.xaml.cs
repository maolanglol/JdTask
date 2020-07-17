using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.Framework.Helper;

namespace Jlion.BrushClient.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : System.Windows.Application
    {
        System.Threading.Mutex mutex;
        public App()
        {

            this.Startup += new StartupEventHandler(App_Startup);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            //mutex = new System.Threading.Mutex(true, "Jlion.BrushClient.Client", out bool ret);
            //if (!ret)
            //{
            //    MessageBox.Show("已有一个程序实例运行");
            //    Environment.Exit(0);
            //}
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            try
            {
                base.OnStartup(e);

                //UI线程未捕获异常处理事件
                this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
                //Task线程内未捕获异常处理事件
                TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
                //非UI线程未捕获异常处理事件
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                Jlion.BrushClient.Client.Startup.Start();

                #region 内存优化
                var cracker = new WpfCrackerHelper();
                cracker.Cracker();
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The located assembly's manifest definition does not match the assembly reference"))
                {
                    MessageBox.Show("请先安装补丁包");
                    Environment.Exit(0);
                    TextHelper.Error(ex.Message, ex);
                }
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            TextHelper.Error($"CurrentDomain_UnhandledException:{e.ExceptionObject?.ToString() ?? ""}");
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            TextHelper.Error("TaskScheduler_UnobservedTaskException", e?.Exception);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            TextHelper.Error("App_DispatcherUnhandledException", e?.Exception);
        }
    }
}
