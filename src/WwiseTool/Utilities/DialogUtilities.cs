using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace WwiseTool.Utilities {
    public static class DialogUtilities {
        public static IAsyncOperation<ContentDialogResult> ShowMessageAsync(string title, string message) {
            return ShowDialogWithContentAsync(title, 
                new TextBlock() { Text = message, TextWrapping = TextWrapping.Wrap }
            );
        }
        public static void ShowMessage(string title, string message) {
            _ = ShowMessageAsync(title, message);
        }

        public static IAsyncOperation<ContentDialogResult> ShowDialogWithContentAsync(string title, FrameworkElement content, 
            string primaryButtonText = null, string secondaryButtonText = null, string closeButtonText = "OK",
            ContentDialogButton defaultButton = ContentDialogButton.Primary) {
            ContentDialog dialog = new() {
                XamlRoot = App.GLOBAL_XamlRoot,
                Style = (Style)Application.Current.Resources["DefaultContentDialogStyle"],

                Title = title, Content = content,

                PrimaryButtonText = primaryButtonText, SecondaryButtonText = secondaryButtonText, CloseButtonText = closeButtonText,
                DefaultButton = defaultButton
            };
            content.Tag = dialog; // Attach dialog to the content, so that they can access it.

            return dialog.ShowAsync();
        }
    }
}
