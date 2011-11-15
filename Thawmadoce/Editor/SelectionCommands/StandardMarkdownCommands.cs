using System.Collections.Generic;
using System.Windows.Input;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class StandardMarkdownCommands : ISelectionPlugin
    {
        private readonly IUserInteraction _userInteraction;

        public StandardMarkdownCommands(IUserInteraction userInteraction)
        {
            _userInteraction = userInteraction;
        }

        public IEnumerable<IVisibleCommand> GetCommands(TextContext selectionText)
        {
            yield return new EncloseSelectionInSomething(selectionText, "__")
                             {
                                 CommandText = "Bold",
                                 CommandIcon = "/Thawmadoce;component/Media/format_text_bold.png",
                                 KeyCombination = new KeyCombo(Key.B, ModifierKeys.Control | ModifierKeys.Shift)
                             };
            yield return new EncloseSelectionInSomething(selectionText, "_")
                             {
                                 CommandText = "Emphasis",
                                 CommandIcon = "/Thawmadoce;component/Media/format_text_italic.png",
                                 KeyCombination = new KeyCombo(Key.E, ModifierKeys.Control | ModifierKeys.Shift)
                             };
            yield return new EncloseSelectionInSomething(selectionText, "<sub>", "</sub>")
                             {
                                 CommandText = "Subscript",
                                 CommandIcon = "/Thawmadoce;component/Media/format_text_subscript.png",
                                 KeyCombination = new KeyCombo(Key.I, ModifierKeys.Control | ModifierKeys.Shift)
                             };
            yield return new EncloseSelectionInSomething(selectionText, "<sup>", "</sup>")
                             {
                                 CommandText = "Superscript",
                                 CommandIcon = "/Thawmadoce;component/Media/format_text_superscript.png",
                                 KeyCombination = new KeyCombo(Key.P, ModifierKeys.Control | ModifierKeys.Shift)
                             };
            yield return new EncloseSelectionInSomething(selectionText, "<u>", "</u>")
                             {
                                 CommandText = "Underline",
                                 CommandIcon = "/Thawmadoce;component/Media/format_text_underline.png",
                                 KeyCombination = new KeyCombo(Key.U, ModifierKeys.Control | ModifierKeys.Shift)
                             };

            yield return new EncloseSelectionInSomething(selectionText, "<strike>", "</strike>")
                             {
                                 CommandText = "Strike out",
                                 CommandIcon = "/Thawmadoce;component/Media/format_text_strikethrough.png",
                                 KeyCombination = new KeyCombo(Key.OemMinus, ModifierKeys.Control | ModifierKeys.Shift)
                             };
            yield return new PrependLines(selectionText, "> ")
                             {
                                 CommandText = "Quote",
                                 CommandIcon = "/Thawmadoce;component/Media/quote.png",
                                 KeyCombination = new KeyCombo(Key.Q, ModifierKeys.Control | ModifierKeys.Shift)
                             };
            yield return new PrependLines(selectionText, "1. ")
                             {
                                 CommandText = "Numbered list",
                                 CommandIcon = "/Thawmadoce;component/Media/format_list_ordered.png",
                                 KeyCombination = new KeyCombo(Key.N, ModifierKeys.Control | ModifierKeys.Shift)
                             };
            yield return new PrependLines(selectionText, "* ")
                             {
                                 CommandText = "Unnumbered list",
                                 CommandIcon = "/Thawmadoce;component/Media/format_list_unordered.png",
                                 KeyCombination = new KeyCombo(Key.M, ModifierKeys.Control | ModifierKeys.Shift)
                             };
            yield return new ToCode(selectionText)
            {
                CommandText = "Code",
                CommandIcon = "/Thawmadoce;component/Media/code-icon.png",
                KeyCombination = new KeyCombo(Key.C, ModifierKeys.Control | ModifierKeys.Shift)
            };

            yield return new SelectionToLink(selectionText, _userInteraction)
            {
                CommandText = "Add Link",
                CommandIcon = "/Thawmadoce;component/Media/code-icon.png",
                KeyCombination = new KeyCombo(Key.L, ModifierKeys.Control | ModifierKeys.Shift)
            };
        }
    }
}