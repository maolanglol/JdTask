using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jlion.BrushClient.Client.LoginPersist;
using Jlion.BrushClient.Framework;

namespace Jlion.BrushClient.Client.Interceptor
{
    /// <summary>
    /// 统一操作日志Aop拦截器
    /// </summary>
    public class LoggerInterceptor : IInterceptor
    {
        public LoggerInterceptor()
        {
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            var businessName = GetLoggerAttribute(invocation);
            if (string.IsNullOrWhiteSpace(businessName))
            {
                invocation.Proceed();
                return;
            }

            var arguments = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString())
                .Where(item => !item.Contains("WP.Device.Plugins") && !item.Contains("System.Threading.Tasks.Task")).ToArray());

            //TextHelper.LogInfo($"收银员：{AccountCache.Persist?.CashId ?? 0}->{businessName} 执行开始 方法名称：{invocation.Method.Name}  参数是：{arguments}", logEnumsType: TextHelper.LogEnumsType.LogsDetail);
            //在被拦截的方法执行完毕后 继续执行
            invocation.Proceed();

            //TextHelper.LogInfo($"收银员：{AccountCache.Persist?.CashId ?? 0}->{businessName} {arguments} 执行结束 返回结果：{invocation.ReturnValue}", logEnumsType: TextHelper.LogEnumsType.LogsDetail);
        }

        /// <summary>
        /// 获得日志特性信息
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private string GetLoggerAttribute(IInvocation invocation)
        {
            try
            {
                var attributes = invocation.Method.GetCustomAttributes(true)?.ToList();
                foreach (var item in attributes)
                {
                    if (item.GetType() == typeof(LoggerAttribute))
                        return ((LoggerAttribute)item).BusinessName;
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }
    }
}
