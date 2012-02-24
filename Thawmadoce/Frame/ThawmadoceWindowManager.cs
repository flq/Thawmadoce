using System.Windows;
using Caliburn.Micro;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Frame
{
    public class ThawmadoceWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var wdw = base.EnsureWindow(model, view, isDialog);
            if (!(model is INeedRemoteControl)) return wdw;
            return SetRemote(model, wdw);
        }

        private static Window SetRemote(object rootModel, Window wdw)
        {
            var remote = new DialogRemoteControl(wdw);
            rootModel.As<INeedRemoteControl>(r => r.Accept(remote));
            return wdw;
        }
    }
}