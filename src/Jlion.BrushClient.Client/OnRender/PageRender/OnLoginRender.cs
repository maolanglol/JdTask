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
using Jlion.BrushClient.Framework.Helper;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnLoginRender : OnSingleRender
    {
        #region 属性定义
        private OnControlRender _onControlRender;
        private OnMainHostRequestPlugins _onMainHostRequestPlugins;
        private OnTipRender _onTipRender;
        #endregion

        public OnLoginRender(
            OnControlRender onControlRender,
            OnTipRender onTipRender,
            OnMainHostRequestPlugins onMainHostRequestPlugins)
        {
            _onControlRender = onControlRender;
            _onMainHostRequestPlugins = onMainHostRequestPlugins;
            _onTipRender = onTipRender;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task RenderLoginAsync(Login login)
        {
            try
            {
                var userName = login.UserName.Text.Trim();
                var passWord = login.Password.Password.Trim();
                //var isRemember = login.cbRememberPassword.IsChecked ?? false;
                //var isAutoLogin = login.cbAutoLogin.IsChecked ?? false;
                if (!Verify(login))
                    return;

                var loginResponse = await _onMainHostRequestPlugins.AuthAsync(new AuthRequest()
                {
                    Mobile = userName,
                    Password = passWord
                });
                if (!(loginResponse?.Success ?? false))
                {
                    _onTipRender.ExecuteTip(login.BodyPanel, loginResponse?.Msg ?? "登陆失败001");
                    return;
                }
                if (string.IsNullOrWhiteSpace(loginResponse?.Data?.AccessToken ?? ""))
                {
                    _onTipRender.ExecuteTip(login.BodyPanel, loginResponse?.Msg ?? "登陆失败002");
                    return;
                }

                var accessToken = loginResponse.Data.AccessToken;
                var refreshToken = loginResponse.Data.RefreshToken;
                //ServiceIniCacheHelper.WriteAccessToken(loginResponse.DataaccessToken);
                //ServiceIniCacheHelper.WriteRefreshToken(refreshToken);

                #region 设置登录信息
                var systemRespnose = await _onMainHostRequestPlugins.QuerySettingsAsync(accessToken);
                AccountCache.SetLogin(new AccountCache()
                {
                    UserName = userName,
                    Password = passWord,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    SystemSettings = systemRespnose?.Data ?? new SystemSettingsResponse()
                });
                #endregion

                #region 登陆成功
                var main = AutofacManage.GetService<Main>();
                main.ShowAmount();
                var suspensionMain = AutofacManage.GetService<SuspensionMain>();
                main.Show();
                suspensionMain.Show();
                login.Hide();
                #endregion
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RenderLoginAsync 登陆失败 errror:{ex.Message}", ex);
                _onTipRender.ExecuteTip(login.BodyPanel, ex.Message ?? "登陆失败003");
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task RenderRegAsync(Login login)
        {
            try
            {
                var userName = login.UserName.Text.Trim();
                var passWord = login.Password.Password.Trim();
                var merchantId = login.MerchantId.Text.Trim().ToInt32OrDefault(0);
                //var isRemember = login.cbRememberPassword.IsChecked ?? false;
                //var isAutoLogin = login.cbAutoLogin.IsChecked ?? false;
                if (!Verify(login))
                    return;

                var loginResponse = await _onMainHostRequestPlugins.RegisterAsync(new UserRegRequest()
                {
                    Mobile = userName,
                    Password = passWord,
                    MerchantId = merchantId
                }); ;
                if (!(loginResponse?.Success ?? false))
                {
                    _onTipRender.ExecuteTip(login.BodyPanel, loginResponse?.Msg ?? "注册失败001");
                    return;
                }
                _onTipRender.ExecuteTip(login.BodyPanel, "注册成功", EnumResultType.OK);
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RenderRegAsync 登陆失败 errror:{ex.Message}", ex);
            }
        }

        #region 私有方法
        public bool Verify(Login login)
        {
            var userName = login.UserName.Text.Trim();
            var passWord = login.Password.Password.Trim();
            //var isRemember = login.cbRememberPassword.IsChecked ?? false;
            //var isAutoLogin = login.cbAutoLogin.IsChecked ?? false;

            if (string.IsNullOrWhiteSpace(userName))
            {
                _onTipRender.ExecuteTip(login.BodyPanel, "请输入用户名");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passWord))
            {
                _onTipRender.ExecuteTip(login.BodyPanel, "请输入用户密码");
                return false;
            }
            return true;
        }
        #endregion
    }
}
