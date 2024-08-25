using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WwiseTool {
    public partial class App : Application {
        public App() {
            this.InitializeComponent();
        }

        // HACK: I don't care about setting the XamlRoot with each dialog. This should be set by the MainWindow.
        public static XamlRoot GLOBAL_XamlRoot;

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
    }
}
