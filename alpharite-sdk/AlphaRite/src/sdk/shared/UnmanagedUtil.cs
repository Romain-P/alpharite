using System.Runtime.InteropServices;

namespace AlphaRite.sdk {
    public static class UnmanagedUtil {

        public enum MouseEvent {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
    }
}