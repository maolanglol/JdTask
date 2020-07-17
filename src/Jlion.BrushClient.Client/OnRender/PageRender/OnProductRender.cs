using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.Interceptor;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnProductRender : OnSingleRender
    {
        private OnTipRender _onTipRender;
        private OnControlRender _onControlRender;
        private OnMainHostRequestPlugins _onMainHostRequestPlugins;
        private OnRedirectRender _onRedirectRender;
        public OnProductRender(OnTipRender onTipRender,
            OnControlRender onControlRender,
            OnRedirectRender onRedirectRender,
            OnMainHostRequestPlugins onMainHostRequestPlugins)
        {
            _onTipRender = onTipRender;
            _onControlRender = onControlRender;
            _onMainHostRequestPlugins = onMainHostRequestPlugins;
            _onRedirectRender = onRedirectRender;
        }


        /// <summary>
        /// 查询渲染产品列表
        /// </summary>
        /// <param name="orderDataRequest"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [LoggerAttribute(BusinessName = "查询渲染商品列表")]
        public virtual async Task RenderListAsync(Product page)
        {
            try
            {
                var listResponse = await _onMainHostRequestPlugins.GetProductListAsync(AccountCache.Persist.AccessToken);
                if (listResponse.Code == Application.Enums.ApiCodeEnums.ERROR_NOLOGIN)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "登陆失效,请退出重新登陆");
                    _onRedirectRender.RedirectLogin();
                    return;
                }
                var data = listResponse.Data;
                _onControlRender.ThreadExecuteUI(() =>
                {
                    for (var i = 0; i < data.Count; i++)
                    {
                        if (i == 0)
                        {
                            data[i].Color = "#818184";
                        }
                        if (i == 1)
                        {
                            data[i].Color = "#708e9e";

                        }
                        if (i == 2)
                        {
                            data[i].Color = "#a9a98d";
                        }
                        if (i == 3)
                        {
                            data[i].Color = "#8aa48d";
                        }
                        if (i == 4)
                        {
                            data[i].Color = "#7f698c";
                        }
                    }
                    page.DataGridProductList.ItemsSource = listResponse.Data;
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderListAsync 异常", ex);
                _onTipRender.ExecuteTip(page.BodyPanel, "查询异常[0001]");
            }
        }

        public virtual async Task RenderPayAsync(Product page, int productId)
        {
            var data = await _onMainHostRequestPlugins.GetPreOrderAsync(productId, AccountCache.Persist.AccessToken);
            var url = data?.Data ?? "";
            if (string.IsNullOrWhiteSpace(url))
            {
                _onTipRender.ExecuteTip(page.BodyPanel, data?.Msg ?? "支付失败");
                return;
            }

            try
            {
                var main = AutofacManage.GetService<Main>();
                main.HideWindow();
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = url;
                proc.Start();
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RenderPayAsync 异常", ex);
                _onTipRender.ExecuteTip(page.BodyPanel, "打开支付链接失败");

            }
        }

        public virtual async Task RenderPayActiveAsync(Product page)
        {
            try
            {
                var cardNo = page.txbActiveCode.Text.Trim();
                if (string.IsNullOrWhiteSpace(cardNo))
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "请输入激活码");
                    return;
                }
                var data = await _onMainHostRequestPlugins.ActiveAsync(AccountCache.Persist.AccessToken, cardNo);
                if (!(data?.Success ?? false))
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, data?.Msg ?? "激活失败");
                    return;
                }
                _onTipRender.ExecuteTip(page.BodyPanel, "激活成功", UControl.EnumResultType.OK);

                var main = AutofacManage.GetService<Main>();
                main.ShowAmount();
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RenderPayAsync 异常", ex);
                _onTipRender.ExecuteTip(page.BodyPanel, "打开支付链接失败");

            }
        }
    }
}
