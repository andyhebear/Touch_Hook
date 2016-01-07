namespace TouchHook
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hWnd;
        public uint msg;
        public IntPtr wparam;
        public IntPtr lparam;
        public uint time;
        public POINT pt;
    }
}

