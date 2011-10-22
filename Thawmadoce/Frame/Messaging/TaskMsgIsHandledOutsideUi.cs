using MemBus.Publishing;
using MemBus.Setup;

namespace Thawmadoce.Frame.Messaging
{
    public class TaskMsgIsHandledOutsideUi : ISetup<IConfigurableBus>
    {
        public void Accept(IConfigurableBus setup)
        {
            setup.ConfigurePublishing(DispatchUiMessages);
        }

        private static void DispatchUiMessages(IConfigurablePublishing obj)
        {
            obj.MessageMatch(mi => mi.Name.EndsWith("TaskMsg")).PublishPipeline(new ParallelNonBlockingPublisher());
        }
    }
}