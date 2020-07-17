using Jlion.BrushClient.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Client.OnRender
{
    public class OnTimerRender : OnSingleRender
    {
        int currentHeight = 0;
        int areHeight = 0;
        System.Windows.Forms.Timer timeDown;
        Browser browser;
        private Action action;


        public OnTimerRender()
        {
        }


        public void Start(Browser browser, int height = 0, Action action = null)
        {
            this.browser = browser;
            currentHeight = 0;
            if (timeDown == null)
            {
                timeDown = new System.Windows.Forms.Timer();
                timeDown.Tick += TimeDown_Tick;
                timeDown.Interval = 200;
            }
            this.action = action;
            this.areHeight = height;
            timeDown.Enabled = true;
        }


        public void Stop()
        {
            if (timeDown != null)
            {
                timeDown.Enabled = false;
            }
        }

        private void TimeDown_Tick(object sender, EventArgs e)
        {
            try
            {
                var doc = (mshtml.HTMLDocument)browser.clientBrowser.Document;
                int height = areHeight <= 0 ? doc.parentWindow.screen.height/5 : areHeight;
                currentHeight += 5;
                doc.parentWindow.scrollBy(0, currentHeight);
                if (currentHeight >= height)
                {
                    this.action?.Invoke();
                    currentHeight = 0;
                    timeDown.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error($"TimeDown_Tick 异常:{ex.Message}",ex);
            }
        }

    }
}
