using System;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// A simple wrapper to MemBus publishing which allows 
    /// you to publish messages from your plugin without having to reference
    /// Membus
    /// </summary>
    public interface IMessagePublisher
    {
        void Publish(object message);
    }
}