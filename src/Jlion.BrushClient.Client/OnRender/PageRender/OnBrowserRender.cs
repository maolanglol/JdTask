using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Client.Model.Response;
using Jlion.BrushClient.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnBrowserRender : OnSingleRender
    {
        int taskId = 0;
        Browser browser;
        OnTimerPlugins _onTimerPlugins;
        OnTimerRender _onTimerRender;
        OnTipRender _onTipRender;
        OnMainHostRequestPlugins _onMainHostRequestPlugins;


        OnControlRender _onControlRender;
        public OnBrowserRender(
            OnTipRender onTipRender,
            OnTimerRender onTimerRender,
            OnControlRender onControlRender,
            OnTimerPlugins onTimerPlugins, OnMainHostRequestPlugins onMainHostRequestPlugins)
        {
            _onControlRender = onControlRender;
            _onTimerRender = onTimerRender;
            _onTipRender = onTipRender;

            _onTimerPlugins = onTimerPlugins;
            _onMainHostRequestPlugins = onMainHostRequestPlugins;

        }
        public void RenderInit(Browser browser)
        {
            this.browser = browser;
            browser.clientBrowser.Navigate(ConstDefintion.JdUrl.HomeUrl);
            browser.clientBrowser.LoadCompleted += ClientBrowser_LoadCompleted;
        }

        public void Start()
        {
            ExecuteTask();
        }

        public void Stop()
        {
            try
            {
                _onTimerPlugins.ClearAll();
                _onTimerRender?.Stop();
            }
            catch (Exception ex)
            {
                TextHelper.Error($"停止失败 message:{ex.Message}", ex);
            }
        }

        public void RenderInitTask(string key = "衣服")
        {
            try
            {
                TextHelper.LogInfo($"领取任务成功....key:{key}");
                UpdateContent("领取任务成功,做任务中");
                _onControlRender.ThreadExecuteUI(() =>
                {
                    var searchUrl = string.Format(ConstDefintion.JdUrl.SearchUrl, System.Web.HttpUtility.UrlEncode(key));
                    browser.clientBrowser.Navigate(searchUrl);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error($"RenderInitTask 异常", ex);
            }
        }

        private void UpdateContent(string content, bool isTrue = false)
        {
            _onControlRender.ThreadExecuteUI(() =>
            {
                this.browser.labContent.Content = content;
                if (isTrue)
                {
                    var suspensionMain = AutofacManage.GetService<SuspensionMain>();
                    suspensionMain.InitAccount();
                }
            });
        }

        public void ExecuteTask()
        {
            _onTimerPlugins.Add(TimeKeyStatics.TaskKey("task"), () =>
            {
                //if (this.taskId > 0)
                //    return false;

                UpdateContent("领取任务中");
                var taskList = _onMainHostRequestPlugins.GetTaskAsync(AccountCache.Persist.AccessToken).Result;

                if ((taskList?.Data?.Count ?? 0) <= 0)
                {
                    UpdateContent("当前没有任务");
                    return false;
                }

                var task = taskList.Data[0];
                var taskId = task.TaskId;
                this.taskId = taskId;

                RenderInitTask(task.SearchKey);
                return false;
            }, 5 * 60 * 1000, DateTime.Now);
        }

        #region 私有方法

        private void Redirect(string url)
        {
            try
            {
                _onControlRender.ThreadExecuteUI(() =>
                {
                    if (!url.IsStartWithHttp())
                    {
                        url = $"http://{url}";
                    }
                    browser.clientBrowser.Navigate(url);
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error($"Redirect 异常", ex);
            }
        }

        private void ClientBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                mshtml.HTMLDocument doms = (mshtml.HTMLDocument)browser.clientBrowser.Document;
                var uri = browser.clientBrowser.Source;
                var homeUrl = uri.ToString();

                TextHelper.LogInfo($"加载任务：{homeUrl}");

                if (Regex.IsMatch(homeUrl, ConstDefintion.JdUrl.Parter))
                {
                    TextHelper.LogInfo($"查找任务,任务匹配成功:{homeUrl}....");

                    var height = ((mshtml.IHTMLElement2)doms.body).scrollHeight;
                    _onTimerRender.Start(this.browser, action: () =>
                    {
                        Task.Factory.StartNew(async () =>
                        {
                            try
                            {
                                if (this.taskId <= 0)
                                {
                                    UpdateContent("任务金额已经满额,请等下次任务哦");
                                    this.taskId = 0;
                                    return;
                                }
                                var postTaskResult = await _onMainHostRequestPlugins.PostTaskAsync(this.taskId, AccountCache.Persist.AccessToken);
                                if ((postTaskResult?.Data?.Amount ?? 0) <= 0)
                                {
                                    UpdateContent("任务金额已经满额,请等下次任务哦");
                                    this.taskId = 0;
                                    return;
                                }
                                UpdateContent($"任务完成,获得佣金{postTaskResult.Data.Amount}元,等待下一个任务", true);
                                this.taskId = 0;
                            }
                            catch (Exception ex)
                            {
                                this.taskId = 0;
                                TextHelper.Error($"提交任务失败", ex);
                            }
                        });
                    });
                    return;
                }

                #region 获得目标任务Url
                var list = GetItemList(homeUrl);
                var url = GetItem(list);
                if (string.IsNullOrWhiteSpace(url))
                {
                    TextHelper.LogInfo($"没有找到匹配的任务");
                    UpdateContent($"没有找到任务", false);
                    return;
                }
                #endregion

                #region 搜索UI中定位到目标Url位置,滚动
                var htmlLinkUrl = FindHtmlElement(url);
                if (htmlLinkUrl == null)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        if (this.taskId <= 0)
                        {
                            UpdateContent("任务金额已经满额,请等下次任务哦");
                            this.taskId = 0;
                            return;
                        }
                        var postTaskResult = await _onMainHostRequestPlugins.PostTaskAsync(this.taskId, AccountCache.Persist.AccessToken);
                        if ((postTaskResult?.Data?.Amount ?? 0) <= 0)
                        {
                            UpdateContent("任务金额已经满额,请等下次任务哦");
                            this.taskId = 0;
                            return;
                        }
                        UpdateContent($"任务完成,获得佣金{postTaskResult.Data.Amount}元,等待下一个任务", true);
                    });
                    //UpdateContent($"领取到任务,但是没有找到匹配的任务,请等待下一个任务");
                    //TextHelper.LogInfo($"领取到任务,但是没有找到匹配的任务,请等待下一个任务");
                    return;
                }
                if (htmlLinkUrl != null)
                {
                    TextHelper.LogInfo($"定位任务中...request:{JsonConvert.SerializeObject(htmlLinkUrl)}");
                    UpdateContent($"定位任务中...", false);
                    Thread.Sleep(5000);
                    _onTimerRender.Start(this.browser, htmlLinkUrl.ScrollHeigt, () =>
                    {
                        Redirect(htmlLinkUrl.Url);
                    });
                }
                #endregion
            }
            catch (Exception ex)
            {
                TextHelper.Error($"加载异常:{ex.Message}", ex);
                UpdateContent("任务领取成功,但是没有找到匹配的任务,请等待下一个任务");
            }
        }

        private HtmlElementUrl FindHtmlElement(string url)
        {
            mshtml.HTMLDocument doms = (mshtml.HTMLDocument)browser.clientBrowser.Document;
            var linkList = doms.links;
            foreach (var item in linkList)
            {
                var linkItem = ((mshtml.IHTMLAnchorElement)item);
                var scrollItem = ((mshtml.IHTMLElement2)item);
                var href = linkItem.href;
                if (href.Contains(url))
                {
                    return new HtmlElementUrl()
                    {
                        Url = href,
                        ScrollHeigt = scrollItem.scrollHeight
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// 解析商品地址
        /// </summary>
        /// <param name="searchUrl"></param>
        /// <returns></returns>
        private List<string> GetItemList(string searchUrl)
        {
            var response = new List<string>();
            try
            {
                var parter = ConstDefintion.JdUrl.Parter;
                using (var client = new HttpClient())
                {
                    var html = client.GetStringAsync(searchUrl).Result;
                    var match = Regex.Matches(html, parter);
                    foreach (var item in match)
                    {
                        response.Add(item.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error($"GetItemList 异常 error:{ex.Message}", ex);
            }
            return response;
        }

        private string GetItem(List<string> list)
        {
            try
            {
                if ((list?.Count ?? 0) <= 0)
                    return null;

                var rd = new Random();

                var index = rd.Next(1, list.Count);
                return list[index];
            }
            catch (Exception ex)
            {
                TextHelper.Error($"GetItem 异常 message:{ex.Message}", ex);
            }
            return null;
        }

        #endregion
    }
}
