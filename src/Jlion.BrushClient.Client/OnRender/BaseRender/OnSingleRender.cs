using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnSingleRender
    {
        //public  DynamicMetaObject GetMetaObject(System.Linq.Expressions.Expression parameter)
        //{
        //    return new AspectWeaver(parameter, this);
        //}

        public void DragMove(Window window)
        {
            try
            {
                window.DragMove();
            }
            catch (Exception ex)
            {
#if DEBUG
                TextHelper.ErrorAsync("DragMove 异常", ex);
#endif
            }
        }

        //public void ExecuteHotKey(System.Windows.Input.Key keys)
        //{
        //    var appSettingsPlugins = AutofacManage.GetService<OnDeviceSettingPlugins>();
        //    var closeKeys = appSettingsPlugins.GlobalConfig.HotKeyConfig.Close.ConvertKeys();
        //    if (keys == closeKeys)
        //    {
        //        ExecuteClose();
        //    }
        //}

        ///// <summary>
        ///// 窗口关闭
        ///// </summary>
        //public void ExecuteClose()
        //{
        //    var main = AutofacManage.GetService<Main>();
        //    var login = AutofacManage.GetService<Login>();
        //    var payment = AutofacManage.GetService<Payment>();
        //    var refund = AutofacManage.GetService<Refund>();
        //    var proxy = AutofacManage.GetService<Proxy>();

        //    if (main.Visibility == Visibility.Visible)
        //    {
        //        var settingPage = main.GetSettlement<Setting>() as Setting;
        //        if (settingPage == null || !settingPage.TabHotKeySettings.IsSelected || AccountCache.Persist.IsStart)
        //            main.Hide();
        //    }
        //    if (login.Visibility == Visibility.Visible)
        //        login.CloseWindow();
        //    if (payment.Visibility == Visibility.Visible)
        //        payment.Hide();
        //    if (refund.Visibility == Visibility.Visible)
        //        refund.Hide();
        //    if (proxy.Visibility == Visibility.Visible)
        //        proxy.Hide();
        //}

    }
}
