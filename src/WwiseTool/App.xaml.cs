using Microsoft.UI.Xaml;
using WwiseTool.Utilities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WwiseTool {
    public partial class App : Application {
        public App() {
            this.InitializeComponent();
        }

        public static WINUI_WindowInfo GLOBAL_WindowInfo;

        // HACK: I don't care about setting the XamlRoot with each dialog. This will be set by the MainWindow.
        public static XamlRoot GLOBAL_XamlRoot;

        protected override void OnLaunched(LaunchActivatedEventArgs args) {
            var mainWindow = new MainWindow();
            GLOBAL_WindowInfo = WindowUtilities.GetInfoForWindow(mainWindow);

            WindowUtilities.CenterWindow(GLOBAL_WindowInfo);

            mainWindow.Activate();
        }
    }
}
