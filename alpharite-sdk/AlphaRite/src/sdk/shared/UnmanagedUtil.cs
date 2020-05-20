using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AlphaRite.sdk {
    public static class UnmanagedUtil {

        public enum MouseEvent {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_ABSOLUTE = 0x8000,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        
        [DllImport("user32.dll")]
        public static extern void keybd_event(int bVk, int bScan, int dwFlags, int dwExtraInfo);

        public static void keyPressed(params int[] codes) { 
            const int KEYEVENTF_KEYDOWN = 0x00;
            const int KEYEVENTF_KEYUP = 0x02;

            foreach (var keyCode in codes)
                keybd_event(keyCode, 0x0, KEYEVENTF_KEYDOWN, 0x0);
            foreach (var keyCode in codes.Reverse())
                keybd_event(keyCode, 0x0, KEYEVENTF_KEYUP, 0x0);
        }
        
        public static void keyDown(params int[] codes) {
            const int KEYEVENTF_KEYDOWN = 0x00;

            foreach (var keyCode in codes)
                keybd_event(keyCode, 0x0, KEYEVENTF_KEYDOWN, 0x0);
        }
        
        public static void keyUp(params int[] codes) {
            const int KEYEVENTF_KEYUP = 0x02;

            foreach (var keyCode in codes)
                keybd_event(keyCode, 0x0, KEYEVENTF_KEYUP, 0x0);
        }

        public static void mousePressed(int mouseCode) {
            var pos = Input.mousePosition;
            var x = (int)(pos.x / Screen.width * 65535f);
            var y = (int)(pos.y / Screen.height * 65535f);
            
            mouse_event((int) MouseEvent.MOUSEEVENTF_XDOWN, x, y, mouseCode, 0);
            mouse_event((int) MouseEvent.MOUSEEVENTF_XUP, x, y, mouseCode, 0);
        }

        public static void mouseWheelDown() {
            mouse_event((int) MouseEvent.MOUSEEVENTF_WHEEL, 0, 0, -120, 0);
        }
        
        public static void mouseWheelUp() {
            mouse_event((int) MouseEvent.MOUSEEVENTF_WHEEL, 0, 0, 120, 0);
        }
    }
}