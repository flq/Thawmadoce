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
using Thawmadoce.Settings;

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
            For<IMessagePublisher>().Use(ctx => new MessagePublisher(ctx.GetInstance<IPublisher>()));

            ForSingletonOf<ISettings>().Use<SettingsImpl>();

            Scan(s =>
            {
                foreach (var a in AssemblyPool.ApplicationAssemblies())
                    s.Assembly(a);
                s.IncludeNamespace("Thawmadoce");
                s.AddAllTypesOf<IStartupTask>();
                s.AddAllTypesOf<ISaga>();
                s.AddAllTypesOf<ISelectionPlugin>();
                s.AddAllTypesOf<IAppItemsPlugin>();
                s.AddAllTypesOf<IKeyboardOnlyActions>();
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