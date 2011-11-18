namespace Thawmadoce.MainApp
{
    public class NewDisplayNameUiMsg
    {
        public string NewTitle { get; private set; }
        public bool Append { get; private set; }

        public bool IsTitleReset { get { return string.IsNullOrWhiteSpace(NewTitle); } }

        public NewDisplayNameUiMsg() : this(null, false)
        {
            
        }

        public NewDisplayNameUiMsg(string newTitle, bool append = true)
        {
            NewTitle = newTitle;
            Append = append;
        }
    }
}