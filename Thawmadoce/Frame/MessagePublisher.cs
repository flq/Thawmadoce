using MemBus;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Frame
{
    internal class MessagePublisher : IMessagePublisher
    {
        private readonly IPublisher _publisher;

        public MessagePublisher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Publish(object message)
        {
            _publisher.Publish(message);
        }
    }
}