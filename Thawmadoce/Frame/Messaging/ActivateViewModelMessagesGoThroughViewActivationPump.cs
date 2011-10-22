using MemBus.Publishing;
using MemBus.Setup;
using StructureMap;

namespace Thawmadoce.Frame.Messaging
{
    internal class ActivateViewModelMessagesGoThroughViewActivationPump : ISetup<IConfigurableBus>
    {
        public void Accept(IConfigurableBus setup)
        {
            setup.ConfigurePublishing(PublishPipelineForViewActivationMessages);
        }

        private static void PublishPipelineForViewActivationMessages(IConfigurablePublishing obj)
        {
            obj.MessageMatch(mi => mi.IsType<ActivateViewModelMsg>()).PublishPipeline(
                new DeferredPublishPipelineMember<ViewActivationPump>(ObjectFactory.GetInstance<ViewActivationPump>),
                new SequentialPublisher());
        }
    }
}