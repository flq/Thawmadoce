using Caliburn.Micro;
using StructureMap;

namespace Thawmadoce.Frame
{
    public class UserInteraction : IUserInteraction
    {
        private readonly IContainer _container;

        public UserInteraction(IContainer container)
        {
            _container = container;
        }

        public IDialogStarter Dialog<VM>()
        {
            return _container.GetInstance<DialogStarter<VM>>();
        }
    }

    public class DialogStarter<VM> : IDialogStarter
    {
        private readonly IWindowManager _mgr;
        private readonly IContainer _container;

        public DialogStarter(IWindowManager mgr, IContainer container)
        {
            _mgr = mgr;
            _container = container;
        }

        public void Run()
        {
            var vm = _container.GetInstance<VM>();
            _mgr.ShowDialog(vm);
        }

        public I Run<I>(I arguments) where I : IUserCancelled
        {
            var vm = _container.With(arguments).GetInstance<VM>();
            var result = _mgr.ShowDialog(vm);
            arguments.UserCancelled = result.HasValue && !result.Value;
            return arguments;
        }
    }
}