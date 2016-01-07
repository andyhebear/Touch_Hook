namespace TouchHook
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CallWndStruct
    {
        public IntPtr lparam;
        public IntPtr wparam;
        public uint message;
        public IntPtr hwnd;
    }
}

