using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GuessWho.View {
    public partial class MainWindow : Window {

        #region Maximization fix

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            switch (msg) {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam) {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero) {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int X;
            public int Y;

            public POINT(int x, int y) {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;
            public static readonly RECT Empty;

            public int Width {
                get {
                    return Math.Abs(Right - Left);
                }
            }

            public int Height {
                get {
                    return Bottom - Top;
                }
            }

            public RECT(int left, int top, int right, int bottom) {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(RECT other) {
                Left = other.Left;
                Top = other.Top;
                Right = other.Right;
                Bottom = other.Bottom;
            }

            public bool IsEmpty {
                get {
                    return Left >= Right || Top >= Bottom;
                }
            }

            public bool Equals(RECT other) {
                return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
            }

            public override bool Equals(object obj) {
                return obj is RECT other && Equals(other);
            }

            public override int GetHashCode() {
                unchecked {
                    int hashCode = Left;
                    hashCode = (hashCode * 397) ^ Top;
                    hashCode = (hashCode * 397) ^ Right;
                    hashCode = (hashCode * 397) ^ Bottom;
                    return hashCode;
                }
            }

            public static bool operator ==(RECT rect1, RECT rect2) {
                return (rect1.Left == rect2.Left && rect1.Top == rect2.Top && rect1.Right == rect2.Right && rect1.Bottom == rect2.Bottom);
            }

            public static bool operator !=(RECT rect1, RECT rect2) {
                return !(rect1 == rect2);
            }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion

        private HwndSource WindowHandle { get; set; }

        public MainWindow() {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e) {
            WindowHandle?.Dispose();
        }

        private void Window_SourceInitialized(object sender, EventArgs e) {
            WindowHandle = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            WindowHandle?.AddHook(WindowProc);
        }

        private void CloseWindowButton_OnClick(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Normal;
            Close();
        }

        private void MaximizeWindowButton_OnClick(object sender, RoutedEventArgs e) {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeWindowButton_OnClick(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }
    }
}