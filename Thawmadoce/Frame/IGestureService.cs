using System.Windows.Input;

namespace Thawmadoce.Frame
{
    public interface IGestureService
    {
        void AddKeyBinding(KeyBinding binding, object scope = null);
        void RemoveInputBindings(object scope);
    }
}