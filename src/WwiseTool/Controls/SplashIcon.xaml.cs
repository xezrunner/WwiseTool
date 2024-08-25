using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WwiseTool.Controls {
    public sealed partial class SplashIcon : UserControl {
        public SplashIcon() {
            this.InitializeComponent();
        }

        public bool ShowDebugBounds {
            get { return (bool)GetValue(ShowDebugBoundsProperty); }
            set { SetValue(ShowDebugBoundsProperty, value); }
        }

        public static readonly DependencyProperty ShowDebugBoundsProperty =
            DependencyProperty.Register("ShowDebugBounds", typeof(bool), typeof(SplashIcon), new PropertyMetadata(false));
    }
}
