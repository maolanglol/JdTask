using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jlion.BrushClient.Framework
{
    /// <summary>
    /// 依赖注入管理器
    /// </summary>
    public class AutofacManage
    {
        private static readonly ContainerBuilder builder = new ContainerBuilder();

        public static IContainer container;

        public static void Init()
        {
            AutofacManage.Build();
        }

        public static void Register(Action<ContainerBuilder> action)
        {
            action(AutofacManage.builder);
        }

        public static T GetService<T>()
        {
            return ResolutionExtensions.Resolve<T>(AutofacManage.container);
        }

        public static T GetService<T>(string name, object obj)
        {
            return ResolutionExtensions.Resolve<T>(AutofacManage.container, new Parameter[]
            {
                new NamedParameter(name, obj)
            });
        }

        public static T GetService<T>(IDictionary<string, object> dict)
        {
            if (dict == null || dict.Count == 0)
            {
                return AutofacManage.GetService<T>();
            }
            NamedParameter[] array = new NamedParameter[dict.Count];
            int num = 0;
            foreach (KeyValuePair<string, object> current in dict)
            {
                array[num++] = new NamedParameter(current.Key, current.Value);
            }
            IComponentContext _contain = AutofacManage.container;
            Parameter[] paramer = array;
            return ResolutionExtensions.Resolve<T>(_contain, paramer);
        }

        private static void Build()
        {
            AutofacManage.container = AutofacManage.builder.Build(0);
        }

    }
}
