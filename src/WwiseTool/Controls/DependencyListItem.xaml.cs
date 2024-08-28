using Microsoft.UI.Xaml.Controls;
using WwiseTool.Backend;

namespace WwiseTool.Controls {
    public sealed partial class DependencyListItem : UserControl {
        public DependencyListItem() {
            this.InitializeComponent();
        }

        public DependencyListItem(Dependency dependency) {
            this.InitializeComponent();
            this.Dependency = dependency;
        }

        public Dependency Dependency;
    }
}
