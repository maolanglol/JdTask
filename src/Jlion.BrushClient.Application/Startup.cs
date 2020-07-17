using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using WP.Device.Plugins.Application.Plugins;
using WP.Device.Plugins.Framework;

namespace WP.Device.Plugins.Application
{
    public class Startup
    {

        public static void Register(Action<ContainerBuilder> configure = null)
        {
            AutofacManage.Register(builder =>
            {
                ///插件单例注入
                var basetype = typeof(OnBasePlugins);
                var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToList();
                builder.RegisterAssemblyTypes(assemblys.ToArray())
                    .Where(t => basetype.IsAssignableFrom(t) && t != basetype).SingleInstance();

                //builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();
                //Assembly assemblies = Assembly.Load("WP.Device.Plugins.Framework");

                ////builder.RegisterAssemblyTypes(assembliesRepository).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().SingleInstance();
                //builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Plugins")).AsImplementedInterfaces().SingleInstance();
                configure?.Invoke(builder);
            });

            AutofacManage.Init();

            //MapperConfig.Init();
        }

        /// <summary>
        /// 全局启动设备相关插件服务
        /// </summary>
        /// <param name="onDeviceSettingPlugins"></param>
        public static void Start(OnDeviceSettingPlugins onDeviceSettingPlugins)
        {
            ////TODO 全局启动
            //#region 注册插件Demo

            //#region 注册全局键盘钩子
            //DeviceGlobalManage.Register((data) =>
            //{
            //    if (data.IsValid)
            //    {
            //        this.txbPayCode.Text = data.Code;
            //    }
            //});
            //#endregion


            //#region 注册OCR 图片文字识别插件
            ////注册图片查找钩子
            //DeviceGlobalManage.OrcRegister((data, bit) =>
            //{
            //    UpdateValueMethod myDelegate = new UpdateValueMethod(UpdateValue);
            //    base.Dispatcher.BeginInvoke(myDelegate, data, bit);
            //});
            //#endregion

            //#region 注册全局鼠标钩子
            ////DeviceGlobalManage.MouseRegister(MouseMoveEventHandler, MouseDoubleEvent);
            //#endregion

            //#region 注册全局窗口查找钩子
            //DeviceGlobalManage.PointRegister(PointTimerHandler);
            //#endregion

            //#endregion
        }

        /// <summary>
        /// 全局停止设备相关插件服务
        /// </summary>
        public static void Stop()
        {
            //TODO 全局停止 GlobalDeviceManage
        }
    }
}
