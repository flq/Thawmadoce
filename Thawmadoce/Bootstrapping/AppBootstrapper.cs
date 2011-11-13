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
            return AssemblyPool.ApplicationAssemblies();
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
