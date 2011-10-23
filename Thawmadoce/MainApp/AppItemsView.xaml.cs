using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Thawmadoce.MainApp
{
    /// <summary>
    /// Interaction logic for AppItemsView.xaml
    /// </summary>
    public partial class AppItemsView : UserControl
    {
        public AppItemsView()
        {
            InitializeComponent();
            Loaded += HandleLoaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            // WHAT THE FUCK, JESUS!!

            var style = (Style)TryFindResource("PanelStyle");
            var sb = (Storyboard)TryFindResource("Open");
            sb = sb.Clone();
            ((DoubleAnimation)sb.Children[0]).From = AppItems.ActualHeight*-1;
            sb.Freeze();
            style.Triggers[0].EnterActions.Clear();
            var beginStoryboard = new BeginStoryboard { Storyboard = sb };
            style.Triggers[0].EnterActions.Add(beginStoryboard);

            sb = (Storyboard)TryFindResource("Close");
            sb = sb.Clone();
            ((DoubleAnimation)sb.Children[0]).To = AppItems.ActualHeight * -1;
            sb.Freeze();

            style.Triggers[0].ExitActions.Clear();
            style.Triggers[0].ExitActions.Add(new BeginStoryboard { Storyboard = sb });
            
            

        }
    }
}
