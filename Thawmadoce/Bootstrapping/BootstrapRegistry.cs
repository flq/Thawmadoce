using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using MemBus;
using MemBus.Configurators;
using MemBus.Subscribing;
using StructureMap;
using StructureMap.Configuration.DSL;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Messaging;

namespace Thawmadoce.Bootstrapping
{
    public class BootstrapRegistry : Registry
    {
        public BootstrapRegistry()
        {
            ForSingletonOf<IWindowManager>().Use(new WindowManager());
            ForSingletonOf<ShellViewModel>().Use<ShellViewModel>();
            For<IGestureService>().Use(ctx => ctx.GetInstance<ShellViewModel>().GestureService);
            Forward<ShellViewModel,IGestureService>();

            For<Application>().Use(Application.Current);
            Forward<Application,DispatcherObject>();
            ForSingletonOf<IDispatchServices>().Use<WpfDispatchService>();

            ForSingletonOf<IBus>().Use(ConstructBus);
            For(typeof(IObservable<>)).Use(typeof(MessageObservable<>));
            Forward<IBus, IPublisher>();
            Forward<IBus, ISubscriber>();

            Scan(s =>
            {
                s.TheCallingAssembly();
                if (Directory.Exists("plugins")) //StructureMap bails out if it doesn't exist
                  s.AssembliesFromPath("plugins");
                s.AddAllTypesOf<IStartupTask>();
                s.AddAllTypesOf<ISaga>();
                s.Convention<HandlerRegistrationOnViewModels>();
            });

            SetAllProperties(c => c.TypeMatches(type => type.Name.EndsWith("ViewModel")));
            
        }

        private static IBus ConstructBus()
        {

            return BusSetup.StartWith<Conservative>(
                new IoCSupport(new StructuremapBridge(() => ObjectFactory.Container)))
                .Apply<FlexibleSubscribeAdapter>(c => c.ByMethodName("Handle"))
                .Apply<PassViewModelMessagesThroughViewActivation>()
                .Apply<UiMsgMustBeDispatched>()
                .Apply<TaskMsgIsHandledOutsideUi>()
                .Construct();
        }
    }
}