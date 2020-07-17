using Autofac;
using Autofac.Extras.DynamicProxy2;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Jlion.BrushClient.Application;
using Jlion.BrushClient.Application.Plugins;
using Jlion.BrushClient.Client.Helper;
using Jlion.BrushClient.Client.Interceptor;
using Jlion.BrushClient.Client.OnRender;
using Jlion.BrushClient.Framework;
using Jlion.BrushClient.Framework.Helper;
using static Jlion.BrushClient.Framework.TextHelper;
using Unity;

namespace Jlion.BrushClient.Client
{
    public class Startup
    {

        public static void Register(Action<UnityContainer> configure = null)
        {
            AutofacManage.Init();

            AutofacManage.Register(builder =>
            {
                //插件注入
                var basetype = typeof(OnBasePlugins);
                var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList().Select(item => item);

                builder.RegisterAssemblyTypes(basetype);

                ////OnBaseRequest 注入
                //var baseRequestType = typeof(OnBaseRequest);
                //builder.RegisterAssemblyTypes(baseRequestType);

                ////OnSignleRequest 注入
                //var baseSignleRequestType = typeof(OnSingleRequest);
                //builder.RegisterAssemblyTypesBySingleton(baseSignleRequestType);

                //OnSingleRender 注入
                var baseRenderType = typeof(OnSingleRender);
                builder.RegisterAssemblyTypesBySingleton(baseRenderType);
                //OnSingleRender 拦截
                builder.SetInterceptor(baseRenderType);


                //窗体注入
                var baseWindowType = typeof(Window);
                //var perDependencyWinidowType = typeof(PayResult);
                builder.RegisterAssemblyTypesBySingleton(baseWindowType);

                var baseScopeRenderType = typeof(OnPerDependencyRender);
                builder.RegisterAssemblyTypes(baseScopeRenderType);

                configure?.Invoke(builder);
            });
            //MapperConfig.Init();
        }

        //public static void Register(Action<ContainerBuilder> configure = null)
        //{
        //    //AutofacManage.Register(builder =>
        //    //{

        //    //    //builder.Register(c => new LoggerInterceptor());

        //    //    ////插件注入
        //    //    //var basetype = typeof(OnBasePlugins);
        //    //    //var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList();

        //    //    //builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    //    .Where(t => basetype.IsAssignableFrom(t) && t != basetype).SingleInstance();

        //    //    //////IBuilder 注入
        //    //    ////var baseBuilder = typeof(IBuilder);

        //    //    ////builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    ////    .Where(t => baseBuilder.IsAssignableFrom(t) && t != baseBuilder).SingleInstance();

        //    //    ////OnBaseRequest 注入
        //    //    //var baseRequestType = typeof(OnBaseRequest);
        //    //    //builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    //    .Where(t => baseRequestType.IsAssignableFrom(t) && t != baseRequestType).InstancePerDependency();

        //    //    ////OnSignleRequest 注入
        //    //    //var baseSignleRequestType = typeof(OnSingleRequest);
        //    //    //builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    //    .Where(t => baseSignleRequestType.IsAssignableFrom(t) && t != baseSignleRequestType).InstancePerDependency();

        //    //    ////OnSingleRender 注入
        //    //    //var baseRenderType = typeof(OnSingleRender);
        //    //    //builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    //    .Where(t => baseRenderType.IsAssignableFrom(t) && t != baseRenderType).SingleInstance()
        //    //    //    .InterceptedBy(typeof(LoggerInterceptor)).EnableClassInterceptors();

        //    //    //var baseDynamicType = typeof(IDynamicMetaObjectProvider);
        //    //    //builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    //  .Where(t => baseDynamicType.IsAssignableFrom(t) && t != baseDynamicType).SingleInstance();

        //    //    ////窗体注入
        //    //    //var baseWindowType = typeof(Window);
        //    //    ////builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    ////    .Where(t => t != baseWindowType).InstancePerDependency();

        //    //    //builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    //    .Where(t => baseWindowType.IsAssignableFrom(t) && t != baseWindowType).SingleInstance();

        //    //    //var baseScopeRenderType = typeof(OnPerDependencyRender);
        //    //    //builder.RegisterAssemblyTypes(assemblys.ToArray())
        //    //    //    .Where(t => baseScopeRenderType.IsAssignableFrom(t) && t != baseScopeRenderType).InstancePerDependency();

        //    //    //////启用类代理拦截
        //    //    //builder.RegisterType<OnSingleRender>().InterceptedBy(typeof(LoggerInterceptor)).EnableClassInterceptors();

        //    //    //configure?.Invoke(builder);
        //    //});

        //    //AutofacManage.Init();

        //    //MapperConfig.Init();
        //}

        /// <summary>
        /// 全局启动设备相关插件服务
        /// </summary>
        public static void Start()
        {
            Register();
            MoveSettings();
        }

        #region 私有方法
        /// <summary>
        /// 配置文件迁移
        /// </summary>
        private static void MoveSettings()
        {
            var dbPath = DocumentDefintion.DocumentPath;

            try
            {
                var appSettings = AutofacManage.GetService<OnAppSettingPlugins>();
                if (!Directory.Exists(dbPath))
                {
                    Directory.CreateDirectory(dbPath);
                }
            }
            catch (Exception ex)
            {
                TextHelper.ErrorAsync($"配置迁移错误：{ex.Message}");
            }
        }
        #endregion
    }
}
