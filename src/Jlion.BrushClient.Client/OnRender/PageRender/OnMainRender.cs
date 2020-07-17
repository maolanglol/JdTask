using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Jlion.BrushClient.Application.Model;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.Helper;
using Jlion.BrushClient.Client.Interceptor;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Client.Model.Request;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.UControl;
using static Jlion.BrushClient.Client.Enums.OptionEnums;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnMainRender : OnSingleRender
    {
        #region 属性定义
        private OnControlRender _onControlRender;
        private OnMainHostRequestPlugins _onMainHostRequestPlugins;
        private OnTipRender _onTipRender;
        private OnRedirectRender _onRedirectRender;
        #endregion

        public OnMainRender(OnMainHostRequestPlugins onMainHostRequestPlugins,
            OnRedirectRender onRedirectRender,
            OnTipRender onTipRender,
            OnControlRender onControlRender)
        {
            _onControlRender = onControlRender;
            _onTipRender = onTipRender;
            _onRedirectRender = onRedirectRender;
            _onMainHostRequestPlugins = onMainHostRequestPlugins;
        }

        #region public Methods
        public virtual async Task Start(Main main)
        {
            var browser = AutofacManage.GetService<Browser>();
            if (!AccountCache.Persist.IsMember)
            {
                _onTipRender.ExecuteTip(main.BodyPanel, "您还不是会员,请成为会员后开启挣钱之旅");
                return;
            }
            browser.Show();
            browser.StartService();
            main.Hide();
        }

        public virtual async Task Stop(Main main)
        {
            var browser = AutofacManage.GetService<Browser>();
            browser.StopService();
            browser.Hide();
        }

        public virtual async Task RenderAccountAsync(Main main)
        {
            try
            {
                var acccountResponse = await _onMainHostRequestPlugins.QueryAccountAsync(AccountCache.Persist.AccessToken);
                if (acccountResponse.Code == Application.Enums.ApiCodeEnums.ERROR_NOLOGIN)
                {
                    _onTipRender.ExecuteTip(main.BodyPanel, "登陆失效,请退出重新登陆");
                    _onRedirectRender.RedirectLogin();
                    return;
                }
                var data = acccountResponse?.Data ?? null;
                AccountCache.Persist.IsMember = data?.IsMember ?? false;
                _onControlRender.ThreadExecuteUI(() =>
                {
                    var suspensionMain = AutofacManage.GetService<SuspensionMain>();
                    suspensionMain.labPayMoney.Content = $"￥{(acccountResponse.Data?.Balance ?? 0)}";
                    data.RealName = string.IsNullOrWhiteSpace(data.RealName) ? "未实名认证" : data.RealName;
                    main.labStoreName.Content = data.RealName;
                    main.StoreBox.DataContext = data;

                });
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RenderAccountAsync 异常 message:{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        [LoggerAttribute(BusinessName = "渲染主窗体")]
        public virtual void RenderMainWindows(bool isShowMain = true)
        {
            var main = AutofacManage.GetService<Main>();
            var suspensionMan = AutofacManage.GetService<SuspensionMain>();

            main.InitializeNotify();
            if (!isShowMain)
                return;

            main.Show();
            main.Activate();
            main.Topmost = false;
            main.Topmost = true;
        }


        /// <summary>
        /// 获取当前的Page 渲染的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        public Page GetSettingInstance<T>(Main page)
        {
            try
            {
                var obj = page.FrameTab.Content;
                if (obj is Page)
                {
                    return obj as Page;
                }
            }
            catch (Exception ex)
            {
                TextHelper.ErrorAsync("GetSettingInstance 异常", ex);
            }
            return null;
        }

        /// <summary>
        /// 渲染菜单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="navation"></param>
        [LoggerAttribute(BusinessName = "渲染菜单")]
        public virtual void RenderMenus(Main page, string navation, ButtonEx button)
        {
            try
            {
                page.FrameTab.Source = new Uri(navation, UriKind.Relative);
                var obj = page.FrameTab.Content;
                page.ClearStyle();
                button.Style = page.GetActiveStyle("ActiveButtonEx");

                var _=this.RenderAccountAsync(page);
            }
            catch (Exception ex)
            {
                TextHelper.ErrorAsync("RenderMenus 异常", ex);
            }
        }

        /// <summary>
        /// 网络检测
        /// </summary>
        /// <param name="page"></param>
        public void RenderInternet(Main page)
        {
            //try
            //{
            //    var isProxy = _onDeviceSettingPlugins.GlobalConfig?.ProxyConfig?.IsOpen ?? false;
            //    _onTimerPlugins.Add(TimeKeyStatics.InternetKey("pingBaidu"), () =>
            //    {
            //        _onControlRender.ThreadExecuteUI(() =>
            //        {
            //            var isPing = ServiceIniCacheHelper.ReadIsPing();
            //            if (!isPing)
            //                return;

            //            var payHost = _onAppSettingPlugins.PluginsHost.Url.TrimEnd('/');
            //            payHost = payHost.Remove(0, payHost.IndexOf(":/") + 3);
            //            var bl = _onToolRequest.PingInternet(payHost, isProxy);

            //            //var bl = _onToolRequest.PingInternet(isProxy);
            //            if (bl)
            //            {
            //                page.btnInternet.Icon = new BitmapImage(new Uri(EnumInternetType.Online.GetDescription(), UriKind.RelativeOrAbsolute));
            //                page.btnInternet.Content = "网络畅通";
            //            }
            //            else
            //            {
            //                page.btnInternet.Icon = new BitmapImage(new Uri(EnumInternetType.Offline.GetDescription(), UriKind.RelativeOrAbsolute));
            //                page.btnInternet.Content = "网络异常";

            //                _onNotificationRender.ExecuteRender(new Model.NotifyDataRequest()
            //                {
            //                    Content = "网络异常",
            //                    Title = "异常提醒"
            //                });
            //            }
            //        });
            //        return false;
            //    }, 20000, DateTime.Now);
            //}
            //catch (Exception ex)
            //{
            //    TextHelper.ErrorAsync("RenderInternet 异常", ex);
            //}
        }

        /// <summary>
        /// 退出
        /// </summary>
        [LoggerAttribute(BusinessName = "退出")]
        public virtual void ExecuteExit(Main main)
        {
            try
            {
                var browser = AutofacManage.GetService<Browser>();
                browser.StopService();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                TextHelper.ErrorAsync("ExecuteExit 异常", ex);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="page"></param>
        [LoggerAttribute(BusinessName = "退出登录")]
        public virtual void RenderLogout(Main page)
        {
            try
            {
                var browser = AutofacManage.GetService<Browser>();
                browser.StopService();
                LoginPersist.AccountCache.Clear();

                var login = AutofacManage.GetService<Login>();
                var suspensionMain = AutofacManage.GetService<SuspensionMain>();


                suspensionMain.Hide();
                browser.Hide();
                login.IsAuto = false;
                login.Show();
            }
            catch (Exception ex)
            {
                TextHelper.ErrorAsync("RenderLogout 异常", ex);
            }

        }
        #endregion

        #region private Methods
        #endregion
    }
}
