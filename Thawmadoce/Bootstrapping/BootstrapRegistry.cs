using Caliburn.Micro;
using MemBus;
using Scal.Services;
using StructureMap.Configuration.DSL;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using Thawmadoce.Settings;

namespace Thawmadoce.Bootstrapping
{
    public class BootstrapRegistry : Registry
    {
        public BootstrapRegistry()
        {
            ForSingletonOf<IWindowManager>().Use<ThawmadoceWindowManager>();
            ForSingletonOf<IUserInteraction>().Use<UserInteraction>();
            ForSingletonOf<ShellViewModel>().Use<ShellViewModel>();
            For<IGestureService>().Use(ctx => ctx.GetInstance<ShellViewModel>().GestureService);

            ForSingletonOf<ISettings>().Use<SettingsImpl>();
            For<IMessagePublisher>().Use(ctx => new MessagePublisher(ctx.GetInstance<IPublisher>()));

            Scan(s =>
            {
                foreach (var a in AssemblyPool.ApplicationAssemblies())
                    s.Assembly(a);
                s.IncludeNamespace("Thawmadoce");
                s.AddAllTypesOf<ISelectionPlugin>();
                s.AddAllTypesOf<IAppItemsPlugin>();
                s.AddAllTypesOf<IKeyboardOnlyActions>();
            });

            SetAllProperties(c => c.TypeMatches(type => type.Name.EndsWith("ViewModel")));
        }
    }
}