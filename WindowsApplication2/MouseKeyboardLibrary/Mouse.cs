//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;
//using System.Runtime.InteropServices;
//using System.Windows.Forms;
//using System.Reflection;


//namespace MouseKeyboardLibrary
//{


//    /// <summary>
//    /// Operations that simulate mouse events
//    /// </summary>
//    public static class Mouse
//    {

//        [DllImport("user32.dll")]
//        static extern int ShowCursor(bool show);

//        [DllImport("user32.dll")]
//        static extern void mouse_event(int flags, int dX, int dY, int buttons, int extraInfo);

//        const int MOUSEEVENTF_MOVE = 0x1;
//        const int MOUSEEVENTF_LEFTDOWN = 0x2;
//        const int MOUSEEVENTF_LEFTUP = 0x4;
//        const int MOUSEEVENTF_RIGHTDOWN = 0x8;
//        const int MOUSEEVENTF_RIGHTUP = 0x10;
//        const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
//        const int MOUSEEVENTF_MIDDLEUP = 0x40;
//        const int MOUSEEVENTF_WHEEL = 0x800;
//        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

//        public static event MouseEventHandler MouseDown;
//        public static event MouseEventHandler MouseUp;
//        public static event MouseEventHandler MouseMove;
//        public static event MouseEventHandler MouseWheel;
//        public static event EventHandler Click;
//        public static event EventHandler DoubleClick;

//        private static APIMouseHook hook;

//        static Mouse()
//        {
//            hook = new APIMouseHook();
//        }

//        /// <summary>
//        /// Gets or sets a structure that represents both X and Y mouse coordinates
//        /// </summary>
//        public static MousePoint Position
//        {
//            get
//            {
//                return new MousePoint(Cursor.Position);
//            }
//            set
//            {
//                Cursor.Position = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets only the mouse's x coordinate
//        /// </summary>
//        public static int X
//        {
//            get
//            {
//                return Cursor.Position.X;
//            }
//            set
//            {
//                Cursor.Position = new Point(value, Y);
//            }
//        }

//        /// <summary>
//        /// Gets or sets only the mouse's y coordinate
//        /// </summary>
//        public static int Y
//        {
//            get
//            {
//                return Cursor.Position.Y;
//            }
//            set
//            {
//                Cursor.Position = new Point(X, value);
//            }
//        }

//        /// <summary>
//        /// Press a mouse button down
//        /// </summary>
//        /// <param name="button"></param>
//        public static void SimulateMouseDown(MouseButton button)
//        {
//            mouse_event(((int)button), 0, 0, 0, 0);
//        }

//        /// <summary>
//        /// Let a mouse button up
//        /// </summary>
//        /// <param name="button"></param>
//        public static void SimulateMouseUp(MouseButton button)
//        {
//            mouse_event(((int)button) * 2, 0, 0, 0, 0);
//        }

//        /// <summary>
//        /// Click a mouse button (down then up)
//        /// </summary>
//        /// <param name="button"></param>
//        public static void SimulateClick(MouseButton button)
//        {
//            SimulateMouseDown(button);
//            SimulateMouseUp(button);
//        }

//        /// <summary>
//        /// Double click a mouse button (down then up twice)
//        /// </summary>
//        /// <param name="button"></param>
//        public static void SimulateDoubleClick(MouseButton button)
//        {
//            SimulateClick(button);
//            SimulateClick(button);
//        }

//        /// <summary>
//        /// Show a hidden current on currently application
//        /// </summary>
//        public static void Show()
//        {
//            ShowCursor(true);
//        }

//        /// <summary>
//        /// Hide mouse cursor only on current application's forms
//        /// </summary>
//        public static void Hide()
//        {
//            ShowCursor(false);
//        }

//        #region API and Hook code

//        private class APIMouseHook
//        {

//            private enum MouseEventType
//            {
//                None,
//                MouseDown,
//                MouseUp,
//                DoubleClick,
//                MouseWheel,
//                MouseMove
//            }

//            private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

//            private int hHook = 0;

//            public APIMouseHook()
//            {

