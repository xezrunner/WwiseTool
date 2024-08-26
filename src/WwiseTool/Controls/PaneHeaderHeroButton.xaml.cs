using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WwiseTool.Controls {
    public sealed partial class PaneHeaderHeroButton : UserControl {
        public PaneHeaderHeroButton() {
            this.InitializeComponent();
        }

        public event EventHandler<RoutedEventArgs> Click;

        public string Glyph {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(PaneHeaderHeroButton), new PropertyMetadata("&#xE8B7;"));

        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(PaneHeaderHeroButton), new PropertyMetadata("Action title"));

        public string Description {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(PaneHeaderHeroButton), new PropertyMetadata("Action description goes here..."));

        private void Button_Click(object sender, RoutedEventArgs e) {
            Click?.Invoke(this, e);
        }
    }
}
