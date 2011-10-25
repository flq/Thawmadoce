using System.Windows;
using Thawmadoce.Extensibility;

namespace Thawmadoce.MainApp
{
    public class FileOpsSaga : ISaga
    {
        public void Handle(OpenFileUiMsg msg)
        {
            MessageBox.Show("Open file!");
        }

        public void Handle(SaveFileUiMsg msg)
        {
            MessageBox.Show("Save file!");
        }
    }
}