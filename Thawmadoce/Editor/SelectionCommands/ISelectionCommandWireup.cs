using System;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    internal interface ISelectionCommandWireup
    {
        void AfterModificationCallback(Action<TextContext> newSelection);
    }
}