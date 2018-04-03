//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Runtime.InteropServices;
//using System.Windows.Forms;
//using System.Reflection;

//namespace MouseKeyboardLibrary
//{




//    public static class Keyboard
//    {


//        #region API and Hook code

//        private class APIKeyboardHook
//        {

//            [StructLayout(LayoutKind.Sequential)]
//            private class POINT
//            {
//                public int x;
//                public int y;
//            }

//            [StructLayout(LayoutKind.Sequential)]
//            private class MouseHookStruct
//            {
//                public POINT pt;
//                public int hwnd;
//                public int wHitTestCode;
//                public int dwExtraInfo;
//            }

//            [StructLayout(LayoutKind.Sequential)]
//            private class MouseLLHookStruct
//            {
//                public POINT pt;
//                public int mouseData;
//                public int flags;
//                public int time;
//                public int dwExtraInfo;
//            }

//            [StructLayout(LayoutKind.Sequential)]
//            private class KeyboardHookStruct
//            {
//                public int vkCode;
//                public int scanCode;
//                public int flags;
//                public int time;
//                public int dwExtraInfo;
//            }

//            [DllImport("user32.dll", CharSet = CharSet.Auto,
//               CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//            private static extern int SetWindowsHookEx(
//                int idHook,
//                HookProc lpfn,
//                IntPtr hMod,
//                int dwThreadId);

//            [DllImport("user32.dll", CharSet = CharSet.Auto,
//                CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//            private static extern int UnhookWindowsHookEx(int idHook);


//            [DllImport("user32.dll", CharSet = CharSet.Auto,
//                 CallingConvention = CallingConvention.StdCall)]
//            private static extern int CallNextHookEx(
//                int idHook,
//                int nCode,
//                int wParam,
//                IntPtr lParam);

//            [DllImport("user32")]
//            private static extern int ToAscii(
//                int uVirtKey,
//                int uScanCode,
//                byte[] lpbKeyState,
//                byte[] lpwTransKey,
//                int fuState);

//            [DllImport("user32")]
//            private static extern int GetKeyboardState(byte[] pbKeyState);

//            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
//            private static extern short GetKeyState(int vKey);

//            private const int WH_MOUSE_LL = 14;
//            private const int WH_KEYBOARD_LL = 13;
//            private const int WH_MOUSE = 7;
//            private const int WH_KEYBOARD = 2;
//            private const int WM_MOUSEMOVE = 0x200;
//            private const int WM_LBUTTONDOWN = 0x201;
//            private const int WM_RBUTTONDOWN = 0x204;
//            private const int WM_MBUTTONDOWN = 0x207;
//            private const int WM_LBUTTONUP = 0x202;
//            private const int WM_RBUTTONUP = 0x205;
//            private const int WM_MBUTTONUP = 0x208;
//            private const int WM_LBUTTONDBLCLK = 0x203;
//            private const int WM_RBUTTONDBLCLK = 0x206;
//            private const int WM_MBUTTONDBLCLK = 0x209;
//            private const int WM_MOUSEWHEEL = 0x020A;
//            private const int WM_KEYDOWN = 0x100;
//            private const int WM_KEYUP = 0x101;
//            private const int WM_SYSKEYDOWN = 0x104;
//            private const int WM_SYSKEYUP = 0x105;

//            private const byte VK_SHIFT = 0x10;
//            private const byte VK_CAPITAL = 0x14;
//            private const byte VK_NUMLOCK = 0x90;

//            private const byte VK_LSHIFT = 0xA0;
//            private const byte VK_RSHIFT = 0xA1;
//            private const byte VK_LCONTROL = 0xA2;
//            private const byte VK_RCONTROL = 0x3;
//            private const byte VK_LALT = 0xA4;
//            private const byte VK_RALT = 0xA5;

//            private const byte LLKHF_ALTDOWN = 0x20;

//            private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

//            private int hHook = 0;

//            public APIKeyboardHook()
//            {

//                // Start Hook
//                hHook = SetWindowsHookEx
//                    (
//                        WH_KEYBOARD_LL,
//                        new HookProc(KeyboardHookProc),
//                        Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
//                        0
//                    );
                 
//            }

//            ~APIKeyboardHook()
//            {
                
//                // Stop Hook
//                if (hHook != 0)
//                {
//                    UnhookWindowsHookEx(hHook);
//                }

//            }

//            private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
//            {


//                bool handled = false;

//                if (nCode > -1 && (KeyDown != null || KeyUp != null || KeyPress != null))
//                {

//                    KeyboardHookStruct keyboardHookStruct =
//                        (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

//                    // Is Control being held down?
//                    bool control = ((GetKeyState(VK_LCONTROL) & 0x80) != 0) ||
//                                   ((GetKeyState(VK_RCONTROL) & 0x80) != 0);

//                    // Is Shift being held down?
//                    bool shift = ((GetKeyState(VK_LSHIFT) & 0x80) != 0) ||
//                                 ((GetKeyState(VK_RSHIFT) & 0x80) != 0);

//                    // Is Alt being held down?
//                    bool alt = ((GetKeyState(VK_LALT) & 0x80) != 0) ||
//                               ((GetKeyState(VK_RALT) & 0x80) != 0);

//                    // Is CapsLock on?
//                    bool capslock = (GetKeyState(VK_CAPITAL) != 0);

//                    // Create event using keycode and control/shift/alt values found above
//                    KeyEventArgs e = new KeyEventArgs(
//                        (Keys)(
//                            keyboardHookStruct.vkCode |
//                            (control ? (int)Keys.Control : 0) |
//                            (shift ? (int)Keys.Shift : 0) |
//                            (alt ? (int)Keys.Alt : 0)
//                            ));

//                    // Handle KeyDown and KeyUp events
//                    switch (wParam)
//                    {

//                        case WM_KEYDOWN:
//                        case WM_SYSKEYDOWN:
//                            if (KeyDown != null)
//                            {
//                                KeyDown(this, e);
//                                handled = handled || e.Handled;
//                            }
//                            break;
//                        case WM_KEYUP:
//                        case WM_SYSKEYUP:
//                            if (KeyUp != null)
//                            {
//                                KeyUp(this, e);
//                                handled = handled || e.Handled;
//                            }
//                            break;

//                    }

//                    // Handle KeyPress event
//                    if(wParam == WM_KEYDOWN && 
//                       !handled && 
//                       !e.SuppressKeyPress &&
//                        KeyPress != null)
//                    {

//                        byte[] keyState = new byte[256];
//                        byte[] inBuffer = new byte[2];
//                        GetKeyboardState(keyState);
                        
//                        if (ToAscii(keyboardHookStruct.vkCode,
//                                  keyboardHookStruct.scanCode,
//                                  keyState,
//                                  inBuffer,
//                                  keyboardHookStruct.flags) == 1)
//                        {
                            
//                            char key = (char)inBuffer[0];
//                            if ((capslock ^ shift) && Char.IsLetter(key)) 
//                                key = Char.ToUpper(key);
//                            KeyPressEventArgs e2 = new KeyPressEventArgs(key);
//                            KeyPress(this, e2);
//                            handled = handled || e.Handled;

//                        }

//                    }

//                }

//                if (handled)
//                {
//                    return 1;
//                }
//                else
//                {
//                    return CallNextHookEx(hHook, nCode, wParam, lParam);
//                }

//            }


//        }

//        #endregion

//    }

//}
