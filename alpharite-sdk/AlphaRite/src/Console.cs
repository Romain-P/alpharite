using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

// --------------------------------------------------
// UnityInjector - ConsoleWindow.cs
// Copyright (c) Usagirei 2015 - 2015
// Ported from BepInEx project
// ----------------
namespace AlphaRite {
    internal class Alpharite {
        public static void println(string msg, params object[] args) {
            if (!ConsoleWindow.IsAttached) {
                ConsoleWindow.Attach();
                ConsoleWindow.Title = "AlphaRite Logs";
            }

            ConsoleWindow.StandardOut.WriteLine("[INFO]:\t"+msg, args);
            ConsoleWindow.StandardOut.Flush();
        }
    }

    internal class ConsoleWindow {
        private static IntPtr _cOut;
        private static IntPtr _oOut;
        public static bool IsAttached { get; private set; }

        public static TextWriter StandardOut { get; private set; }

        public static string Title {
            set {
                if (!IsAttached)
                    return;

                if (value == null) throw new ArgumentNullException(nameof(value));

                if (value.Length > 24500) throw new InvalidOperationException("Console title too long");

                if (!SetConsoleTitle(value)) throw new InvalidOperationException("Console title invalid");
            }
        }

        public static void Attach() {
            if (IsAttached)
                return;

            if (_oOut == IntPtr.Zero)
                _oOut = GetStdHandle(-11);

            // Store Current Window
            var currWnd = GetForegroundWindow();

            //Check for existing console before allocating
            if (GetConsoleWindow() == IntPtr.Zero)
                if (!AllocConsole())
                    throw new Exception("AllocConsole() failed");

            // Restore Foreground
            SetForegroundWindow(currWnd);

            _cOut = CreateFile("CONOUT$", 0x80000000 | 0x40000000, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
            Kon.conOut = _cOut;

            if (!SetStdHandle(-11, _cOut))
                throw new Exception("SetStdHandle() failed");
            Init();

            IsAttached = true;
        }

        public static void Detach() {
            if (!IsAttached)
                return;

            if (!CloseHandle(_cOut))
                throw new Exception("CloseHandle() failed");
            _cOut = IntPtr.Zero;
            if (!FreeConsole())
                throw new Exception("FreeConsole() failed");
            if (!SetStdHandle(-11, _oOut))
                throw new Exception("SetStdHandle() failed");
            Init();

            IsAttached = false;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreateFile(
            string fileName,
            uint desiredAccess,
            int shareMode,
            IntPtr securityAttributes,
            int creationDisposition,
            int flagsAndAttributes,
            IntPtr templateFile);

        [DllImport("kernel32.dll", SetLastError = false)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        private static void Init() {
            var stdOut = Console.OpenStandardOutput();
            StandardOut = new StreamWriter(stdOut, Encoding.Default) {
                AutoFlush = true
            };

            Console.SetOut(StandardOut);
            Console.SetError(StandardOut);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetStdHandle(int nStdHandle, IntPtr hConsoleOutput);

        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetConsoleTitle(string title);
    }

    internal class Kon {
        #region pinvoke

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput,
            out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, short attributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        #endregion

        #region Types

        private struct CONSOLE_SCREEN_BUFFER_INFO {
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal short wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
        }

        private struct COORD {
            internal short X;
            internal short Y;
        }

        private struct SMALL_RECT {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }

        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        #endregion

        #region Private

        private static short ConsoleColorToColorAttribute(short color, bool isBackground) {
            if ((color & -16) != 0)
                throw new ArgumentException("Arg_InvalidConsoleColor");
            if (isBackground)
                color <<= 4;
            return color;
        }

        private static CONSOLE_SCREEN_BUFFER_INFO GetBufferInfo(bool throwOnNoConsole, out bool succeeded) {
            succeeded = false;
            if (!(conOut == INVALID_HANDLE_VALUE))
                try {
                    // FIXME: Windows console shouldn't be used in other OSs in the first place
                    CONSOLE_SCREEN_BUFFER_INFO console_SCREEN_BUFFER_INFO;
                    if (!GetConsoleScreenBufferInfo(conOut, out console_SCREEN_BUFFER_INFO)) {
                        var consoleScreenBufferInfo =
                            GetConsoleScreenBufferInfo(GetStdHandle(-12), out console_SCREEN_BUFFER_INFO);
                        if (!consoleScreenBufferInfo)
                            consoleScreenBufferInfo =
                                GetConsoleScreenBufferInfo(GetStdHandle(-10), out console_SCREEN_BUFFER_INFO);

                        if (!consoleScreenBufferInfo)
                            if (Marshal.GetLastWin32Error() == 6 && !throwOnNoConsole)
                                return default;
                    }

                    succeeded = true;
                    return console_SCREEN_BUFFER_INFO;
                }
                catch (EntryPointNotFoundException) {
                    // Fails under unsupported OSes
                }

            if (!throwOnNoConsole)
                return default;
            throw new Exception("IO.IO_NoConsole");
        }

        private static void SetConsoleColor(bool isBackground, ConsoleColor c) {
            new UIPermission(UIPermissionWindow.SafeTopLevelWindows).Demand();
            var color = ConsoleColorToColorAttribute((short) c, isBackground);
            bool flag;
            var bufferInfo = GetBufferInfo(false, out flag);
            if (!flag)
                return;
            var num = bufferInfo.wAttributes;
            num &= (short) (isBackground ? -241 : -16);
            num = (short) ((ushort) num | (ushort) color);
            SetConsoleTextAttribute(conOut, num);
        }

        private static ConsoleColor GetConsoleColor(bool isBackground) {
            bool flag;
            var bufferInfo = GetBufferInfo(false, out flag);
            if (!flag)
                return isBackground ? ConsoleColor.Black : ConsoleColor.Gray;
            return ColorAttributeToConsoleColor((short) (bufferInfo.wAttributes & 240));
        }

        private static ConsoleColor ColorAttributeToConsoleColor(short c) {
            if ((short) (c & 255) != 0)
                c >>= 4;
            return (ConsoleColor) c;
        }

        internal static IntPtr conOut = IntPtr.Zero;

        #endregion

        #region Public

        public static void ResetConsoleColor() {
            SetConsoleColor(true, ConsoleColor.Black);
            SetConsoleColor(false, ConsoleColor.Gray);
        }

        public static ConsoleColor ForegroundColor {
            get => GetConsoleColor(false);
            set => SetConsoleColor(false, value);
        }

        public static ConsoleColor BackgroundColor {
            get => GetConsoleColor(true);
            set => SetConsoleColor(true, value);
        }

        #endregion
    }

    internal static class SafeConsole {
        private static GetColorDelegate _getBackgroundColor;
        private static GetColorDelegate _getForegroundColor;
        private static SetColorDelegate _setBackgroundColor;
        private static SetColorDelegate _setForegroundColor;

        static SafeConsole() {
            var tConsole = typeof(Console);
            InitColors(tConsole);
        }

        public static ConsoleColor BackgroundColor {
            get => _getBackgroundColor();
            set => _setBackgroundColor(value);
        }

        public static ConsoleColor ForegroundColor {
            get => _getForegroundColor();
            set => _setForegroundColor(value);
        }

        private static void InitColors(Type tConsole) {
            const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.Static;

            var sfc = tConsole.GetMethod("set_ForegroundColor", BINDING_FLAGS);
            var sbc = tConsole.GetMethod("set_BackgroundColor", BINDING_FLAGS);
            var gfc = tConsole.GetMethod("get_ForegroundColor", BINDING_FLAGS);
            var gbc = tConsole.GetMethod("get_BackgroundColor", BINDING_FLAGS);

            _setForegroundColor = sfc != null
                ? (SetColorDelegate) Delegate.CreateDelegate(typeof(SetColorDelegate), sfc)
                : value => { };

            _setBackgroundColor = sbc != null
                ? (SetColorDelegate) Delegate.CreateDelegate(typeof(SetColorDelegate), sbc)
                : value => { };

            _getForegroundColor = gfc != null
                ? (GetColorDelegate) Delegate.CreateDelegate(typeof(GetColorDelegate), gfc)
                : () => ConsoleColor.Gray;

            _getBackgroundColor = gbc != null
                ? (GetColorDelegate) Delegate.CreateDelegate(typeof(GetColorDelegate), gbc)
                : () => ConsoleColor.Black;
        }

        private delegate ConsoleColor GetColorDelegate();

        private delegate void SetColorDelegate(ConsoleColor value);
    }
}