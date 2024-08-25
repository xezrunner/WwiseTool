using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace WwiseTool.Pages {
    public sealed partial class Startup : Page {
        public Startup() {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(GetStarted));
        }
    }
}
