namespace Thawmadoce.Editor
{
    public class NewMarkdownTaskMsg
    {
        public string MarkdownText { get; private set; }

        public NewMarkdownTaskMsg(string markdownText)
        {
            MarkdownText = markdownText;
        }
    }
}