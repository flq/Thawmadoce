using System.Diagnostics;
using System.IO;
using System.Reflection;
using Thawmadoce.Frame.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using StructureMap;

namespace Thawmadoce.Bootstrapping
{

    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        private IContainer _container;

        /// <summary>
        /// By default, we are configured to use MEF
        /// </summary>
        protected override void Configure()
        {
            ObjectFactory.Initialize(i => i.AddRegistry<BootstrapRegistry>());
            _container = ObjectFactory.Container;
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
                service = typeof(object);
            var returnValue = key == null ? _container.GetInstance(service) : _container.GetInstance(service, key);
            return returnValue;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service).OfType<object>();
        }

        protected override IEnumerable<System.Reflection.Assembly> SelectAssemblies()
        {
            var plugins = new List<Assembly>();
            if (Directory.Exists("plugins"))
            {
                foreach (var f in Directory.GetFiles("plugins", "*.dll"))
                {
                    try
                    {
                        plugins.Add(Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), f)));
                    }
                    catch (Exception x)
                    {
                        Debug.WriteLine("Assembly load failed!");
                    }
                }
            }
            return GetType().Assembly.ToEnumerable().Concat(plugins);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            _container.GetAllInstances<IStartupTask>().ForEach(t => t.Run());
            base.OnStartup(sender, e);
        }
    }
}
