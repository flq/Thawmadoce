using System;
using MemBus.Publishing;
using MemBus.Setup;
using StructureMap;

namespace Thawmadoce.Frame.Messaging
{
    public class UiMsgMustBeDispatched : ISetup<IConfigurableBus>
    {
        public void Accept(IConfigurableBus setup)
        {
            setup.ConfigurePublishing(DispatchUiMessages);
        }

        private static void DispatchUiMessages(IConfigurablePublishing obj)
        {
            obj.MessageMatch(mi => mi.Name.EndsWith("UiMsg")).PublishPipeline(
                new UiMsgDispatcher(ObjectFactory.GetInstance<IDispatchServices>()));
        }


        private class UiMsgDispatcher : IPublishPipelineMember
        {
            private readonly IDispatchServices _svc;
            private readonly SequentialPublisher _seq = new SequentialPublisher();

            public UiMsgDispatcher(IDispatchServices svc)
            {
                _svc = svc;
            }

            public void LookAt(PublishToken token)
            {
                _svc.EnsureActionOnDispatcher(()=> _seq.LookAt(token));
            }
        }
    }
}