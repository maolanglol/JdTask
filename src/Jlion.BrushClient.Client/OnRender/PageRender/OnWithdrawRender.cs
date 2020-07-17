using Jlion.BrushClient.Application.Enums;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.Interceptor;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnWithdrawRender : OnSingleRender
    {
        private OnTipRender _onTipRender;
        private OnControlRender _onControlRender;
        private OnRedirectRender _onRedirectRender;

        private OnMainHostRequestPlugins _onMainHostRequestPlugins;

        public OnWithdrawRender(OnTipRender onTipRender,
            OnControlRender onControlRender,
            OnRedirectRender onRedirectRender,
            OnMainHostRequestPlugins onMainHostRequestPlugins)
        {
            _onTipRender = onTipRender;
            _onControlRender = onControlRender;
            _onRedirectRender = onRedirectRender;

            _onMainHostRequestPlugins = onMainHostRequestPlugins;
        }

        /// <summary>
        /// 查询提现记录列表
        /// </summary>
        /// <param name="orderDataRequest"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [LoggerAttribute(BusinessName = "查询提现记录列表")]
        public virtual async Task RenderListAsync(WithdrawRecord page, int pageIndex = 1)
        {
            try
            {
                var rows = 20;
                var gteTime = Convert.ToDateTime(page.DpStartTime.Text);
                var lteTime = Convert.ToDateTime(page.DpEndTime.Text);
                var status = (EnumWithdrawStatus)Convert.ToInt32(((ComboBoxItem)page.CmbStatus.SelectedValue).Tag.ToString());
                var response = await _onMainHostRequestPlugins.WithdrawRecordListAsync(new Application.Model.WithdrwaRecordGetRequest()
                {
                    Page = pageIndex,
                    Rows = rows,
                    Status = status,
                    Types = EnumAccountType.User,
                    GteTime = gteTime,
                    LteTime = lteTime,
                    AccessToken = AccountCache.Persist.AccessToken
                });

                if (response != null && response.Code == Application.Enums.ApiCodeEnums.ERROR_NOLOGIN)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "登陆失效,请退出重新登陆");
                    _onRedirectRender.RedirectLogin();
                    return;
                }

                var resp = response.Data;
                #region 数据绑定

                _onControlRender.ThreadExecuteUI(() =>
                {
                    _onControlRender.BindFrameworkElement(page.DataGridOrderList, resp?.Data?.ToList() ?? null);

                    if ((resp.Data.Count()) <= 0)
                        page.SpPager.Visibility = Visibility.Collapsed;
                    else
                        page.SpPager.Visibility = Visibility.Visible;

                    var sump = Convert.ToInt32(resp.TotalCount) % rows;
                    var totalPage = Convert.ToInt32(resp?.TotalCount ?? 0) / rows + (sump > 0 ? 1 : 0);
                    page.labTotalPage.Content = $"/{totalPage}";
                    page.labCurrentPage.Content = $"{pageIndex}";

                    page.labPrePage.Tag = $"{pageIndex - 1}";
                    page.labPrePage.IsEnabled = pageIndex > 1;
                    page.labNextPage.Tag = $"{pageIndex + 1}";
                    page.labNextPage.IsEnabled = (pageIndex + 1) <= totalPage;
                    page.txbJumb.Tag = totalPage;
                });
                #endregion
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderList 异常", ex);
                _onTipRender.ExecuteTip(page.BodyPanel, "查询异常[0001]");
            }
        }
    }
}
