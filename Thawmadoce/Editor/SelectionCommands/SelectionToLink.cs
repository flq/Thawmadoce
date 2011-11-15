using System;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class SelectionToLink : SelectionCommand
    {
        private readonly IUserInteraction _userInteraction;

        public SelectionToLink(TextContext selectionText, IUserInteraction userInteraction) : base(selectionText)
        {
            _userInteraction = userInteraction;
        }

        protected override TextContext Execute()
        {
            var args = _userInteraction.Dialog<EnterLinkViewModel>().Run(new LinkArgs());
            if (args.UserCanceled)
                return TextContext;
            var nextRefId = TextContext.NextReferenceId;
            TextContext.ReplaceSelection("[{0}][{1}]", TextContext.CurrentSelection, nextRefId);
            TextContext.AppendReference(args.Link);
            return TextContext;
        }
    }
}