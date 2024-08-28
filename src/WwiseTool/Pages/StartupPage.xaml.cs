using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using WwiseTool.Backend;
using WwiseTool.Dialogs;
using WwiseTool.Utilities;

namespace WwiseTool.Pages {
    public sealed partial class StartupPage : Page {
        public StartupPage() {
            this.InitializeComponent();
        }

        DependencyManager dependencyManager;

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            await Task.Delay(750); // TODO: temporary for ui-experiments branch!

            await STARTUP_Dependencies();

            Frame.Navigate(typeof(GetStartedPage));
        }

        async Task STARTUP_Dependencies() {
            dependencyManager = new(WwiseToolDependencies.Dependencies);
            var result = dependencyManager.CheckDependencyStatus();

            var dialogContent = new DependenciesDialogContent(result);
            await DialogUtilities.ShowDialogWithContentAsync("Dependencies", dialogContent, closeButtonText: null);
        }
    }
}
