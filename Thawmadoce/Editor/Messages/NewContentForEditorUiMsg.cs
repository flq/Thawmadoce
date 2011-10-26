using System;

namespace Thawmadoce.Editor
{
    public class NewContentForEditorUiMsg
    {
        public NewContentForEditorUiMsg(string fileContents)
        {
            Text = fileContents;
        }

        public NewContentForEditorUiMsg() : this("")
        {
            
        }

        public string Text { get; private set; }
    }
}