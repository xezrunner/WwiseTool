using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using WwiseTool.Backend;
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

            bool fatal = false;
            string s;
            switch (result.answer) {
                case DependencyManagerAnswer.OK: return;
                case DependencyManagerAnswer.DependencyPathError:
                    s = "There's a problem with the 'Dependencies' directory in the application directory.";
                    break;
                case DependencyManagerAnswer.MissingOptionalDependencies:
                    s = "Some optional dependencies are missing:\n";
                    goto missingDependencies;
                case DependencyManagerAnswer.MissingRequiredDependencies:
                    s = "The following required dependencies are missing:\n";
                    fatal = true;
                missingDependencies:
                    s += String.Join("; ", result.missingDependencies.Select(d => d.Name));
                    break;
                default: throw new Exception("Unexpected dependency manager answer!");
            }

            if (!String.IsNullOrEmpty(result.error)) s += $"\n\nError: {result.error}";
            // TODO: Automatically grab dependencies!
            if (fatal) s += "\n\nThe application cannot continue execution. Please provide the required dependencies.";

            if (!String.IsNullOrEmpty(s)) await DialogUtilities.ShowMessageAsync("📦 Dependency manager", s);

            if (fatal) Application.Current.Exit();
        }
    }
}
