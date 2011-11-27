using System.Windows;
using Caliburn.Micro;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Frame
{
    public class ThawmadoceWindowManager : WindowManager
    {
        protected override System.Windows.Window CreateWindow(object rootModel, bool isDialog, object context)
        {
            var wdw = base.CreateWindow(rootModel, isDialog, context);
            if (!(rootModel is INeedRemoteControl)) return wdw;
            return SetRemote(rootModel, wdw);
        }

        private static Window SetRemote(object rootModel, Window wdw)
        {
            var remote = new DialogRemoteControl(wdw);
            rootModel.As<INeedRemoteControl>(r => r.Accept(remote));
            return wdw;
        }
    }
}