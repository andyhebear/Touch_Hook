namespace TouchHook
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Threading;

    public class WindowsHook : IDisposable
    {
        protected IntPtr hHook;
        protected HookDelegate hookDelegate;
        protected HookType hookType;
        protected IntPtr hWnd;

        public event HookEventHandler HookInvoked;

        public WindowsHook(IntPtr hWnd, HookType hook)
        {
            this.hHook = IntPtr.Zero;
            this.hWnd = IntPtr.Zero;
            this.hWnd = hWnd;
            this.hookType = hook;
            this.hookDelegate = new HookDelegate(this.CoreHookProc);
        }

        public WindowsHook(IntPtr hWnd, HookType hook, HookDelegate proc)
        {
            this.hHook = IntPtr.Zero;
            this.hWnd = IntPtr.Zero;
            this.hWnd = hWnd;
            this.hookType = hook;
            this.hookDelegate = proc;
        }

        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        protected int CoreHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            HookEventArgs e = new HookEventArgs();
            e.HookCode = nCode;
            e.wParam = wParam;
            e.lParam = lParam;
            if (this.hookType == HookType.WH_CALLWNDPROC)
            {
                e.cwstruct = (CallWndStruct) Marshal.PtrToStructure(lParam, typeof(CallWndStruct));
            }
            else if (this.hookType == HookType.WH_GETMESSAGE)
            {
                e.message = (Message) Marshal.PtrToStructure(lParam, typeof(Message));
            }
            if (nCode >= 0)
            {
                int num = this.OnHookInvoked(e);
                if (num != 0)
                {
                    return num;
                }
            }
            if (this.hookType == HookType.WH_CALLWNDPROC)
            {
                return Win32.CallNextHookEx(this.hHook, nCode, wParam, ref e.cwstruct);
            }
            return Win32.CallNextHookEx(this.hHook, nCode, wParam, ref e.message);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            this.UninstallHook();
        }

        ~WindowsHook()
        {
            this.Dispose(false);
        }

        public virtual void InstallHook()
        {
            IntPtr moduleHandle = Win32.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
            uint windowThreadProcessId = Win32.GetWindowThreadProcessId(this.hWnd, IntPtr.Zero);
            this.hHook = Win32.SetWindowsHookEx(this.hookType, this.hookDelegate, moduleHandle, windowThreadProcessId);
        }

        protected int OnHookInvoked(HookEventArgs e)
        {
            int num = 0;
            if (this.HookInvoked != null)
            {
                num = this.HookInvoked(this, e);
            }
            return num;
        }

        public virtual void UninstallHook()
        {
            if (this.hHook != IntPtr.Zero)
            {
                Win32.UnhookWindowsHookEx(this.hHook);
            }
        }

        public delegate int HookDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate int HookEventHandler(object sender, HookEventArgs e);
    }
}

