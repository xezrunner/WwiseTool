using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace WwiseTool.Utilities {
    public static class DialogUtilities {
        public static IAsyncOperation<ContentDialogResult> ShowMessageAsync(string title, string message) {
            return ShowDialogWithContent(title, new TextBlock() { Text = message });
        }
        public static void ShowMessage(string title, string message) {
            _ = ShowDialogWithContent(title, new TextBlock() { Text = message });
        }

        public static IAsyncOperation<ContentDialogResult> ShowDialogWithContent(string title, UIElement content, 
            string primaryButtonText = null, string secondaryButtonText = null, string closeButtonText = "OK",
            ContentDialogButton defaultButton = ContentDialogButton.Primary) {
            ContentDialog dialog = new() {
                XamlRoot = App.GLOBAL_xamlRoot,
                Style = (Style)Application.Current.Resources["DefaultContentDialogStyle"],
                Title = title, Content = content,
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText,
                DefaultButton = defaultButton
            };

            return dialog.ShowAsync();
        }
    }
}
