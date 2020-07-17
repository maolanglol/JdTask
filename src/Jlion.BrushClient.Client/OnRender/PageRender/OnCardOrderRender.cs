using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Jlion.BrushClient.Application.Enums;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Client.Model.Response;
using Jlion.BrushClient.Client.OnRender.PrintRender;
using Jlion.BrushClient.Client.OnRequest;
using Jlion.BrushClient.Framework;
using WP.Device.Plugins.Kernel;
using Jlion.BrushClient.UControl;
using static Jlion.BrushClient.Client.Enums.OptionEnums;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnCardOrderRender : OnSingleRender
    {
        private OnOrderRemoteRequest _onOrderRequest;
        private OnCashierRequest _onCashierRequest;
        private OnControlRender _onControlRender;
        private OnTipRender _onTipRender;
        private OnDeviceSettingPlugins _onDeviceSettingPlugins;
        private OnSystemRender _onSystemRender;

        public OnCardOrderRender(OnOrderRemoteRequest onOrderRequest,
            OnCashierRequest onCashierRequest,
            OnControlRender onControlRender, OnTipRender onTipRender,
            OnSystemRender onSystemRender,
            OnDeviceSettingPlugins onDeviceSettingPlugins)
        {
            _onOrderRequest = onOrderRequest;
            _onCashierRequest = onCashierRequest;

            _onControlRender = onControlRender;
            _onTipRender = onTipRender;

            _onSystemRender = onSystemRender;
            _onDeviceSettingPlugins = onDeviceSettingPlugins;
        }

        /// <summary>
        /// 查询渲染订单列表
        /// </summary>
        /// <param name="orderDataRequest"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task RenderListAsync(CardOrder page, int pageIndex = 1)
        {
            try
            {
                #region 组装数据
                var cardNo = page.txbCardNo.Text.Equals("请输入会员手机号或卡号") ? "" : page.txbCardNo.Text;
                var orderDataRequest = new Model.CardOrderRequest()
                {
                    CashId = AccountCache.Persist.CashId,
                    OrderNo = page.txbOrderNo.Text.Trim(),
                    Type = Convert.ToInt32(((ComboBoxItem)page.CmbOrderType.SelectedValue).Tag.ToString()),
                    PageIndex = pageIndex,
                    StartTime = Convert.ToDateTime(page.DpStartTime.Text),
                    EndTime = Convert.ToDateTime(page.DpEndTime.Text),
                    Rows = 8,
                    StoresId = AccountCache.Persist.StoreId,
                    PhoneOrCardNo = cardNo,
                };
                #endregion

                #region 信息验证
                if ((orderDataRequest?.CashId ?? 0) <= 0)
                {
                    _onControlRender.ThreadExecuteUI(() =>
                    {
                        _onTipRender.ExecuteTip(page.BodyPanel, "参数错误");
                        page.DataGridOrderList.DataContext = new List<CardOrderItemResponse>();
                    });
                    return;
                }
                #endregion

                #region 查询订单
                var resp = await _onOrderRequest.ExecuteQueryAsync(orderDataRequest);
                if (!(resp?.IsSuccess ?? false))
                {
                    _onControlRender.ThreadExecuteUI(() =>
                    {
                        _onTipRender.ExecuteTip(page.BodyPanel, resp?.Msg ?? "查询异常");
                        page.DataGridOrderList.DataContext = new List<CardOrderItemResponse>();
                    });
                    return;
                }
                #endregion

                #region 数据绑定

                _onControlRender.ThreadExecuteUI(() =>
                {
                    page.btnExport.IsEnabled = (resp.Data?.Items?.Count ?? 0) > 0;
                    page.btnPrinter.IsEnabled = (resp.Data?.Items?.Count ?? 0) > 0;
                    page.btnRefund.IsEnabled = (resp.Data?.Items?.Count ?? 0) > 0;
                    //if ((resp.Data?.Items?.Count ?? 0) > 0)
                    //{
                    _onControlRender.BindFrameworkElement(page.DataGridOrderList, resp.Data?.Items ?? null);
                    //}

                    if ((resp.Data?.Items?.Count ?? 0) <= 0)
                        page.SpPager.Visibility = Visibility.Collapsed;
                    else
                        page.SpPager.Visibility = Visibility.Visible;

                    var sump = Convert.ToInt32(resp.Data?.Total ?? 0) % orderDataRequest.Rows;
                    var totalPage = Convert.ToInt32(resp.Data?.Total ?? 0) / orderDataRequest.Rows + (sump > 0 ? 1 : 0);
                    page.labTotalPage.Content = $"/{totalPage}";
                    page.labCurrentPage.Content = $"{orderDataRequest.PageIndex}";

                    page.labPrePage.Tag = $"{orderDataRequest.PageIndex - 1}";
                    page.labPrePage.IsEnabled = orderDataRequest.PageIndex > 1;
                    page.labNextPage.Tag = $"{orderDataRequest.PageIndex + 1}";
                    page.labNextPage.IsEnabled = (orderDataRequest.PageIndex + 1) <= totalPage;
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

        /// <summary>
        /// 退款渲染
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public async Task RenderRefundAsync(CardOrder page, List<string> orderNos)
        {
            try
            {
                #region 验证信息
                if ((orderNos?.Count ?? 0) <= 0)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "请选择要退款的订单");
                    return;
                }

                if (orderNos?.Count > 1)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "只能选择单笔进行退款");
                    return;
                }

                var orderList = _onControlRender.ConvertObject<CardOrderItemResponse>(page.DataGridOrderList.Items);
                var orders = FindOrderNos(orderList, new List<string>() { orderNos[0] });
                var order = orders != null ? orders[0] : null;

                if (string.IsNullOrWhiteSpace(order.OrderNo))
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "没有找到该笔订单");
                    return;
                }
                if (order.Type == (int)CardOrderTypeEnums.Recharge)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "暂不支持对充值的订单进行退款");
                    return;
                }
                if (order.Type == (int)CardOrderTypeEnums.Refund)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "该订单已经退款不能再退款");
                    return;
                }
                //var orderItem = await _onOrderRequest.ExecuteQueryAsync(order[0].OutTradeNo, order[0].OrderType, Convert.ToDateTime(order[0].PayTime), AccountCache.Persist.MerchantId);
                //if (orderItem?.State.ToClientOrderStatus() != EnumOrderStatus.PaySuccess && orderItem?.State.ToClientOrderStatus() != EnumOrderStatus.PartSucccess)
                //{
                //    _onTipRender.ExecuteTip(page.BodyPanel, "该订单已经退款");
                //    return;
                //}
                if (order.CanRefundAmt.ToDecimalOrDefault(0) <= 0)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "没有可退金额");
                    return;
                }

                if (AccountCache.Persist == null)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "登录失效,请重新退出登录");
                    return;
                }
                #endregion

                var isEnabledPassword = _onDeviceSettingPlugins.GlobalConfig.ReceiptConfig.IsEnablePassword;
                _onTipRender.ExecuteConfimTextBoxCombine(page.BodyPanel, "退款", "金额：", order.CanRefundAmt.ToString(),
                    submitAction: (obj, eventArgs) =>
                    {
                        var tipConfirmBox = obj as TipConfirmBox;
                        var canFundAmount = order.CanRefundAmt.ToDecimalOrDefault(0);//可退金额
                        var fundAmount = tipConfirmBox.Value.ToDecimalOrDefault(0);
                        if (fundAmount > canFundAmount)
                        {
                            _onTipRender.ExecuteTip(page.BodyPanel, "退款金额大于可退金额");
                            return;
                        }

                        var password = tipConfirmBox.TextInputPasswordValue;
                        if (isEnabledPassword && string.IsNullOrWhiteSpace(password))
                        {
                            _onTipRender.ExecuteTip(page.BodyPanel, "请输入密码");
                            return;
                        }
                        TextHelper.LogInfo($"储蓄卡退款开始... orderNos:{orderNos[0]},amount:{fundAmount}");
                        ExecuteRefundAsync(page, orderNos[0], fundAmount, 0, 0, password);
                        page.BodyPanel.Children.Remove(tipConfirmBox);
                    }, closeAction: (obj, eventArgs) =>
                    {
                        page.BodyPanel.Children.Remove(obj as TipConfirmBox);
                    }, isPassword: isEnabledPassword);
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderRefundAsync 异常", ex);
            }
        }

        public async void ExecuteRefundAsync(CardOrder page, string orderNo, decimal refundAmount, decimal totalAmount, decimal receiveAmount, string refundPassword)
        {
            if (string.IsNullOrWhiteSpace(refundPassword))
            {
                refundPassword = await _onCashierRequest.FindRefundPassword(AccountCache.Persist.CashId);
            }

            var resp = await _onOrderRequest.ExecuteRefundAsync(orderNo, refundPassword, refundAmount);

            if (!(resp.IsSuccess) || (resp?.Data ?? null) == null)
            {
                _onTipRender.ExecuteTip(page.BodyPanel, resp?.Msg ?? "退款失败");
                return;
            }

            await this.RenderListAsync(page);

            #region 打印退款小票
            TextHelper.LogInfo($"打印退款小票储蓄卡退款小票开始");
            var printer = PrintFactory.GetPrintRender(_onDeviceSettingPlugins.GlobalConfig.PrintConfig);
            var printerResp = await printer.ExecuteRefundAsync(new Model.RefundContentRequest()
            {
                IsRepair = false,
                TotalAmount = totalAmount,
                ReceiptAmount = receiveAmount,
                RefundAmount = resp.Data.RefundFee,
                IsCard = true,
                PayType = resp.Data.PayType.ToPayType().GetDescription(),
                TradeNo = resp.Data.OutTradeNo,
                TradeTime = string.IsNullOrEmpty(resp.Data.RefundTime) ? DateTime.Now : Convert.ToDateTime(resp.Data.RefundTime)
            }, AccountCache.ToMerchantResponse(), true);
            #endregion

            _onSystemRender.ExecuteVideoAsync(refundAmount, EnumOptionPayType.Refund);
            _onTipRender.ExecuteTip(page.BodyPanel, "退款成功", UControl.EnumResultType.OK);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public async Task RenderPrinterAsync(CardOrder page)
        {
            try
            {
                #region 验证信息
                var orderNoList = GetSelectOrderNoList(page.DataGridOrderList);
                if ((orderNoList?.Count ?? 0) <= 0)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "请选择要打印的订单");
                    return;
                }

                var data = _onControlRender.ConvertObject<CardOrderItemResponse>(page.DataGridOrderList.Items);
                var orderList = FindOrderNos(data, orderNoList);

                if ((orderList?.Count ?? 0) <= 0)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, "未找到要打印的订单");
                    return;
                }
                #endregion

                var outOrderNoList = new List<string>();
                var printer = PrintFactory.GetPrintRender(_onDeviceSettingPlugins.GlobalConfig.PrintConfig);
                var respVerify = printer.Verify();
                if (!(respVerify?.IsSuccess ?? false))
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, $"{respVerify?.Msg}");
                    return;
                }
                foreach (var item in orderList)
                {
                    var resp = await ExecutePrinter(printer, item);
                    if (!resp)
                    {
                        outOrderNoList.Add(item.OrderNo);
                    }
                }
                if ((outOrderNoList?.Count ?? 0) > 0)
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, $"{string.Join(",", outOrderNoList.ToArray())} 打印失败");
                    return;
                }

                _onTipRender.ExecuteTip(page.BodyPanel, "打印成功", UControl.EnumResultType.OK);
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderPrinterAsync 异常", ex);
                _onTipRender.ExecuteTip(page.BodyPanel, "打印异常");
            }
        }

        public async Task RenderExport(CardOrder page)
        {
            try
            {
                TextHelper.LogInfo($"导出储蓄卡订单信息");
                var cardNo = page.txbCardNo.Text.Equals("请输入会员手机号或卡号") ? "" : page.txbCardNo.Text;
                var orderDataRequest = new Model.CardOrderRequest()
                {
                    CashId = AccountCache.Persist.CashId,
                    OrderNo = page.txbOrderNo.Text.Trim(),
                    Type = Convert.ToInt32(((ComboBoxItem)page.CmbOrderType.SelectedValue).Tag.ToString()),
                    PageIndex = 1,
                    StartTime = Convert.ToDateTime(page.DpStartTime.Text),
                    EndTime = Convert.ToDateTime(page.DpEndTime.Text),
                    Rows = 9999,
                    StoresId = AccountCache.Persist.StoreId,
                    PhoneOrCardNo = cardNo,
                };

                var resp = await _onOrderRequest.ExecuteQueryAsync(orderDataRequest);
                if (!(resp?.IsSuccess ?? false))
                {
                    _onTipRender.ExecuteTip(page.BodyPanel, resp?.Msg ?? "查询数据异常");
                    return;
                }

                var dt = resp.Data?.Items.ToModel();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "数据文件(*.xls)|*.xls";
                sfd.FilterIndex = 2;
                sfd.FileName = dt.TableName + ".xls";
                if (sfd.ShowDialog() == true)
                {
                    NPOIUtils.Export(dt, dt.TableName, sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderExport 异常", ex);
                _onTipRender.ExecuteTip(page.BodyPanel, "导出异常");
            }
        }

        /// <summary>
        /// 渲染全选
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="isChecked"></param>
        public void RenderCheckOption(DependencyObject parent, bool isChecked)
        {
            try
            {
                var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    var v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                    var child = v as CheckBox;

                    if (child == null)
                    {
                        RenderCheckOption(v, isChecked);
                    }
                    else
                    {
                        child.IsChecked = isChecked;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderCheckOption 异常", ex);
            }
        }

        /// <summary>
        /// 获取选中的订单列表
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<string> GetSelectOrderNoList(DependencyObject parent)
        {
            var result = new List<string>();
            try
            {
                var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    var v = (DependencyObject)VisualTreeHelper.GetChild(parent, i);
                    var child = v as CheckBox;
                    if (child == null)
                    {
                        var list = GetSelectOrderNoList(v);
                        if (list.Count > 0)
                            result.AddRange(list);
                    }
                    else
                    {
                        if (child.IsChecked ?? false)
                            result.Add(child.Tag.ToString());

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error("GetSelectOrderNoList 异常", ex);
            }
            return result;
        }

        #region 私有方法
        /// <summary>
        /// 根据订单列表 获得订单信息
        /// </summary>
        /// <param name="orderNos"></param>
        /// <returns></returns>
        private List<CardOrderItemResponse> FindOrderNos(List<CardOrderItemResponse> data, List<string> orderNos)
        {
            try
            {
                return data?.Where(item => orderNos.Contains(item.OrderNo))?.ToList();
            }
            catch (Exception ex)
            {
                TextHelper.Error("FindOrderNos 异常", ex);
                return null;
            }
        }

        private async Task<bool> ExecutePrinter(OnPrintBaseRender onPrintBase, CardOrderItemResponse orderItem)
        {
            var isRefund = orderItem.Type == (int)CardOrderTypeEnums.Refund;
            if (isRefund)
            {
                var refundResp = await onPrintBase.ExecuteRefundAsync(orderItem.ToRefundModel(), AccountCache.ToMerchantResponse(), false);
                return refundResp?.IsSuccess ?? false;
            }

            var resp = await onPrintBase.ExecutePrintAsync(orderItem.ToModel(), AccountCache.ToMerchantResponse(), false);
            return resp?.IsSuccess ?? false;
        }
        #endregion
    }
}
