using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace WwiseTool.Utilities {
    public struct WINUI_WindowInfo {
        public nint hWnd;
        public WindowId windowId;
        public AppWindow appWindow;
    }

    public static class WindowUtilities {
        public static WINUI_WindowInfo GetInfoForWindow(Window window) {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            return new WINUI_WindowInfo {
                hWnd = hWnd,
                windowId = windowId,
                appWindow = appWindow
            };
        }

        public static void CenterWindow(WINUI_WindowInfo info) {
            // Based on https://stackoverflow.com/posts/71730765/revisions
            var displayArea = DisplayArea.GetFromWindowId(info.windowId, DisplayAreaFallback.Nearest);
            if (displayArea == null) return;

            var CenteredPosition = info.appWindow.Position;
            CenteredPosition.X = ((displayArea.WorkArea.Width  - info.appWindow.Size.Width)  / 2);
            CenteredPosition.Y = ((displayArea.WorkArea.Height - info.appWindow.Size.Height) / 2);
            info.appWindow.Move(CenteredPosition);

        }

        public static void CenterWindow(Window window) {
            var info = GetInfoForWindow(window);
            CenterWindow(info);
        }
    }
}
