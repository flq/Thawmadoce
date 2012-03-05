using Scal;
using Scal.Configuration;
using DynamicXaml.Extensions;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Messaging;

namespace Thawmadoce.Bootstrapping
{
    public class AppConfiguration : ScalConfiguration
    {
        public AppConfiguration()
        {
            AssemblyPool
                .AddAssemblies(Bootstrapping.AssemblyPool.ApplicationAssemblies());

            Messaging.AddConfigurationArtefact(new PassViewModelMessagesThroughViewActivation());
            Messaging
                .HandleTheseMessagesAsynchronously(msg => msg.Name.EndsWith("TaskMsg"))
                .HandleTheseMessagesOnDispatcher(msg => msg.Name.EndsWith("UiMsg"))
                .TypesBeingAMessageHub(t => t.CanBeCastTo<ISaga>())
                .TypesSubscribedToMessaging(t => t.CanBeCastTo<AbstractViewModel>())
                .TypesSubscribedToMessaging(t => t.Name.EndsWith("ViewModel"));

            UnhandledExceptionsPassedTo<HandleExceptionOccurredMessages.UnhandledExceptionHandler>();
            
            StartViewModel<ShellViewModel>();
        }
    }
}