//                // Start Hook
//                hHook = SetWindowsHookEx
//                    (
//                        WH_MOUSE_LL,
//                        new HookProc(MouseHookProc),
//                        Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
//                        0
//                    );
                 
//            }

//            ~APIMouseHook()
//            {
                
//                // Stop Hook
//                if (hHook != 0)
//                {
//                    UnhookWindowsHookEx(hHook);
//                }

//            }

//            private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
//            {

//                if (nCode > -1 && (MouseDown != null || MouseUp != null || MouseMove != null))
//                {

//                    MouseLLHookStruct mouseHookStruct = 
//                        (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

//                    MouseButtons button = GetButton(wParam);
//                    MouseEventType eventType = GetEventType(wParam);
                    
//                    MouseEventArgs e = new MouseEventArgs(
//                        button,
//                        (eventType == MouseEventType.DoubleClick ? 2 : 1),
//                        mouseHookStruct.pt.x,
//                        mouseHookStruct.pt.y,
//                        (eventType == MouseEventType.MouseWheel ? (short)((mouseHookStruct.mouseData >> 16) & 0xffff) : 0));

//                    // Prevent multiple Right Click events (this probably happens for popup menus)
//                    if (button == MouseButtons.Right && mouseHookStruct.flags != 0)
//                    {
//                        eventType = MouseEventType.None;
//                    }

//                    switch (eventType)
//                    {
//                        case MouseEventType.MouseDown:
//                            if (MouseDown != null)
//                            {
//                                MouseDown(this, e);
//                            }
//                            break;
//                        case MouseEventType.MouseUp:
//                            if (Click != null)
//                            {
//                                Click(this, new EventArgs());
//                            }
//                            if (MouseUp != null)
//                            {
//                                MouseUp(this, e);
//                            }
//                            break;
//                        case MouseEventType.DoubleClick:
//                            if (DoubleClick != null)
//                            {
//                                DoubleClick(this, new EventArgs());
//                            }
//                            break;
//                        case MouseEventType.MouseWheel:
//                            if (MouseWheel != null)
//                            {
//                                MouseWheel(this, e);
//                            }
//                            break;
//                        case MouseEventType.MouseMove:
//                            if (MouseMove != null)
//                            {
//                                MouseMove(this, e);
//                            }
//                            break;
//                        default:
//                            break;
//                    }

//                }

//                return CallNextHookEx(hHook, nCode, wParam, lParam);

//            }

//            private MouseButtons GetButton(Int32 wParam)
//            {

//                switch (wParam)
//                {

//                    case WM_LBUTTONDOWN:
//                    case WM_LBUTTONUP:
//                    case WM_LBUTTONDBLCLK:
//                        return MouseButtons.Left;
//                    case WM_RBUTTONDOWN:
//                    case WM_RBUTTONUP:
//                    case WM_RBUTTONDBLCLK:
//                        return MouseButtons.Right;
//                    case WM_MBUTTONDOWN:
//                    case WM_MBUTTONUP:
//                    case WM_MBUTTONDBLCLK:
//                        return MouseButtons.Middle;
//                    default:
//                        return MouseButtons.None;

//                }

//            }

//            private MouseEventType GetEventType(Int32 wParam)
//            {

//                switch (wParam)
//                {

//                    case WM_LBUTTONDOWN:
//                    case WM_RBUTTONDOWN:
//                    case WM_MBUTTONDOWN:
//                        return MouseEventType.MouseDown;
//                    case WM_LBUTTONUP:
//                    case WM_RBUTTONUP:
//                    case WM_MBUTTONUP:
//                        return MouseEventType.MouseUp;
//                    case WM_LBUTTONDBLCLK:
//                    case WM_RBUTTONDBLCLK:
//                    case WM_MBUTTONDBLCLK:
//                        return MouseEventType.DoubleClick;
//                    case WM_MOUSEWHEEL:
//                        return MouseEventType.MouseWheel;
//                    case WM_MOUSEMOVE:
//                        return MouseEventType.MouseMove;
//                    default:
//                        return MouseEventType.None;

//                }
//            }

//        }

//        #endregion

//    }

//}
