using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.Helper;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnAboutRender : OnSingleRender
    {
        private OnAppSettingPlugins _onAppSettingPlugins;
        public OnAboutRender(OnAppSettingPlugins onAppSettingPlugins)
        {
            _onAppSettingPlugins = onAppSettingPlugins;
        }

        public void Render(About page)
        {
            try
            {
                page.labVersion.Content = _onAppSettingPlugins.Version;
            }
            catch (Exception ex)
            {
                TextHelper.Error("Render 异常", ex);
            }
        }
    }
}
