namespace TouchHook
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class WM_MouseHook : WindowsHook
    {
        private WindowsHook LowLevelMouseHook;
        private int timeStamp;

        public event EventHandler<MouseEventArgs> MouseDown;

        public event EventHandler<MouseEventArgs> MouseMove;

        public event EventHandler<MouseEventArgs> MouseUp;

        public WM_MouseHook(IntPtr hWnd) : base(hWnd, HookType.WH_MOUSE)
        {
            base.HookInvoked += new WindowsHook.HookEventHandler(this.MouseHookInvoked);
        }

        private MouseEventArgs DecodeLowLevelMouse(IntPtr lParam)
        {
            MSLLHOOKSTRUCT msllhookstruct = (MSLLHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            int time = msllhookstruct.time;
            int timeStamp = this.timeStamp;
            this.timeStamp = msllhookstruct.time;
            MouseEventArgs args = new MouseEventArgs();
            args.x = msllhookstruct.pt.X;
            args.y = msllhookstruct.pt.Y;
            return args;
        }

        private MouseEventArgs DecodeMouseMove(IntPtr lParam)
        {
            MOUSEHOOKSTRUCT mousehookstruct = (MOUSEHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof(MOUSEHOOKSTRUCT));
            MouseEventArgs args = new MouseEventArgs();
            args.x = mousehookstruct.pt.X;
            args.y = mousehookstruct.pt.Y;
            args.hitTestCode = mousehookstruct.wHitTestCode;
            return args;
        }

        public override void InstallHook()
        {
            base.InstallHook();
        }

        public int MouseHookInvoked(object sender, HookEventArgs e)
        {
            uint wParam = (uint) ((int) e.wParam);
            EventHandler<MouseEventArgs> mouseMove = null;
            MouseEventArgs args = null;
            switch (wParam)
            {
                case 0x200:
                    mouseMove = this.MouseMove;
                    args = this.DecodeMouseMove(e.lParam);
                    break;

                case 0x201:
                    mouseMove = this.MouseDown;
                    args = this.DecodeLowLevelMouse(e.lParam);
                    break;

                case 0x202:
                    mouseMove = this.MouseUp;
                    args = this.DecodeLowLevelMouse(e.lParam);
                    break;
            }
            if ((mouseMove != null) && (args != null))
            {
                mouseMove(this, args);
            }
            return 0;
        }

        public override void UninstallHook()
        {
            base.UninstallHook();
        }
    }
}

