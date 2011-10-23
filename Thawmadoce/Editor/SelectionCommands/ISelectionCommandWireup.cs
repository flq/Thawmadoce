using System;

namespace Thawmadoce.Editor.SelectionCommands
{
    internal interface ISelectionCommandWireup
    {
        void AfterModificationCallback(Action<string> newSelection);
    }
}