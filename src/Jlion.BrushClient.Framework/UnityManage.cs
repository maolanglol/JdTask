using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;

namespace Jlion.BrushClient.Framework
{
    public static class AutofacManage
    {
        private static UnityContainer container;
        private static VirtualMethodInterceptor virtualMethodInterceptor = new VirtualMethodInterceptor();
        private static bool isAlreadyAddInterception = false;

        public static void Init()
        {
            AutofacManage.Build();
        }

        public static void Register(Action<UnityContainer> action)
        {
            action(AutofacManage.container);
        }

        public static T GetService<T>()
        {
            try
            {
                return container.Resolve<T>();
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }


        private static void Build()
        {
            AutofacManage.container = new UnityContainer();
        }

        public static List<Type> GetChildTypes(string assemblyName)
        {
            var resp = new List<Type>();
            var types = Assembly.Load(assemblyName).GetTypes().ToList();

            types?.ForEach(item =>
            {
                if (item.Namespace.Equals(assemblyName))
                {
                    resp.Add(item);
                }
            });
            return resp;
        }

        public static List<Type> GetCallingTypes(this Type baseType)
        {
            var resp = new List<Type>();
            var types = AppDomain.CurrentDomain.GetAssemblies().Select(item => item.GetTypes().ToList()).ToList();

            types?.ForEach(item =>
            {
                item.ForEach(p =>
                {
                    if (p.BaseType == baseType)
                    {
                        resp.Add(p);
                    }
                });
            });
            return resp;
        }


        public static void RegisterAssemblyTypesBySingleton(this UnityContainer container, List<Type> types)
        {
            if (container == null)
                return;

            types.ForEach(item =>
            {
                container.RegisterSingleton(item);
            });
        }

        public static void RegisterAssemblyTypesBySingleton(this UnityContainer container, string assemblyName)
        {
            var list = GetChildTypes(assemblyName);
            container.RegisterAssemblyTypesBySingleton(list);
        }

        public static void RegisterAssemblyTypesBySingleton(this UnityContainer container, Type baseTypes)
        {
            var list = GetCallingTypes(baseTypes);
            container.RegisterAssemblyTypesBySingleton(list);
        }

        public static void RegisterAssemblyTypesBySingletonExclude(this UnityContainer container, Type baseTypes, Type excludeTypes)
        {
            var list = GetCallingTypes(baseTypes);
            list = list.Where(item => item != excludeTypes).ToList();
            container.RegisterAssemblyTypesBySingleton(list);
        }

        public static void RegisterAssemblyTypes(this UnityContainer container, Type baseTypes)
        {
            var list = GetCallingTypes(baseTypes);

            container.RegisterAssemblyTypes(list);
        }

        public static void RegisterAssemblyTypes(this UnityContainer container, List<Type> types)
        {
            if (container == null)
                return;

            types?.ForEach(item =>
            {
                container.RegisterType(item);
            });
        }

        public static void SetInterceptor(this UnityContainer container, Type baseTypes)
        {
            var list = GetCallingTypes(baseTypes);
            container.SetInterceptorFor(list);
        }

        private static void SetInterceptorFor(this UnityContainer container, List<Type> types)
        {
            if (container == null)
                return;

            //多次添加拦截器，会进行多次重复拦截
            if (!isAlreadyAddInterception)
            {
                container.AddNewExtension<Interception>();
                isAlreadyAddInterception = true;
            }

            types.ForEach(item =>
            {
                if (HasLoggerAttribute(item))
                    container.Configure<Interception>().SetInterceptorFor(item, virtualMethodInterceptor);
            });
        }

        private static bool HasLoggerAttribute(Type type)
        {
            var members = type.GetMembers();
            string logAttribute = "WP.Device.Plugins.Client.Interceptor.LoggerAttribute";
            foreach (var member in members)
            {
                var attrs = member.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    if (attr?.ToString() == logAttribute)
                        return true;
                }
            }
            return false;
        }
    }
}
