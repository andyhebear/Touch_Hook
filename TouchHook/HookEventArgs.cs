namespace TouchHook
{
    using System;

    public class HookEventArgs : EventArgs
    {
        public CallWndStruct cwstruct;
        public int HookCode;
        public IntPtr lParam;
        public Message message;
        public IntPtr wParam;
    }
}

