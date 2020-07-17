//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Media.Imaging;
//using Jlion.BrushClient.Application.Plugins;
//using Jlion.BrushClient.Client.OnRequest;
//using Jlion.BrushClient.Framework;
//using Jlion.BrushClient.Framework.Helper;
//using WP.Device.Plugins.Kernel;
//using static Jlion.BrushClient.Client.Enums.OptionEnums;

//namespace Jlion.BrushClient.Client.OnRender
//{
//    public class OnToolRender : OnSingleRender
//    {
//        private OnToolRequest _onToolRequest;
//        private OnAppSettingPlugins _onAppSettingPlugins;
//        private OnControlRender _onControlRender;

//        private OnDeviceSettingPlugins _onDeviceSettingPlugins;

//        public OnToolRender(OnToolRequest onToolRequest, OnAppSettingPlugins onAppSettingPlugins,
//            OnDeviceSettingPlugins onDeviceSettingPlugins,
//            OnControlRender onControlRender)
//        {
//            _onToolRequest = onToolRequest;

//            _onAppSettingPlugins = onAppSettingPlugins;
//            _onDeviceSettingPlugins = onDeviceSettingPlugins;
//            _onControlRender = onControlRender;
//        }

//        public async Task RenderCheck(Tool page)
//        {
//            try
//            {
//                var checkClient = await this.RenderChekcIntegrity(page);
//                var checkInternet = await this.RenderChekcInternet(page);

//                _onControlRender.ThreadExecuteUI(() =>
//                {

//                    var checkBl = !checkClient || !checkInternet;

//                    var checkType = checkBl ? EnumCheckType.ERROR : EnumCheckType.OK;
//                    page.checkResult.Source = new BitmapImage(new Uri(checkType.GetDescription(), UriKind.RelativeOrAbsolute));
//                    page.checkResult.Visibility = System.Windows.Visibility.Visible;
//                });
//            }
//            catch (Exception ex)
//            {
//                TextHelper.Error("RenderCheck 异常", ex);
//            }
//        }

//        /// <summary>
//        /// 检测完整性
//        /// </summary>
//        /// <param name="page"></param>
//        /// <returns></returns>
//        public async Task<bool> RenderChekcIntegrity(Tool page)
//        {
//            return await Task.Factory.StartNew<bool>(() =>
//            {
//                try
//                {
//                    _onControlRender.ThreadExecuteUI(() =>
//                    {
//                        page.imgClient.Visibility = System.Windows.Visibility.Collapsed;
//                        page.labClientMark.Visibility = System.Windows.Visibility.Visible;
//                    });

//                    Thread.Sleep(1000);

//                    #region 检测完整性
//                    var list = ConstDefintion.GetFiles;
//                    var checkResult = false;
//                    var errorCount = 0;
//                    list.ForEach(item =>
//                    {
//                        checkResult = FileHelper.IsExists(item);
//                        if (!checkResult)
//                        {
//                            errorCount++;
//                            return;
//                        }
//                    });
//                    checkResult = errorCount <= 0;
//                    #endregion

//                    #region 切回UI线程进行渲染
//                    _onControlRender.ThreadExecuteUI(() =>
//                    {
//                        page.borderResultBox.Visibility = System.Windows.Visibility.Visible;

//                        var checkType = checkResult ? EnumCheckType.OK : EnumCheckType.ERROR;
//                        page.imgClient.Source = new BitmapImage(new Uri(checkType.GetDescription(), UriKind.RelativeOrAbsolute));

//                        page.clientCheckResultImage.Source = new BitmapImage(new Uri((checkResult ? IconEnumType.CheckClientIconSuccess : IconEnumType.CheckClientIconFail).GetDescription(), UriKind.RelativeOrAbsolute));


//                        page.imgClient.Visibility = System.Windows.Visibility.Visible;
//                        page.labClientMark.Visibility = System.Windows.Visibility.Collapsed;
//                    });
//                    #endregion
//                    return checkResult;
//                }
//                catch (Exception ex)
//                {
//                    TextHelper.Error("RenderChekcIntegrity 异常", ex);
//                    return false;
//                }
//            });
//        }

//        /// <summary>
//        /// 检测网络
//        /// </summary>
//        /// <param name="page"></param>
//        /// <returns></returns>
//        public async Task<bool> RenderChekcInternet(Tool page)
//        {
//            var isProxy = _onDeviceSettingPlugins.GlobalConfig?.ProxyConfig?.IsOpen ?? false;
//            return await Task.Factory.StartNew<bool>(() =>
//            {
//                try
//                {
//                    _onControlRender.ThreadExecuteUI(() =>
//                    {
//                        page.imgInternet.Visibility = System.Windows.Visibility.Collapsed;
//                        page.labInternetMark.Visibility = System.Windows.Visibility.Visible;
//                    });

//                    Thread.Sleep(1000);

//                    #region 检测网络
//                    //检测百度网络
//                    var bl = _onToolRequest.PingInternet(isProxy: isProxy);

//                    //检测自身网络
//                    var payHost = _onAppSettingPlugins.PluginsHost.Url.TrimEnd('/');
//                    payHost = payHost.Remove(0, payHost.IndexOf(":/") + 3);

//                    var payHostCheck = _onToolRequest.PingInternet(payHost,isProxy);
//                    var chekcResult = !bl || !payHostCheck;
//                    #endregion

//                    #region 切回UI线程进行渲染
//                    _onControlRender.ThreadExecuteUI(() =>
//                    {
//                        page.borderResultBox.Visibility = System.Windows.Visibility.Visible;

//                        var checkType = chekcResult ? EnumCheckType.ERROR : EnumCheckType.OK;
//                        page.imgInternet.Source = new BitmapImage(new Uri(checkType.GetDescription(), UriKind.RelativeOrAbsolute));
//                        page.internetCheckResultImage.Source = new BitmapImage(new Uri(((!chekcResult) ? IconEnumType.CheckInternetIconSuccess : IconEnumType.CheckInternetIconFail).GetDescription(), UriKind.RelativeOrAbsolute));

//                        page.imgInternet.Visibility = System.Windows.Visibility.Visible;
//                        page.labInternetMark.Visibility = System.Windows.Visibility.Collapsed;
//                    });
//                    #endregion

//                    return !chekcResult;
//                }
//                catch (Exception ex)
//                {
//                    TextHelper.Error("RenderChekcIntegrity 异常", ex);
//                    return false;
//                }
//            });
//        }

//        /// <summary>
//        /// 生成支付的子订单号
//        /// </summary>
//        /// <param name="cashId"></param>
//        /// <param name="deviceId"></param>
//        /// <returns></returns>
//        public string GetSubOtNo(long cashId, string deviceId)
//        {
//            if (string.IsNullOrWhiteSpace(deviceId) && deviceId.Length < 32)
//                throw new ArgumentException("device Id is not null");

//            return $"DP{cashId}{deviceId.Substring(8, 16)}{DateTime.Now.ToString("yyMMddHHmmssfff")}";
//        }
//    }
//}
