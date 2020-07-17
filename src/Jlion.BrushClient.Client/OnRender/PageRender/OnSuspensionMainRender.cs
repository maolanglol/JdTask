using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.Helper;
using Jlion.BrushClient.Client.Interceptor;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnSuspensionMainRender : OnSingleRender
    {

        public OnTimerPlugins _onTimerPlugins;
        public OnControlRender _onControlRender;

        public OnSuspensionMainRender(OnTimerPlugins onTimerPlugins,
            OnControlRender onControlRender)
        {
            _onControlRender = onControlRender;

            _onTimerPlugins = onTimerPlugins;
        }

        public new void DragMove(Window window)
        {
            base.DragMove(window);
        }
        /// <summary>
        /// 执行菜单
        /// </summary>
        /// <param name="page"></param>
        public void ExecuteMenu(SuspensionMain page)
        {
            try
            {
                if (page.childMenus.Visibility == Visibility.Hidden)
                {
                    this.RenderShow(page);
                }
                else
                {
                    this.RenderHidden(page);
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error("ExecuteMenu", ex);
            }
        }

        public void RenderShow(SuspensionMain page)
        {
            try
            {
                page.childMenus.Visibility = Visibility.Visible;
                page.Height = 500;
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderShow 异常", ex);
            }
        }

        public void RenderHidden(SuspensionMain page)
        {
            try
            {
                page.childMenus.Visibility = Visibility.Hidden;
                page.Height = 110;
            }
            catch (Exception ex)
            {
                TextHelper.Error("RenderHidden 异常", ex);
            }
        }

        //public void RenderTopMost(SuspensionMain page)
        //{
        //    _onTimerPlugins.Add(TimeKeyStatics.TopMostKey(nameof(SuspensionMain)), () =>
        //    {
        //        _onControlRender.ThreadExecuteUI(() =>
        //        {
        //            page.Topmost = false;
        //            page.Topmost = true;
        //        });
        //        return false;
        //    }, 500, DateTime.Now);
        //}
    }
}
