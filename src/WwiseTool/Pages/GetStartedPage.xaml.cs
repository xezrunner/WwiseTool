using Microsoft.UI.Xaml.Controls;
using WwiseTool.Utilities;

namespace WwiseTool.Pages {
    public sealed partial class GetStartedPage : Page {
        public GetStartedPage() {
            this.InitializeComponent();
        }

        private void ActionButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
            //DialogUtilities.ShowMessage("Get started", "Action clicked!");
            Frame.Navigate(typeof(ProjectPage));
        }
    }
}
