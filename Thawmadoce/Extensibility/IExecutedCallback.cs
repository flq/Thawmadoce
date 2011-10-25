using System;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Implemented by the convenience base class for <see cref="IVisibleCommand"/>
    /// </summary>
    public interface IExecutedCallback
    {
        void SetCallback(Action executedCallback);
    }
}