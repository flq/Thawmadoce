namespace Thawmadoce.Editor
{
    public class AppendTextUiMsg
    {
        public string Text { get; private set; }

        public AppendTextUiMsg(string text)
        {
            Text = text;
        }
    }
}