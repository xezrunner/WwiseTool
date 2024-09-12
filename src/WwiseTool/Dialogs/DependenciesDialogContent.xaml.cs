using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WwiseTool.Backend;
using WwiseTool.Controls;

namespace WwiseTool.Dialogs {
    public sealed partial class DependenciesDialogContent : UserControl, INotifyPropertyChanged {
        private DependenciesDialogContent() {
            this.InitializeComponent();
        }

        DependencyManagerResult dependencyManagerResult;

        ContentDialog parentDialog;

        public DependenciesDialogContent(DependencyManagerResult dependencyManagerResult) {
            this.InitializeComponent();
            this.dependencyManagerResult = dependencyManagerResult;
        }

        private void root_Loaded(object sender, RoutedEventArgs e) {
            parentDialog = (ContentDialog)Tag;
            // TODO: interface/(abstract) class for such dialog content?
            if (parentDialog == null) throw new Exception("No dialog was attached to the Tag of this control. This is unexpected!");

            RefreshContentDialogProperties();
        }

        bool isDebugEnabled = false;

        bool isError                            { get { return dependencyManagerResult.answer != DependencyManagerAnswer.OK; } }
        bool areRequiredDependenciesMissing     { get { return dependencyManagerResult.answer.HasFlag(DependencyManagerAnswer.MissingRequiredDependencies); } }
        bool areOptionalDependenciesMissing     { get { return dependencyManagerResult.answer.HasFlag(DependencyManagerAnswer.MissingOptionalDependencies); } }
        bool areOnlyOptionalDependenciesMissing { get { return !areRequiredDependenciesMissing && dependencyManagerResult.answer.HasFlag(DependencyManagerAnswer.MissingOptionalDependencies); } }
        bool isHardError                        { get { return (isError && !areRequiredDependenciesMissing && !areOptionalDependenciesMissing); } }

        string[] DependencyManagerAnswerNames = Enum.GetNames(typeof(DependencyManagerAnswer))
            .Append("MissingRequiredDependencies, MissingOptionalDependencies")
            .ToArray();

        private void debugAnswerComboBox_Loaded(object sender, RoutedEventArgs e) {
            debugAnswerComboBox.SelectedItem = dependencyManagerResult.answer.ToString();
        }

        private void debugAnswerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            dependencyManagerResult.answer = (DependencyManagerAnswer)Enum.Parse(typeof(DependencyManagerAnswer), (string)debugAnswerComboBox.SelectedItem);
            NotifyAllPropertiesChanged();
        }

        // Dependencies:
        private void dependencyListStackPanel_Loaded(object sender, RoutedEventArgs e) {
            if (dependencyManagerResult.missingDependencies == null) return;

            foreach (var dependency in dependencyManagerResult.missingDependencies) {
                DependencyListItem item = new(dependency);
                dependencyListStackPanel.Children.Add(item);
            }
        }

        void RefreshContentDialogProperties() {
            parentDialog.Title = isHardError ? null : "Dependencies";
            parentDialog.CloseButtonText = (!isError || !areRequiredDependenciesMissing ? "OK" : null);
        }

        // Property change notifications:
        public event PropertyChangedEventHandler PropertyChanged;

        // NOTE: [CallerMemberName] will insert the method name you call this method from.
        void NotifyPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        void NotifyAllPropertiesChanged() {
            NotifyPropertyChanged(null);

            // HACK: update dialog layout:
            RefreshContentDialogProperties();
        }
    }
}
