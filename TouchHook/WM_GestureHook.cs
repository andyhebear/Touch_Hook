namespace TouchHook
{
    using System;
    using System.Runtime.InteropServices;

    internal class WM_GestureHook : WindowsHook
    {
        private GestureUpdate mAllGestures;
        private readonly int mdGestureConfigSize;
        private readonly int mdGestureInfoSize;
        private int mdNumConfigs;
        private GestureUpdate mPan;
        private LastGestureInfo mPanInfo;
        private GestureUpdate mPressAndTap;
        private GestureUpdate mRotate;
        private GestureUpdate mTwoFingerTap;
        private GestureUpdate mZoom;
        private LastGestureInfo mZoomInfo;
        public PanGestureEventHandler PanEventHandler;
        public RotateGestureEventHandler RotateEventHandler;
        public TwoFingerTapGestureEventHandler TwoFingerTapEventHandler;
        public ZoomGestureEventHandler ZoomEventHandler;

        public WM_GestureHook(IntPtr hWnd) : base(hWnd, HookType.WH_GETMESSAGE)
        {
            this.mdNumConfigs = 5;
            base.HookInvoked += new WindowsHook.HookEventHandler(this.GestureHookInvoked);
            this.mdGestureConfigSize = Marshal.SizeOf(new Win32.GESTURECONFIG());
            this.mdGestureInfoSize = Marshal.SizeOf(new Win32.GESTUREINFO());
            this.mAllGestures.config.dwID = 0;
            this.mAllGestures.config.dwWant = 1;
            this.mAllGestures.config.dwBlock = 0;
            this.mZoom.config.dwID = 3;
            this.mPan.config.dwID = 4;
            this.mRotate.config.dwID = 5;
            this.mTwoFingerTap.config.dwID = 6;
            this.mPressAndTap.config.dwID = 7;
            this.ClearGestureConfigs();
        }

        private void ClearGestureConfigs()
        {
            this.mZoom.config.dwWant = 0;
            this.mZoom.config.dwBlock = 0;
            this.mZoom.dirty = false;
            this.mPan.config.dwWant = 0;
            this.mPan.config.dwBlock = 0;
            this.mPan.dirty = false;
            this.mRotate.config.dwWant = 0;
            this.mRotate.config.dwBlock = 0;
            this.mRotate.dirty = false;
            this.mTwoFingerTap.config.dwWant = 0;
            this.mTwoFingerTap.config.dwBlock = 0;
            this.mTwoFingerTap.dirty = false;
            this.mPressAndTap.config.dwWant = 0;
            this.mPressAndTap.config.dwBlock = 0;
            this.mPressAndTap.dirty = false;
        }

        public void ConfigureGesture(GestureId gesture)
        {
            this.ConfigureGesture(gesture, true);
        }

        public void ConfigureGesture(GestureId gesture, bool allow)
        {
            switch (gesture)
            {
                case GestureId.All:
                    this.SetFlag(ref this.mAllGestures.config, 1, allow);
                    break;

                case GestureId.Zoom:
                    this.SetFlag(ref this.mZoom.config, 1, allow);
                    this.mZoom.dirty = true;
                    break;

                case GestureId.Pan:
                    this.SetFlag(ref this.mPan.config, 1, allow);
                    this.mPan.dirty = true;
                    break;

                case GestureId.Pan_Single_Vert:
                    this.SetFlag(ref this.mPan.config, 2, allow);
                    this.mPan.dirty = true;
                    break;

                case GestureId.Pan_Single_Hor:
                    this.SetFlag(ref this.mPan.config, 4, allow);
                    this.mPan.dirty = true;
                    break;

                case GestureId.Pan_With_Gutter:
                    this.SetFlag(ref this.mPan.config, 8, allow);
                    this.mPan.dirty = true;
                    break;

                case GestureId.Pan_With_Inertia:
                    this.SetFlag(ref this.mPan.config, 0x10, allow);
                    this.mPan.dirty = true;
                    break;

                case GestureId.Rotate:
                    this.SetFlag(ref this.mRotate.config, 1, allow);
                    this.mRotate.dirty = true;
                    break;

                case GestureId.TwoFingerTap:
                    this.SetFlag(ref this.mTwoFingerTap.config, 1, allow);
                    this.mTwoFingerTap.dirty = true;
                    break;

                case GestureId.PressAndTap:
                    this.SetFlag(ref this.mPressAndTap.config, 1, allow);
                    this.mPressAndTap.dirty = true;
                    break;
            }
            if (gesture == GestureId.All)
            {
                this.mAllGestures.dirty = true;
                this.ClearGestureConfigs();
            }
            else
            {
                this.mAllGestures.dirty = false;
            }
        }

        private void ConfigureGesturesForSubmission()
        {
            Win32.GESTURECONFIG[] gestureconfigArray;
            int cIDs = 0;
            if (this.mAllGestures.dirty)
            {
                cIDs = 1;
                gestureconfigArray = new Win32.GESTURECONFIG[cIDs];
                gestureconfigArray[0] = this.mAllGestures.config;
            }
            else
            {
                if (this.mPan.dirty)
                {
                    cIDs++;
                }
                if (this.mZoom.dirty)
                {
                    cIDs++;
                }
                if (this.mRotate.dirty)
                {
                    cIDs++;
                }
                if (this.mPressAndTap.dirty)
                {
                    cIDs++;
                }
                if (this.mTwoFingerTap.dirty)
                {
                    cIDs++;
                }
                gestureconfigArray = new Win32.GESTURECONFIG[cIDs];
                int num2 = 0;
                if (this.mPan.dirty)
                {
                    gestureconfigArray[num2++] = this.mPan.config;
                }
                if (this.mZoom.dirty)
                {
                    gestureconfigArray[num2++] = this.mZoom.config;
                }
                if (this.mRotate.dirty)
                {
                    gestureconfigArray[num2++] = this.mRotate.config;
                }
                if (this.mPressAndTap.dirty)
                {
                    gestureconfigArray[num2++] = this.mPressAndTap.config;
                }
                if (this.mTwoFingerTap.dirty)
                {
                    gestureconfigArray[num2++] = this.mTwoFingerTap.config;
                }
            }
            if (!Win32.SetGestureConfig(base.hWnd, 0, cIDs, ref gestureconfigArray, this.mdGestureConfigSize))
            {
                throw new Exception("Error in execution of SetGestureConfig");
            }
        }

        private void DecodeGesture(ref Message message)
        {
            Win32.GESTUREINFO pGestureInfo = new Win32.GESTUREINFO();
            pGestureInfo.cbSize = this.mdGestureInfoSize;
            if (Win32.GetGestureInfo(message.lparam, ref pGestureInfo))
            {
                switch (pGestureInfo.dwID)
                {
                    case 1:
                    case 2:
                        Win32.DefWindowProc(base.hWnd, message.msg, message.lparam, message.wparam);
                        break;

                    case 3:
                        this.DecodeZoomGesture(ref pGestureInfo);
                        break;

                    case 4:
                        this.DecodePanGesture(ref pGestureInfo);
                        break;

                    case 5:
                        this.DecodeRotateGesture(ref pGestureInfo);
                        break;

                    case 6:
                        this.DecodeTwoFingerTapGesture(ref pGestureInfo);
                        break;

                    case 7:
                        this.DecodePressAndTapGesture(ref pGestureInfo);
                        break;
                }
                Win32.CloseGestureInfoHandle(message.lparam);
            }
        }

        private void DecodePanGesture(ref Win32.GESTUREINFO gestureInfo)
        {
            PanGestureEventArgs e = new PanGestureEventArgs();
            switch (gestureInfo.dwFlags)
            {
                case 1:
                    e.state = GestureState.Begin;
                    this.mPanInfo.ptFirst.X = gestureInfo.ptsLocation.X;
                    this.mPanInfo.ptFirst.Y = gestureInfo.ptsLocation.Y;
                    this.mPanInfo.ptSecond.X = 0;
                    this.mPanInfo.ptSecond.Y = 0;
                    break;

                case 2:
                    e.state = GestureState.Inertia;
                    this.mPanInfo.ptSecond.X = gestureInfo.ptsLocation.X;
                    this.mPanInfo.ptSecond.Y = gestureInfo.ptsLocation.Y;
                    break;

                case 4:
                    e.state = GestureState.End;
                    this.mPanInfo.ptSecond.X = gestureInfo.ptsLocation.X;
                    this.mPanInfo.ptSecond.Y = gestureInfo.ptsLocation.Y;
                    break;

                default:
                    e.state = GestureState.Move;
                    this.mPanInfo.ptSecond.X = gestureInfo.ptsLocation.X;
                    this.mPanInfo.ptSecond.Y = gestureInfo.ptsLocation.Y;
                    break;
            }
            e.distance = (int) (((ulong) gestureInfo.ullArguments) & 0xffffffffL);
            e.ptFirst.X = this.mPanInfo.ptFirst.X;
            e.ptFirst.Y = this.mPanInfo.ptFirst.Y;
            e.ptSecond.X = this.mPanInfo.ptSecond.X;
            e.ptSecond.Y = this.mPanInfo.ptSecond.Y;
            if (this.PanEventHandler != null)
            {
                this.PanEventHandler(this, e);
            }
            this.mPanInfo.argument = (int) (((ulong) gestureInfo.ullArguments) & 0xffffffffL);
            if (gestureInfo.dwFlags != 1)
            {
                this.mPanInfo.ptFirst.X = this.mPanInfo.ptSecond.X;
                this.mPanInfo.ptFirst.Y = this.mPanInfo.ptSecond.Y;
            }
        }

        private void DecodePressAndTapGesture(ref Win32.GESTUREINFO gestureInfo)
        {
            PressAndTapGestureEventArgs args = new PressAndTapGestureEventArgs();
            switch (gestureInfo.dwFlags)
            {
                case 1:
                    args.state = GestureState.Begin;
                    return;

                case 4:
                    args.state = GestureState.End;
                    break;
            }
        }

        private void DecodeRotateGesture(ref Win32.GESTUREINFO gestureInfo)
        {
            RotateGestureEventArgs e = new RotateGestureEventArgs();
            if (gestureInfo.dwFlags == 1)
            {
                e.state = GestureState.Begin;
                e.initialRotation = Win32.ArgToRadians(gestureInfo.ullArguments & ((long) 0xffffffffL));
                e.rotation = 0.0;
            }
            else
            {
                if (gestureInfo.dwFlags == 4)
                {
                    e.state = GestureState.End;
                }
                else
                {
                    e.state = GestureState.Move;
                }
                e.rotation = Win32.ArgToRadians(gestureInfo.ullArguments & ((long) 0xffffffffL));
            }
            e.ptCenter.X = gestureInfo.ptsLocation.X;
            e.ptCenter.Y = gestureInfo.ptsLocation.Y;
            if (this.RotateEventHandler != null)
            {
                this.RotateEventHandler(this, e);
            }
        }

        private void DecodeTwoFingerTapGesture(ref Win32.GESTUREINFO gestureInfo)
        {
            TwoFingerTapGestureEventArgs e = new TwoFingerTapGestureEventArgs();
            switch (gestureInfo.dwFlags)
            {
                case 1:
                    e.state = GestureState.Begin;
                    break;

                case 4:
                    e.state = GestureState.End;
                    break;
            }
            e.distance = (int) (((ulong) gestureInfo.ullArguments) & 0xffffffffL);
            e.ptCenter.X = gestureInfo.ptsLocation.X;
            e.ptCenter.Y = gestureInfo.ptsLocation.Y;
            if (this.TwoFingerTapEventHandler != null)
            {
                this.TwoFingerTapEventHandler(this, e);
            }
        }

        private void DecodeZoomGesture(ref Win32.GESTUREINFO gestureInfo)
        {
            ZoomGestureEventArgs e = new ZoomGestureEventArgs();
            if (gestureInfo.dwFlags == 1)
            {
                e.state = GestureState.Begin;
                this.mZoomInfo.ptFirst.X = gestureInfo.ptsLocation.X;
                this.mZoomInfo.ptFirst.Y = gestureInfo.ptsLocation.Y;
            }
            else
            {
                if (e.state == GestureState.End)
                {
                    e.state = GestureState.End;
                }
                else
                {
                    e.state = GestureState.Move;
                }
                this.mZoomInfo.ptSecond.X = gestureInfo.ptsLocation.X;
                this.mZoomInfo.ptSecond.Y = gestureInfo.ptsLocation.Y;
                new POINT((this.mZoomInfo.ptFirst.X + this.mZoomInfo.ptSecond.X) / 2, (this.mZoomInfo.ptFirst.Y + this.mZoomInfo.ptSecond.Y) / 2);
                e.zoom = ((double) (((ulong) gestureInfo.ullArguments) & 0xffffffffL)) / ((double) this.mZoomInfo.argument);
            }
            e.distance = (int) (((ulong) gestureInfo.ullArguments) & 0xffffffffL);
            e.ptFirst.X = this.mZoomInfo.ptFirst.X;
            e.ptFirst.Y = this.mZoomInfo.ptFirst.Y;
            e.ptSecond.X = this.mZoomInfo.ptSecond.X;
            e.ptSecond.Y = this.mZoomInfo.ptSecond.Y;
            if (this.ZoomEventHandler != null)
            {
                this.ZoomEventHandler(this, e);
            }
            this.mZoomInfo.argument = (int) (((ulong) gestureInfo.ullArguments) & 0xffffffffL);
            if (gestureInfo.dwFlags != 1)
            {
                this.mZoomInfo.ptFirst.X = this.mZoomInfo.ptSecond.X;
                this.mZoomInfo.ptFirst.Y = this.mZoomInfo.ptSecond.Y;
            }
        }

        public int GestureHookInvoked(object sender, HookEventArgs e)
        {
            switch (e.message.msg)
            {
                case 0x119:
                    this.DecodeGesture(ref e.message);
                    break;

                case 0x11a:
                    this.ConfigureGesturesForSubmission();
                    break;
            }
            return 0;
        }

        private void SetFlag(ref Win32.GESTURECONFIG config, int flag, bool on)
        {
            if (on)
            {
                config.dwWant |= flag;
                config.dwBlock &= ~flag;
            }
            else
            {
                config.dwWant &= ~flag;
                config.dwBlock |= flag;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct GestureUpdate
        {
            public Win32.GESTURECONFIG config;
            public bool dirty;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LastGestureInfo
        {
            public POINT ptFirst;
            public POINT ptSecond;
            public int argument;
        }
    }
}

