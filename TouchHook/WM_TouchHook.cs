namespace TouchHook
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class WM_TouchHook : WindowsHook
    {
        [CompilerGenerated]
        private static bool _DisableNativePressAndHoldGesture_k__BackingField;
        public static List<CapturedTouch> capturedTouches = new List<CapturedTouch>();
        private Win32.WndProcDelegate gestureDelegate;
        private int lpPrevWndProc;
        private int mdTouchInputSize;
        private float scalingFactorX;
        private float scalingFactorY;

        public event EventHandler<TouchEventArgs> TouchDown;

        public event EventHandler<TouchEventArgs> TouchMove;

        public event EventHandler<TouchEventArgs> TouchUp;

        public WM_TouchHook(IntPtr hWnd, HookType hookType) : base(hWnd, hookType)
        {
            this.scalingFactorX = 1f;
            this.scalingFactorY = 1f;
            DisableNativePressAndHoldGesture = true;
            this.mdTouchInputSize = Marshal.SizeOf(new TOUCHINPUT());
            this.gestureDelegate = new Win32.WndProcDelegate(this.GestureWndProc);
        }

        private void DecodeTouch(IntPtr wparam, IntPtr lparam)
        {
            int cInputs = LoWord(wparam.ToInt32());
            if (cInputs == 0)
            {
                Win32.CloseTouchInputHandle(lparam);
            }
            else
            {
                TOUCHINPUT[] pInputs = new TOUCHINPUT[cInputs];
                if (!Win32.GetTouchInputInfo(lparam, cInputs, pInputs, this.mdTouchInputSize))
                {
                    Win32.GetLastError();
                }
                else
                {
                    for (int i = 0; i < cInputs; i++)
                    {
                        TOUCHINPUT touchinput = pInputs[i];
                        EventHandler<TouchEventArgs> touchDown = null;
                        if ((touchinput.dwFlags & 2L) != 0L)
                        {
                            touchDown = this.TouchDown;
                        }
                        else if ((touchinput.dwFlags & 4L) != 0L)
                        {
                            touchDown = this.TouchUp;
                        }
                        else if ((touchinput.dwFlags & 1L) != 0L)
                        {
                            touchDown = this.TouchMove;
                        }
                        if (touchDown != null)
                        {
                            TouchEventArgs e = new TouchEventArgs();
                            e.contactX = touchinput.cxContact / 100;
                            e.contactY = touchinput.cyContact / 100;
                            e.id = touchinput.dwID;
                            e.x = (int) (((float) (touchinput.x / 100)) / this.scalingFactorX);
                            e.y = (int) (((float) (touchinput.y / 100)) / this.scalingFactorY);
                            e.time = touchinput.dwTime;
                            e.mask = touchinput.dwMask;
                            e.flags = touchinput.dwFlags;
                            touchDown(this, e);
                        }
                    }
                    Win32.CloseTouchInputHandle(lparam);
                }
            }
        }

        private static bool FoundSimilarTouchCapture(uint captureTime, IntPtr lParam)
        {
            return false;
        }

        protected int GestureWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == 0x2cc)
            {
                if (DisableNativePressAndHoldGesture)
                {
                    return 1;
                }
            }
            else if (msg == 0x240)
            {
                this.DecodeTouch(wParam, lParam);
                return 0;
            }
            return Win32.CallWindowProc(this.lpPrevWndProc, hWnd, msg, wParam, lParam);
        }

        [DllImport("gdi32.dll", CharSet=CharSet.Auto, SetLastError=true, ExactSpelling=true)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);
        public int GetMaxTouches()
        {
            return Win32.GetSystemMetrics(0x5f);
        }

        public override void InstallHook()
        {
            base.InstallHook();
            this.lpPrevWndProc = Win32.SetWindowLong(base.hWnd, -4, this.gestureDelegate);
            if (this.lpPrevWndProc == 0)
            {
                Console.WriteLine("Setting of the new WndProc failed...");
            }
            try
            {
                if (!Win32.RegisterTouchWindow(base.hWnd, 2))
                {
                    this.UninstallHook();
                }
            }
            catch
            {
                this.UninstallHook();
            }
        }

        public bool IsTouchAvailable()
        {
            return ((Win32.GetSystemMetrics(0x5e) & 0x40) > 0);
        }

        private static int LoWord(int number)
        {
            return (number & 0xffff);
        }

        private void OutputCapturedTouches()
        {
            for (int i = 0; i < capturedTouches.Count; i++)
            {
                Console.Write(string.Concat(new object[] { "Captured Touch id #", capturedTouches[i].id, " ", capturedTouches[i].point.X, ":", capturedTouches[i].point.Y, "\n" }));
            }
        }

        public int TouchHookInvoked(object sender, HookEventArgs e)
        {
            if (base.hookType == HookType.WH_CALLWNDPROC)
            {
                if (e.cwstruct.message == 0x240)
                {
                    this.DecodeTouch(e.cwstruct.wparam, e.cwstruct.lparam);
                }
            }
            else if (base.hookType == HookType.WH_GETMESSAGE)
            {
                switch (e.message.msg)
                {
                    case 0x200:
                    case 0x201:
                    case 0x202:
                    case 0x203:
                    case 0x2a3:
                    {
                        long num = Win32.GetMessageExtraInfo().ToInt64();
                        bool flag = false;
                        if ((((ulong) num) & 0xff515700L) == 0xff515700L)
                        {
                            flag = true;
                        }
                        else if ((e.message.msg == 0x200) && FoundSimilarTouchCapture(e.message.time, e.message.lparam))
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            e.message.msg = 0;
                        }
                        else
                        {
                            capturedTouches.Clear();
                        }
                        break;
                    }
                }
            }
            return 0;
        }

        public override void UninstallHook()
        {
            base.UninstallHook();
            Win32.SetWindowLong(base.hWnd, -4, this.lpPrevWndProc);
        }

        public static bool DisableNativePressAndHoldGesture
        {
            [CompilerGenerated]
            get
            {
                return _DisableNativePressAndHoldGesture_k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                _DisableNativePressAndHoldGesture_k__BackingField = value;
            }
        }

        public class CapturedTouch
        {
            public uint captureTime;
            public int id;
            public Point point;

            public CapturedTouch(int id, Point point, uint captureTime)
            {
                this.id = id;
                this.point = point;
                this.captureTime = captureTime;
            }
        }

        public enum DeviceCap
        {
            DESKTOPHORZRES = 0x76,
            DESKTOPVERTRES = 0x75,
            HORZRES = 8,
            VERTRES = 10
        }
    }
}

