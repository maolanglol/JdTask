using Jlion.BrushClient.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnRedirectRender: OnSingleRender
    {
        private OnTipRender _onTipRender;
        private OnControlRender _onControlRender;
        public OnRedirectRender(OnTipRender onTipRender,OnControlRender onControlRender)
        {
            _onTipRender = onTipRender;
            _onControlRender = onControlRender;
        }

        public void RedirectLogin()
        {
            _onControlRender.ThreadExecuteUI(() =>
            {
                var login = AutofacManage.GetService<Login>();
                login.ShowLogin();
            });
        }
    }
}
