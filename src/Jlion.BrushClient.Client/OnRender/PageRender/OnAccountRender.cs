using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnAccountRender : OnSingleRender
    {
        OnMainHostRequestPlugins _onMainHostRequestPlugins;
        OnTipRender _onTipRender;
        OnRedirectRender _onRedirectRender;
        OnControlRender _onControlRender;

        public OnAccountRender(OnMainHostRequestPlugins onMainHostRequestPlugins,
            OnControlRender onControlRender,
            OnTipRender onTipRender,
            OnRedirectRender onRedirectRender)
        {
            _onMainHostRequestPlugins = onMainHostRequestPlugins;
            _onTipRender = onTipRender;
            _onRedirectRender = onRedirectRender;
            _onControlRender = onControlRender;
        }

        public virtual async Task RenderAccountAsync(Account account)
        {
            try
            {
                var acccountResponse = await _onMainHostRequestPlugins.QueryAccountAsync(AccountCache.Persist.AccessToken);
                if (acccountResponse.Code == Application.Enums.ApiCodeEnums.ERROR_NOLOGIN)
                {
                    _onTipRender.ExecuteTip(account.BodyPanel, "登陆失效,请退出重新登陆");
                    _onRedirectRender.RedirectLogin();
                    return;
                }
                var data = acccountResponse?.Data ?? null;
                _onControlRender.ThreadExecuteUI(() =>
                {
                    if (!string.IsNullOrWhiteSpace(data.AlipayAccount))
                    {
                        account.tbAlipayAccount.IsReadOnly = true;
                    }
                    if (!string.IsNullOrWhiteSpace(data.RealName))
                    {
                        account.tbAlipayName.IsReadOnly = true;
                    }
                    var handleFee = $"提现手续费{AccountCache.Persist.SystemSettings.HandleFee * 100}%";
                    if (AccountCache.Persist.SystemSettings.HandleFee >= 1)
                    {
                        handleFee = $"提现手续费{AccountCache.Persist.SystemSettings.HandleFee}元";
                    }
                    account.lbHandler.Content = handleFee;
                    account.BodyPanel.DataContext = data;
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RenderAccountAsync 异常 message:{ex.Message}", ex);
            }
        }

        public virtual async Task ExecuteWithdrawAsync(Account account)
        {
            try
            {
                var alipayAccount = account.tbAlipayAccount.Text;
                var alipayName = account.tbAlipayName.Text;
                var amount = account.tbAmount.Text.ToDecimalOrDefault(0);
                if (string.IsNullOrWhiteSpace(alipayName))
                {
                    _onTipRender.ExecuteTip(account.BodyPanel, "请输入支付宝账号真实姓名");
                    return;
                }
                if (string.IsNullOrWhiteSpace(alipayAccount))
                {
                    _onTipRender.ExecuteTip(account.BodyPanel, "请输入提现支付宝账号");
                    return;
                }
                if (amount <= 0)
                {
                    _onTipRender.ExecuteTip(account.BodyPanel, "请输入提现金额");
                    return;
                }
                if (amount < 5)
                {
                    _onTipRender.ExecuteTip(account.BodyPanel, "最低提现5元");
                    return;
                }
                var handler = amount * AccountCache.Persist.SystemSettings.HandleFee;


                var widthdrawResponse = await _onMainHostRequestPlugins.WithdrawAsync(new Application.Model.WithdrawRequest()
                {
                    AlipayAccount = alipayAccount,
                    Amount = amount,
                    Name = alipayName,
                    AccessToken = AccountCache.Persist.AccessToken,
                });
                if (!(widthdrawResponse?.Data ?? false))
                {
                    _onTipRender.ExecuteTip(account.BodyPanel, widthdrawResponse?.Msg ?? "提现申请失败");
                    return;
                }
                if (widthdrawResponse.Code == Application.Enums.ApiCodeEnums.ERROR_NOLOGIN)
                {
                    _onTipRender.ExecuteTip(account.BodyPanel, "登陆失效,请退出重新登陆");
                    _onRedirectRender.RedirectLogin();
                    return;
                }
                _onTipRender.ExecuteTip(account.BodyPanel, "提现申请成功,请耐心等待审核", UControl.EnumResultType.OK);
                var _ = RenderAccountAsync(account);
            }
            catch (Exception ex)
            {
                TextHelper.Error($"ExecuteWithdrawAsync 异常", ex);
            }
        }

        public virtual void RenderHandler(Account account)
        {
            try
            {
                var amount = account.tbAmount.Text.ToDecimalOrDefault(0);
                var handler = amount * AccountCache.Persist.SystemSettings.HandleFee;
                if (AccountCache.Persist.SystemSettings.HandleFee >= 1)
                {
                    handler = 1;
                }
                account.lbTotalAmount.Content = $"预计扣除手续费{handler}元";
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RenderHandlerAsync 异常 message:{ex.Message}", ex);
            }
        }
    }
}
