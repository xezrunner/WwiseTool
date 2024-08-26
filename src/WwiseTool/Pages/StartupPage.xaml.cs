using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace WwiseTool.Pages {
    public sealed partial class StartupPage : Page {
        public StartupPage() {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            await Task.Delay(750); // TODO: temporary for ui-experiments branch!
            Frame.Navigate(typeof(GetStartedPage));
        }
    }
}
