using System;
using System.Windows;
using Caliburn.Micro;
using Thawmadoce.Editor;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce {
    
    public class ShellViewModel : Screen
    {
        private readonly GestureService _gestureSvc = new GestureService();
        private string _title = "The awesome markdown centrifuge";

        public MarkdownEditorViewModel Editor { get; set; }


        public IGestureService GestureService
        {
            get { return _gestureSvc; }
        }

        protected override void OnActivate()
        {
            this.ActivateAllChilds();
            DisplayName = _title;
        }

        protected override void OnViewAttached(object view, object context)
        {
            _gestureSvc.AttachView((UIElement)view);
        }

        
    }
}
