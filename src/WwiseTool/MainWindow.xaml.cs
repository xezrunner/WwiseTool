using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI.ViewManagement;
using WwiseTool.Pages;

namespace WwiseTool {
    public sealed partial class MainWindow : Window {
        AppWindow appWindow;

        public MainWindow() {
            this.InitializeComponent();
            appWindow = this.AppWindow; // cache

            ExtendsContentIntoTitleBar = true;

            var uiSettings = new UISettings();
            uiSettings.ColorValuesChanged += UiSettings_ColorValuesChanged;
        }

        AppWindowTitleBar systemTitleBar;

        // NOTE: We have to use the 'root' control for its Loaded event to get and assign the "canonical" XamlRoot.
        // We should only load pages and such after this is assigned, as ContentDialogs need it!
        private void root_Loaded(object sender, RoutedEventArgs e) {
            // HACK: set XamlRoot regardless of activation state, since there's no Loaded event in WinUI3:
            // HACK: set XamlRoot globally, since this is the only window we'll have:
            App.GLOBAL_XamlRoot = root.XamlRoot;

            //TrySetMicaBackdrop(true);

            testFrame.Navigate(typeof(StartupPage));
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

            var systemTitleBar = App.GLOBAL_WindowInfo.appWindow.TitleBar;
            setTitlebarWindowControlColors();
        }

        private void UiSettings_ColorValuesChanged(UISettings sender, object args) {
            setTitlebarWindowControlColors();
        }

        void setTitlebarWindowControlColors() {
            if (systemTitleBar == null) systemTitleBar = App.GLOBAL_WindowInfo.appWindow.TitleBar;
            if (systemTitleBar == null) throw new Exception("No AppWindow TitleBar!");

            // Window control button colors:
            systemTitleBar.ButtonHoverBackgroundColor = (Application.Current.Resources["WindowControlHoverBackground"] as SolidColorBrush).Color;
            systemTitleBar.ButtonPressedBackgroundColor = (Application.Current.Resources["WindowControlPressedBackground"] as SolidColorBrush).Color;
        }
    }
}
