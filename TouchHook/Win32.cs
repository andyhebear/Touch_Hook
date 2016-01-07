namespace TouchHook
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class Win32
    {
        public const int GC_ALLGESTURES = 1;
        public const int GC_PAN = 1;
        public const int GC_PAN_WITH_GUTTER = 8;
        public const int GC_PAN_WITH_INERTIA = 0x10;
        public const int GC_PAN_WITH_SINGLE_FINGER_HORIZONTALLY = 4;
        public const int GC_PAN_WITH_SINGLE_FINGER_VERTICALLY = 2;
        public const int GC_PRESSANDTAP = 1;
        public const int GC_ROLLOVER = 1;
        public const int GC_ROTATE = 1;
        public const int GC_TWOFINGERTAP = 1;
        public const int GC_ZOOM = 1;
        public const int GESTURECONFIGMAXCOUNT = 0x100;
        public const int GF_BEGIN = 1;
        public const int GF_END = 4;
        public const int GF_INERTIA = 2;
        public const int GID_BEGIN = 1;
        public const int GID_END = 2;
        public const int GID_PAN = 4;
        public const int GID_PRESSANDTAP = 7;
        public const int GID_ROTATE = 5;
        public const int GID_TWOFINGERTAP = 6;
        public const int GID_ZOOM = 3;
        public const int GWL_EXSTYLE = -20;
        public const int GWL_HINSTANCE = -6;
        public const int GWL_HWNDPARENT = -8;
        public const int GWL_ID = -12;
        public const int GWL_STYLE = -16;
        public const int GWL_USERDATA = -21;
        public const int GWL_WNDPROC = -4;
        public const uint MOUSEEVENTF_FROMTOUCH = 0xff515700;
        public const int SM_DIGITIZER = 0x5e;
        public const int SM_MAXIMUMTOUCHES = 0x5f;
        public const uint TABLET_DISABLE_FLICKFALLBACKKEYS = 0x100000;
        public const uint TABLET_DISABLE_FLICKS = 0x10000;
        public const uint TABLET_DISABLE_PENBARRELFEEDBACK = 0x10;
        public const uint TABLET_DISABLE_PENTAPFEEDBACK = 8;
        public const uint TABLET_DISABLE_PRESSANDHOLD = 1;
        public const uint TABLET_DISABLE_SMOOTHSCROLLING = 0x80000;
        public const uint TABLET_DISABLE_TOUCHSWITCH = 0x8000;
        public const uint TABLET_DISABLE_TOUCHUIFORCEOFF = 0x200;
        public const uint TABLET_DISABLE_TOUCHUIFORCEON = 0x100;
        public const uint TABLET_ENABLE_FLICKLEARNINGMODE = 0x40000;
        public const uint TABLET_ENABLE_FLICKSONCONTEXT = 0x20000;
        public const uint TABLET_ENABLE_MULTITOUCHDATA = 0x1000000;
        public const uint TOUCHEVENTF_DOWN = 2;
        public const uint TOUCHEVENTF_INRANGE = 8;
        public const uint TOUCHEVENTF_MOVE = 1;
        public const uint TOUCHEVENTF_NOCOALESCE = 0x20;
        public const uint TOUCHEVENTF_PALM = 0x80;
        public const uint TOUCHEVENTF_PEN = 0x40;
        public const uint TOUCHEVENTF_PRIMARY = 0x10;
        public const uint TOUCHEVENTF_UP = 4;
        public const uint TOUCHINPUTMASKF_CONTACTAREA = 4;
        public const uint TOUCHINPUTMASKF_EXTRAINFO = 2;
        public const uint TOUCHINPUTMASKF_TIMEFROMSYSTEM = 1;
        public const uint TWF_FINETOUCH = 1;
        public const uint TWF_NONE = 0;
        public const uint TWF_WANTPALM = 2;
        public const long ULL_ARGUMENTS_BIT_MASK = 0xffffffffL;
        public const uint WM_ACTIVATE = 6;
        public const uint WM_ACTIVATEAPP = 0x1c;
        public const uint WM_AFXFIRST = 0x360;
        public const uint WM_AFXLAST = 0x37f;
        public const uint WM_APP = 0x8000;
        public const uint WM_ASKCBFORMATNAME = 780;
        public const uint WM_CANCELJOURNAL = 0x4b;
        public const uint WM_CANCELMODE = 0x1f;
        public const uint WM_CAPTURECHANGED = 0x215;
        public const uint WM_CHANGECBCHAIN = 0x30d;
        public const uint WM_CHANGEUISTATE = 0x127;
        public const uint WM_CHAR = 0x102;
        public const uint WM_CHARTOITEM = 0x2f;
        public const uint WM_CHILDACTIVATE = 0x22;
        public const uint WM_CLEAR = 0x303;
        public const uint WM_CLOSE = 0x10;
        public const uint WM_COMMAND = 0x111;
        public const uint WM_COMPACTING = 0x41;
        public const uint WM_COMPAREITEM = 0x39;
        public const uint WM_CONTEXTMENU = 0x7b;
        public const uint WM_COPY = 0x301;
        public const uint WM_COPYDATA = 0x4a;
        public const uint WM_CREATE = 1;
        public const uint WM_CTLCOLORBTN = 0x135;
        public const uint WM_CTLCOLORDLG = 310;
        public const uint WM_CTLCOLOREDIT = 0x133;
        public const uint WM_CTLCOLORLISTBOX = 0x134;
        public const uint WM_CTLCOLORMSGBOX = 0x132;
        public const uint WM_CTLCOLORSCROLLBAR = 0x137;
        public const uint WM_CTLCOLORSTATIC = 0x138;
        public const uint WM_CUT = 0x300;
        public const uint WM_DEADCHAR = 0x103;
        public const uint WM_DELETEITEM = 0x2d;
        public const uint WM_DESTROY = 2;
        public const uint WM_DESTROYCLIPBOARD = 0x307;
        public const uint WM_DEVICECHANGE = 0x219;
        public const uint WM_DEVMODECHANGE = 0x1b;
        public const uint WM_DISPLAYCHANGE = 0x7e;
        public const uint WM_DRAWCLIPBOARD = 0x308;
        public const uint WM_DRAWITEM = 0x2b;
        public const uint WM_DROPFILES = 0x233;
        public const uint WM_ENABLE = 10;
        public const uint WM_ENDSESSION = 0x16;
        public const uint WM_ENTERIDLE = 0x121;
        public const uint WM_ENTERMENULOOP = 0x211;
        public const uint WM_ENTERSIZEMOVE = 0x231;
        public const uint WM_ERASEBKGND = 20;
        public const uint WM_EXITMENULOOP = 530;
        public const uint WM_EXITSIZEMOVE = 0x232;
        public const uint WM_FONTCHANGE = 0x1d;
        public const int WM_GESTURE = 0x119;
        public const int WM_GESTURENOTIFY = 0x11a;
        public const uint WM_GETDLGCODE = 0x87;
        public const uint WM_GETFONT = 0x31;
        public const uint WM_GETHOTKEY = 0x33;
        public const uint WM_GETICON = 0x7f;
        public const uint WM_GETMINMAXINFO = 0x24;
        public const uint WM_GETOBJECT = 0x3d;
        public const uint WM_GETTEXT = 13;
        public const uint WM_GETTEXTLENGTH = 14;
        public const uint WM_HANDHELDFIRST = 0x358;
        public const uint WM_HANDHELDLAST = 0x35f;
        public const uint WM_HELP = 0x53;
        public const uint WM_HOTKEY = 0x312;
        public const uint WM_HSCROLL = 0x114;
        public const uint WM_HSCROLLCLIPBOARD = 0x30e;
        public const uint WM_ICONERASEBKGND = 0x27;
        public const uint WM_IME_CHAR = 0x286;
        public const uint WM_IME_COMPOSITION = 0x10f;
        public const uint WM_IME_COMPOSITIONFULL = 0x284;
        public const uint WM_IME_CONTROL = 0x283;
        public const uint WM_IME_ENDCOMPOSITION = 270;
        public const uint WM_IME_KEYDOWN = 0x290;
        public const uint WM_IME_KEYLAST = 0x10f;
        public const uint WM_IME_KEYUP = 0x291;
        public const uint WM_IME_NOTIFY = 0x282;
        public const uint WM_IME_REQUEST = 0x288;
        public const uint WM_IME_SELECT = 0x285;
        public const uint WM_IME_SETCONTEXT = 0x281;
        public const uint WM_IME_STARTCOMPOSITION = 0x10d;
        public const uint WM_INITDIALOG = 0x110;
        public const uint WM_INITMENU = 0x116;
        public const uint WM_INITMENUPOPUP = 0x117;
        public const uint WM_INPUTLANGCHANGE = 0x51;
        public const uint WM_INPUTLANGCHANGEREQUEST = 80;
        public const uint WM_KEYDOWN = 0x100;
        public const uint WM_KEYFIRST = 0x100;
        public const uint WM_KEYLAST = 0x108;
        public const uint WM_KEYUP = 0x101;
        public const uint WM_KILLFOCUS = 8;
        public const uint WM_LBUTTONDBLCLK = 0x203;
        public const uint WM_LBUTTONDOWN = 0x201;
        public const uint WM_LBUTTONUP = 0x202;
        public const uint WM_MBUTTONDBLCLK = 0x209;
        public const uint WM_MBUTTONDOWN = 0x207;
        public const uint WM_MBUTTONUP = 520;
        public const uint WM_MDIACTIVATE = 0x222;
        public const uint WM_MDICASCADE = 0x227;
        public const uint WM_MDICREATE = 0x220;
        public const uint WM_MDIDESTROY = 0x221;
        public const uint WM_MDIGETACTIVE = 0x229;
        public const uint WM_MDIICONARRANGE = 0x228;
        public const uint WM_MDIMAXIMIZE = 0x225;
        public const uint WM_MDINEXT = 0x224;
        public const uint WM_MDIREFRESHMENU = 0x234;
        public const uint WM_MDIRESTORE = 0x223;
        public const uint WM_MDISETMENU = 560;
        public const uint WM_MDITILE = 550;
        public const uint WM_MEASUREITEM = 0x2c;
        public const uint WM_MENUCHAR = 0x120;
        public const uint WM_MENUCOMMAND = 0x126;
        public const uint WM_MENUDRAG = 0x123;
        public const uint WM_MENUGETOBJECT = 0x124;
        public const uint WM_MENURBUTTONUP = 290;
        public const uint WM_MENUSELECT = 0x11f;
        public const uint WM_MOUSEACTIVATE = 0x21;
        public const uint WM_MOUSEFIRST = 0x200;
        public const uint WM_MOUSEHOVER = 0x2a1;
        public const uint WM_MOUSEHWHEEL = 0x20e;
        public const uint WM_MOUSELAST = 0x20d;
        public const uint WM_MOUSELEAVE = 0x2a3;
        public const uint WM_MOUSEMOVE = 0x200;
        public const uint WM_MOUSEWHEEL = 0x20a;
        public const uint WM_MOVE = 3;
        public const uint WM_MOVING = 0x216;
        public const uint WM_NCACTIVATE = 0x86;
        public const uint WM_NCCALCSIZE = 0x83;
        public const uint WM_NCCREATE = 0x81;
        public const uint WM_NCDESTROY = 130;
        public const uint WM_NCHITTEST = 0x84;
        public const uint WM_NCLBUTTONDBLCLK = 0xa3;
        public const uint WM_NCLBUTTONDOWN = 0xa1;
        public const uint WM_NCLBUTTONUP = 0xa2;
        public const uint WM_NCMBUTTONDBLCLK = 0xa9;
        public const uint WM_NCMBUTTONDOWN = 0xa7;
        public const uint WM_NCMBUTTONUP = 0xa8;
        public const uint WM_NCMOUSEMOVE = 160;
        public const uint WM_NCPAINT = 0x85;
        public const uint WM_NCRBUTTONDBLCLK = 0xa6;
        public const uint WM_NCRBUTTONDOWN = 0xa4;
        public const uint WM_NCRBUTTONUP = 0xa5;
        public const uint WM_NEXTDLGCTL = 40;
        public const uint WM_NEXTMENU = 0x213;
        public const uint WM_NOTIFY = 0x4e;
        public const uint WM_NOTIFYFORMAT = 0x55;
        public const uint WM_NULL = 0;
        public const uint WM_PAINT = 15;
        public const uint WM_PAINTCLIPBOARD = 0x309;
        public const uint WM_PAINTICON = 0x26;
        public const uint WM_PALETTECHANGED = 0x311;
        public const uint WM_PALETTEISCHANGING = 0x310;
        public const uint WM_PARENTNOTIFY = 0x210;
        public const uint WM_PASTE = 770;
        public const uint WM_PENWINFIRST = 0x380;
        public const uint WM_PENWINLAST = 0x38f;
        public const uint WM_POWER = 0x48;
        public const uint WM_POWERBROADCAST = 0x218;
        public const uint WM_PRINT = 0x317;
        public const uint WM_PRINTCLIENT = 0x318;
        public const uint WM_QUERYDRAGICON = 0x37;
        public const uint WM_QUERYENDSESSION = 0x11;
        public const uint WM_QUERYNEWPALETTE = 0x30f;
        public const uint WM_QUERYOPEN = 0x13;
        public const uint WM_QUEUESYNC = 0x23;
        public const uint WM_QUIT = 0x12;
        public const uint WM_RBUTTONDBLCLK = 0x206;
        public const uint WM_RBUTTONDOWN = 0x204;
        public const uint WM_RBUTTONUP = 0x205;
        public const uint WM_RENDERALLFORMATS = 0x306;
        public const uint WM_RENDERFORMAT = 0x305;
        public const uint WM_SETCURSOR = 0x20;
        public const uint WM_SETFOCUS = 7;
        public const uint WM_SETFONT = 0x30;
        public const uint WM_SETHOTKEY = 50;
        public const uint WM_SETICON = 0x80;
        public const uint WM_SETREDRAW = 11;
        public const uint WM_SETTEXT = 12;
        public const uint WM_SETTINGCHANGE = 0x1a;
        public const uint WM_SHOWWINDOW = 0x18;
        public const uint WM_SIZE = 5;
        public const uint WM_SIZECLIPBOARD = 0x30b;
        public const uint WM_SIZING = 0x214;
        public const uint WM_SPOOLERSTATUS = 0x2a;
        public const uint WM_STYLECHANGED = 0x7d;
        public const uint WM_STYLECHANGING = 0x7c;
        public const uint WM_SYNCPAINT = 0x88;
        public const uint WM_SYSCHAR = 0x106;
        public const uint WM_SYSCOLORCHANGE = 0x15;
        public const uint WM_SYSCOMMAND = 0x112;
        public const uint WM_SYSDEADCHAR = 0x107;
        public const uint WM_SYSKEYDOWN = 260;
        public const uint WM_SYSKEYUP = 0x105;
        public const uint WM_TABLET_ADDED = 0x2c8;
        public const uint WM_TABLET_DEFBASE = 0x2c0;
        public const uint WM_TABLET_DELETED = 0x2c9;
        public const uint WM_TABLET_FLICK = 0x2cb;
        public const uint WM_TABLET_MAXOFFSET = 0x20;
        public const uint WM_TABLET_QUERYSYSTEMGESTURESTATUS = 0x2cc;
        public const uint WM_TCARD = 0x52;
        public const uint WM_TIMECHANGE = 30;
        public const uint WM_TIMER = 0x113;
        public const uint WM_TOUCH = 0x240;
        public const uint WM_UNDO = 0x304;
        public const uint WM_UNINITMENUPOPUP = 0x125;
        public const uint WM_USER = 0x400;
        public const uint WM_USERCHANGED = 0x54;
        public const uint WM_VKEYTOITEM = 0x2e;
        public const uint WM_VSCROLL = 0x115;
        public const uint WM_VSCROLLCLIPBOARD = 0x30a;
        public const uint WM_WINDOWPOSCHANGED = 0x47;
        public const uint WM_WINDOWPOSCHANGING = 70;
        public const uint WM_WININICHANGE = 0x1a;
        public const uint WM_XBUTTONDBLCLK = 0x20d;
        public const uint WM_XBUTTONDOWN = 0x20b;
        public const uint WM_XBUTTONUP = 0x20c;

        public static double ArgToRadians(long arg)
        {
            return ((((((double) arg) / 65535.0) * 4.0) * 3.14159265) - 6.2831853);
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref CallWndStruct cwstruct);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref Message m);
        [DllImport("user32.dll")]
        public static extern int CallWindowProc(int lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32")]
        public static extern bool CloseGestureInfoHandle(IntPtr lParam);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool CloseTouchInputHandle(IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        public static int GET_X_LPARAM(IntPtr lParam)
        {
            return (short) ((int) lParam);
        }

        public static int GET_Y_LPARAM(IntPtr lParam)
        {
            return (short) (((int) lParam) >> 0x10);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32")]
        public static extern bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo);
        [DllImport("Kernel32.dll")]
        public static extern int GetLastError();
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr GetMessageExtraInfo();
        [DllImport("kernel32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int GetSystemMetrics(int nIndex);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool GetTouchInputInfo(IntPtr hTouchInput, int cInputs, [In, Out] TOUCHINPUT[] pInputs, int cbSize);
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr window, IntPtr module);
        public static POINT MakePoint(IntPtr lParam)
        {
            MOUSEHOOKSTRUCT mousehookstruct = (MOUSEHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof(MOUSEHOOKSTRUCT));
            return mousehookstruct.pt;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool RegisterTouchWindow(IntPtr hWnd, uint ulFlags);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32")]
        public static extern bool SetGestureConfig(IntPtr hWnd, int dwReserved, int cIDs, ref GESTURECONFIG[] pGestureConfig, int cbSize);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, WndProcDelegate newProc);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr SetWindowsHookEx(HookType hook, WindowsHook.HookDelegate callback, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(ref Message m);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [StructLayout(LayoutKind.Sequential)]
        public struct GESTURECONFIG
        {
            public int dwID;
            public int dwWant;
            public int dwBlock;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GESTUREINFO
        {
            public int cbSize;
            public int dwFlags;
            public int dwID;
            public IntPtr hwndTarget;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT ptsLocation;
            public int dwInstanceID;
            public int dwSequenceID;
            public long ullArguments;
            public int cbExtraArgs;
        }

        public delegate int WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}

