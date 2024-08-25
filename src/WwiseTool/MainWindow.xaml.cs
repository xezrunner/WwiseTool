using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using WwiseTool.Pages;

namespace WwiseTool {
    public sealed partial class MainWindow : Window {
        AppWindow appWindow;

        public MainWindow() {
            this.InitializeComponent();
            appWindow = this.AppWindow; // cache

            ExtendsContentIntoTitleBar = true;
        }

        // NOTE: We have to use the 'root' control for its Loaded event to get and assign the "canonical" XamlRoot.
        // We should only load pages and such after this is assigned, as ContentDialogs need it!
        private void root_Loaded(object sender, RoutedEventArgs e) {
            // HACK: set XamlRoot regardless of activation state, since there's no Loaded event in WinUI3:
            // HACK: set XamlRoot globally, since this is the only window we'll have:
            App.GLOBAL_xamlRoot = root.XamlRoot;

            //TrySetMicaBackdrop(true);

            testFrame.Navigate(typeof(Startup));
        }

        // From the WinUI3 Gallery app:
        bool TrySetMicaBackdrop(bool useMicaAlt) {
            if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported()) {
                Microsoft.UI.Xaml.Media.MicaBackdrop micaBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop();
                micaBackdrop.Kind = useMicaAlt ? Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt : Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;
                this.SystemBackdrop = micaBackdrop;

                return true; // Succeeded.
            }

            return false; // Mica is not supported on this system.
        }

        private void titlebar_Loaded(object sender, RoutedEventArgs e) {
            // App title label
            // TODO: Set this up properly later:
            //titlebarAppTitle.Text = AppInfo.Current.DisplayInfo.DisplayName;
            titlebarAppTitle.Text = "Wwise Tool";

            var titlebarXamlRoot = titlebar.XamlRoot;

            titlebarLeftPadding.Width  = new GridLength(appWindow.TitleBar.LeftInset  / titlebarXamlRoot.RasterizationScale);
            titlebarRightPadding.Width = new GridLength(appWindow.TitleBar.RightInset / titlebarXamlRoot.RasterizationScale);

            SetTitleBar(titlebar);
        }
    }
}
