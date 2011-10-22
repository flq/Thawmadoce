using System;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using MemBus;
using MemBus.Configurators;
using MemBus.Subscribing;
using StructureMap;
using StructureMap.Configuration.DSL;
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
                s.AddAllTypesOf<IStartupTask>();
                s.Convention<HandlerRegistrationOnViewModels>();
            });

            SetAllProperties(c => c.TypeMatches(type => type.Name.EndsWith("ViewModel")));
            
        }

        private static IBus ConstructBus()
        {

            return BusSetup.StartWith<Conservative>(
                new IoCSupport(new StructuremapBridge(() => ObjectFactory.Container)))
                .Apply<FlexibleSubscribeAdapter>(c => c.ByMethodName("Handle").ByInterface(typeof(IHandles<>)))
                .Apply<ActivateViewModelMessagesGoThroughViewActivationPump>()
                .Construct();
        }
    }
}