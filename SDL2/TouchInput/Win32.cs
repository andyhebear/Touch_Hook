using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace ICTest
{
    // Destination object for touch/mouse/pen gestures.
    internal interface IInteractionHandler
    {
        void ProcessInteractionEvent(InteractionOutput output);
    }

    // Passed as an argument to ProcessInteractionEvent.
    internal class InteractionOutput
    {
        internal Win32.INTERACTION_CONTEXT_OUTPUT Data;

        internal IntPtr InteractionContext
        {
            get { return _interactionContext; }
        }

        internal bool IsBegin()
        {
            return (Data.InteractionFlags & Win32.INTERACTION_FLAGS.BEGIN) != 0;
        }

        internal bool IsInertia()
        {
            return (Data.InteractionFlags & Win32.INTERACTION_FLAGS.INERTIA) != 0;
        }

        internal bool IsEnd()
        {
            return (Data.InteractionFlags & Win32.INTERACTION_FLAGS.END) != 0;
        }

        #region ** infrastructure

        IntPtr _interactionContext;
        IInteractionHandler _interactionHandler;
        SynchronizationContext _syncContext;

        internal InteractionOutput(IntPtr interactionContext, IInteractionHandler interactionHandler, SynchronizationContext syncContext)
        {
            _interactionContext = interactionContext;
            _interactionHandler = interactionHandler;
            _syncContext = syncContext;
            Alive = true;
        }

        internal bool Alive { get; set; }

        internal IInteractionHandler InteractionHandler
        {
            get { return _interactionHandler; }
        }

        internal SynchronizationContext SyncContext
        {
            get { return _syncContext; }
        }

        #endregion
    }

    [System.Security.SuppressUnmanagedCodeSecurity()]
    internal static class Win32
    {
        //------------------------------------------------------------------
        #region ** fields and utility methods

        internal static readonly int CursorInfoSize;

        #region ** infrastructure

        static Timer _timer;
        static Queue<InteractionOutput> _inertiaQueue;
        static Dictionary<long, InteractionOutput> _listeners;
        static INTERACTION_CONTEXT_OUTPUT_CALLBACK _callback;
        static object _lockObj = new object();

        static Win32()
        {
            CursorInfoSize = Marshal.SizeOf(new CURSORINFO());
            _inertiaQueue = new Queue<InteractionOutput>();
            _listeners = new Dictionary<long, InteractionOutput>();
            _timer = new Timer(TimerElapsed, null, 100, 15);
            _callback = CallbackFunction;
        }

        static void TimerElapsed(object state)
        {
            InteractionOutput[] items = null;
            lock (_lockObj)
            {
                if (_inertiaQueue.Count > 0)
                {
                    items = _inertiaQueue.ToArray();
                    _inertiaQueue.Clear();
                }
            }
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    InteractionOutput io = items[i];
                    for (int j = 0; j < i; j++)
                    {
                        if (items[j].InteractionContext == io.InteractionContext)
                        {
                            io = null;
                            break;
                        }
                    }
                    if (io != null && io.Alive)
                    {
                        io.SyncContext.Post(delegate(object obj)
                        {
                            InteractionOutput m = (InteractionOutput)obj;
                            if (m.Alive)
                            {
                                ProcessInertiaInteractionContext(m.InteractionContext);
                            }
                        }, io);
                    }
                }
            }
        }

        static void CallbackFunction(IntPtr ic, IntPtr outputPtr)
        {
            InteractionOutput io;
            lock (_lockObj)
            {
                _listeners.TryGetValue(ic.ToInt64(), out io);
            }
            if (io != null)
            {
                io.Data = (INTERACTION_CONTEXT_OUTPUT)Marshal.PtrToStructure(outputPtr, typeof(INTERACTION_CONTEXT_OUTPUT));

                io.InteractionHandler.ProcessInteractionEvent(io);

                if (io.Alive && (io.Data.InteractionFlags & INTERACTION_FLAGS.INERTIA) != 0 &&
                    (io.Data.InteractionFlags & (INTERACTION_FLAGS.END | INTERACTION_FLAGS.CANCEL)) == 0)
                {
                    lock (_lockObj)
                    {
                        _inertiaQueue.Enqueue(io);
                    }
                }
            }
        }

        #endregion

        internal static IntPtr CreateInteractionContext(IInteractionHandler interactionHandler, SynchronizationContext syncContext)
        {
            IntPtr ic;
            CreateInteractionContext(out ic);
            InteractionOutput io = new InteractionOutput(ic, interactionHandler, syncContext);
            lock (_lockObj)
            {
                _listeners.Add(ic.ToInt64(), io);
            }
            RegisterOutputCallbackInteractionContext(ic, Marshal.GetFunctionPointerForDelegate(_callback), ic);
            return ic;
        }

        internal static void DisposeInteractionContext(IntPtr interactionContext)
        {
            if (interactionContext != IntPtr.Zero)
            {
                InteractionOutput io;
                lock (_lockObj)
                {
                    if (_listeners.TryGetValue(interactionContext.ToInt64(), out io))
                    {
                        io.Alive = false;
                        _listeners.Remove(interactionContext.ToInt64());
                    }
                }
                DestroyInteractionContext(interactionContext);
            }
        }

        internal static int GET_POINTER_ID(IntPtr wParam)
        {
            return LOWORD(wParam);
        }

        internal static bool IS_POINTER_FLAG_SET(IntPtr wParam, POINTER_MESSAGE_FLAG flag)
        {
            return (HIWORD(wParam) & (int)flag) != 0;
        }

        internal static int HIWORD(IntPtr wParam)
        {
            return (int)((wParam.ToInt64() >> 16) & 0xffff);
        }

        internal static int LOWORD(IntPtr wParam)
        {
            return (int)(wParam.ToInt64() & 0xffff);
        }

        internal static int GET_WHEEL_DELTA(IntPtr wParam)
        {
            return GET_Y(wParam);
        }

        internal static int GET_X(IntPtr lParam)
        {
            return (int)((short)(lParam.ToInt64() & 0xffff));
        }

        internal static int GET_Y(IntPtr lParam)
        {
            return (int)((short)((lParam.ToInt64() >> 16) & 0xffff));
        }

        internal static bool SUCCEEDED(IntPtr hr)
        {
            return unchecked((int)hr.ToInt64()) >= 0;
        }

        internal static bool FAILED(IntPtr hr)
        {
            return unchecked((int)hr.ToInt64()) < 0;
        }

        internal static string GetMessageForHR(IntPtr hr)
        {
            Exception ex = Marshal.GetExceptionForHR(unchecked((int)hr.ToInt64()));
            return ex != null ? ex.Message : string.Empty;
        }

        internal static bool IsCursorSuppressed()
        {
            CURSORINFO ci = new CURSORINFO();
            ci.Size = CursorInfoSize;
            if (GetCursorInfo(ref ci))
            {
                if (ci.Flags == CURSOR_STATE.SUPPRESSED)
                {
                    return true;
                }
            }
            return false;
        }

        internal static void CheckLastError()
        {
            int errCode = Marshal.GetLastWin32Error();
            if (errCode != 0)
            {
                throw new Win32Exception(errCode);
            }
        }

        #endregion

        //------------------------------------------------------------------
        #region ** misc constants

        internal const int
            GWFS_INCLUDE_ANCESTORS = 0x00000001;

        internal enum FEEDBACK_TYPE
        {
            TOUCH_CONTACTVISUALIZATION = 1,
            PEN_BARRELVISUALIZATION = 2,
            PEN_TAP = 3,
            PEN_DOUBLETAP = 4,
            PEN_PRESSANDHOLD = 5,
            PEN_RIGHTTAP = 6,
            TOUCH_TAP = 7,
            TOUCH_DOUBLETAP = 8,
            TOUCH_PRESSANDHOLD = 9,
            TOUCH_RIGHTTAP = 10,
            GESTURE_PRESSANDTAP = 11
        }

        internal const int
            SPI_GETCONTACTVISUALIZATION = 0x2018,
            SPI_SETCONTACTVISUALIZATION = 0x2019,
            SPI_GETGESTUREVISUALIZATION = 0x201A,
            SPI_SETGESTUREVISUALIZATION = 0x201B;

        [Flags]
        internal enum CONTACTVISUALIZATION
        {
            OFF = 0x0000,
            ON = 0x0001,
            PRESENTATIONMODE = 0x0002,
        }

        [Flags]
        internal enum GESTUREVISUALIZATION
        {
            OFF = 0x0000,
            ON = 0x001F,
            TAP = 0x0001,
            DOUBLETAP = 0x0002,
            PRESSANDTAP = 0x0004,
            PRESSANDHOLD = 0x0008,
            RIGHTTAP = 0x0010,
        }

        internal enum CURSOR_STATE
        {
            HIDDEN,
            SHOWING,
            SUPPRESSED
        }

        #endregion

        //------------------------------------------------------------------
        #region ** pointer constants

        internal const int

            WM_PARENTNOTIFY = 0x0210,
            WM_NCPOINTERUPDATE = 0x0241,
            WM_NCPOINTERDOWN = 0x0242,
            WM_NCPOINTERUP = 0x0243,
            WM_POINTERUPDATE = 0x0245,
            WM_POINTERDOWN = 0x0246,
            WM_POINTERUP = 0x0247,
            WM_POINTERENTER = 0x0249,
            WM_POINTERLEAVE = 0x024A,
            WM_POINTERACTIVATE = 0x024B,
            WM_POINTERCAPTURECHANGED = 0x024C,
            WM_POINTERWHEEL = 0x024E,
            WM_POINTERHWHEEL = 0x024F,

            // WM_POINTERACTIVATE return codes
            PA_ACTIVATE = 1,
            PA_NOACTIVATE = 3,

            MAX_TOUCH_COUNT = 256;

        internal enum POINTER_INPUT_TYPE
        {
            POINTER = 0x00000001,
            TOUCH = 0x00000002,
            PEN = 0x00000003,
            MOUSE = 0x00000004
        }

        [Flags]
        internal enum POINTER_FLAGS
        {
            NONE = 0x00000000,
            NEW = 0x00000001,
            INRANGE = 0x00000002,
            INCONTACT = 0x00000004,
            FIRSTBUTTON = 0x00000010,
            SECONDBUTTON = 0x00000020,
            THIRDBUTTON = 0x00000040,
            FOURTHBUTTON = 0x00000080,
            FIFTHBUTTON = 0x00000100,
            PRIMARY = 0x00002000,
            CONFIDENCE = 0x00004000,
            CANCELED = 0x00008000,
            DOWN = 0x00010000,
            UPDATE = 0x00020000,
            UP = 0x00040000,
            WHEEL = 0x00080000,
            HWHEEL = 0x00100000,
            CAPTURECHANGED = 0x00200000,
        }

        [Flags]
        internal enum VIRTUAL_KEY_STATES
        {
            NONE = 0x0000,
            LBUTTON = 0x0001,
            RBUTTON = 0x0002,
            SHIFT = 0x0004,
            CTRL = 0x0008,
            MBUTTON = 0x0010,
            XBUTTON1 = 0x0020,
            XBUTTON2 = 0x0040
        }

        internal enum POINTER_CHANGE
        {
            NONE,
            FIRSTBUTTON_DOWN,
            FIRSTBUTTON_UP,
            SECONDBUTTON_DOWN,
            SECONDBUTTON_UP,
            THIRDBUTTON_DOWN,
            THIRDBUTTON_UP,
            FOURTHBUTTON_DOWN,
            FOURTHBUTTON_UP,
            FIFTHBUTTON_DOWN,
            FIFTHBUTTON_UP
        }

        [Flags]
        internal enum TOUCH_FLAGS
        {
            NONE = 0x00000000
        }

        [Flags]
        internal enum TOUCH_MASK
        {
            NONE = 0x00000000,
            CONTACTAREA = 0x00000001,
            ORIENTATION = 0x00000002,
            PRESSURE = 0x00000004,
        }

        [Flags]
        internal enum PEN_FLAGS
        {
            NONE = 0x00000000,
            BARREL = 0x00000001,
            INVERTED = 0x00000002,
            ERASER = 0x00000004,
        }

        [Flags]
        internal enum PEN_MASK
        {
            NONE = 0x00000000,
            PRESSURE = 0x00000001,
            ROTATION = 0x00000002,
            TILT_X = 0x00000004,
            TILT_Y = 0x00000008,
        }

        [Flags]
        internal enum POINTER_MESSAGE_FLAG
        {
            NEW = 0x00000001,
            INRANGE = 0x00000002,
            INCONTACT = 0x00000004,
            FIRSTBUTTON = 0x00000010,
            SECONDBUTTON = 0x00000020,
            THIRDBUTTON = 0x00000040,
            FOURTHBUTTON = 0x00000080,
            FIFTHBUTTON = 0x00000100,
            PRIMARY = 0x00002000,
            CONFIDENCE = 0x00004000,
            CANCELED = 0x00008000,
        }

        internal enum TOUCH_FEEDBACK
        {
            DEFAULT = 0x1,
            INDIRECT = 0x2,
            NONE = 0x3
        }

        #endregion

        //------------------------------------------------------------------
        #region ** interaction context constants

        internal enum INTERACTION
        {
            NONE = 0x00000000,
            MANIPULATION = 0x00000001,
            TAP = 0x00000002,
            SECONDARY_TAP = 0x00000003,
            HOLD = 0x00000004,
            DRAG = 0x00000005,
            CROSS_SLIDE = 0x00000006
        }

        [Flags]
        internal enum INTERACTION_FLAGS
        {
            NONE = 0x00000000,
            BEGIN = 0x00000001,
            END = 0x00000002,
            CANCEL = 0x00000004,
            INERTIA = 0x00000008
        }

        [Flags]
        internal enum INTERACTION_CONFIGURATION_FLAGS
        {
            NONE = 0x00000000,
            MANIPULATION = 0x00000001,
            MANIPULATION_TRANSLATION_X = 0x00000002,
            MANIPULATION_TRANSLATION_Y = 0x00000004,
            MANIPULATION_ROTATION = 0x00000008,
            MANIPULATION_SCALING = 0x00000010,
            MANIPULATION_TRANSLATION_INERTIA = 0x00000020,
            MANIPULATION_ROTATION_INERTIA = 0x00000040,
            MANIPULATION_SCALING_INERTIA = 0x00000080,
            MANIPULATION_RAILS_X = 0x00000100,
            MANIPULATION_RAILS_Y = 0x00000200,
            MANIPULATION_EXACT = 0x00000400,
            CROSS_SLIDE = 0x00000001,
            CROSS_SLIDE_HORIZONTAL = 0x00000002,
            CROSS_SLIDE_SELECT = 0x00000004,
            CROSS_SLIDE_SPEED_BUMP = 0x00000008,
            CROSS_SLIDE_REARRANGE = 0x00000010,
            CROSS_SLIDE_EXACT = 0x00000020,
            TAP = 0x00000001,
            TAP_DOUBLE = 0x00000002,
            SECONDARY_TAP = 0x00000001,
            HOLD = 0x00000001,
            HOLD_MOUSE = 0x00000002,
            DRAG = 0x00000001
        }

        internal enum INERTIA_PARAMETER
        {
            TRANSLATION_DECELERATION = 0x00000001,
            TRANSLATION_DISPLACEMENT = 0x00000002,
            ROTATION_DECELERATION = 0x00000003,
            ROTATION_ANGLE = 0x00000004,
            EXPANSION_DECELERATION = 0x00000005,
            EXPANSION_EXPANSION = 0x00000006
        }

        internal enum INTERACTION_STATE
        {
            IDLE = 0x00000000,
            IN_INTERACTION = 0x00000001,
            POSSIBLE_DOUBLE_TAP = 0x00000002
        }

        internal enum INTERACTION_CONTEXT_PROPERTY
        {
            MEASUREMENT_UNITS = 0x00000001,
            INTERACTION_UI_FEEDBACK = 0x00000002,
            FILTER_POINTERS = 0x00000003
        }

        internal const int
            ICP_MEASUREMENT_UNITS_HIMETRIC = 0,
            ICP_MEASUREMENT_UNITS_SCREEN = 1,
            ICP_UI_FEEDBACK_OFF = 0,
            ICP_UI_FEEDBACK_ON = 1,
            ICP_FILTER_POINTERS_OFF = 0,
            ICP_FILTER_POINTERS_ON = 1;

        internal enum CROSS_SLIDE_THRESHOLD
        {
            SELECT_START = 0x00000000,
            SPEED_BUMP_START = 0x00000001,
            SPEED_BUMP_END = 0x00000002,
            REARRANGE_START = 0x00000003,
            COUNT = 0x00000004
        }

        [Flags]
        internal enum CROSS_SLIDE_FLAGS
        {
            NONE = 0x00000000,
            SELECT = 0x00000001,
            SPEED_BUMP = 0x00000002,
            REARRANGE = 0x00000004
        }

        internal enum MOUSE_WHEEL_PARAMETER
        {
            CHAR_TRANSLATION_X = 0x00000001,
            CHAR_TRANSLATION_Y = 0x00000002,
            DELTA_SCALE = 0x00000003,
            DELTA_ROTATION = 0x00000004,
            PAGE_TRANSLATION_X = 0x00000005,
            PAGE_TRANSLATION_Y = 0x00000006
        }

        internal enum MANIPULATION_RAILS_STATE
        {
            UNDECIDED = 0x00000000,
            FREE = 0x00000001,
            RAILED = 0x00000002,
        }

        #endregion

        //------------------------------------------------------------------
        #region ** misc structs

        #region CURSORINFO

        [StructLayout(LayoutKind.Sequential)]
        internal struct CURSORINFO
        {
            public int Size;
            public CURSOR_STATE Flags;
            public IntPtr Cursor;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT ScreenPos;
        }

        #endregion

        #region POINT

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
            public POINT(Point pt)
            {
                X = pt.X;
                Y = pt.Y;
            }
            public Point ToPoint()
            {
                return new Point(X, Y);
            }
            public void AssignTo(ref Point destination)
            {
                destination.X = X;
                destination.Y = Y;
            }
            public void CopyFrom(Point source)
            {
                X = source.X;
                Y = source.Y;
            }
            public void CopyFrom(POINT source)
            {
                X = source.X;
                Y = source.Y;
            }
        }

        #endregion

        #region RECT

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(Rectangle source)
            {
                Left = source.Left;
                Top = source.Top;
                Right = source.Right;
                Bottom = source.Bottom;
            }
            public RECT(int x, int y, int width, int height)
            {
                Left = x;
                Top = y;
                Right = Left + width;
                Bottom = Top + height;
            }
            public int Width
            {
                get { return Right - Left; }
            }
            public int Height
            {
                get { return Bottom - Top; }
            }
            public Rectangle ToRectangle()
            {
                return new Rectangle(Left, Top, Width, Height);
            }
            public void Inflate(int dx, int dy)
            {
                Left -= dx;
                Top -= dy;
                Right += dx;
                Bottom += dy;
            }
            public void Deflate(int leftMargin, int topMargin, int rightMargin, int bottomMargin)
            {
                Left += leftMargin;
                Top += topMargin;
                Right -= rightMargin;
                Bottom -= bottomMargin;
                if (Bottom < Top)
                {
                    Bottom = Top;
                }
                if (Right < Left)
                {
                    Right = Left;
                }
            }
            public void Offset(int dx, int dy)
            {
                Left += dx;
                Top += dy;
                Right += dx;
                Bottom += dy;
            }
        }

        #endregion

        #endregion

        //------------------------------------------------------------------
        #region ** pointer structs

        #region POINTER_INFO

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTER_INFO
        {
            public POINTER_INPUT_TYPE pointerType;
            public int PointerID;
            public int FrameID;
            public POINTER_FLAGS PointerFlags;
            public IntPtr SourceDevice;
            public IntPtr WindowTarget;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtPixelLocation;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtPixelLocationRaw;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtHimetricLocation;
            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtHimetricLocationRaw;
            public uint Time;
            public uint HistoryCount;
            public uint InputData;
            public VIRTUAL_KEY_STATES KeyStates;
            public long PerformanceCount;
            public int ButtonChangeType;
        }

        #endregion

        #region POINTER_TOUCH_INFO

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTER_TOUCH_INFO
        {
            [MarshalAs(UnmanagedType.Struct)]
            public POINTER_INFO PointerInfo;
            public TOUCH_FLAGS TouchFlags;
            public TOUCH_MASK TouchMask;
            [MarshalAs(UnmanagedType.Struct)]
            public RECT ContactArea;
            [MarshalAs(UnmanagedType.Struct)]
            public RECT ContactAreaRaw;
            public uint Orientation;
            public uint Pressure;
        }

        #endregion

        #region POINTER_PEN_INFO

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTER_PEN_INFO
        {
            [MarshalAs(UnmanagedType.Struct)]
            public POINTER_INFO PointerInfo;
            public PEN_FLAGS PenFlags;
            public PEN_MASK PenMask;
            public uint Pressure;
            public uint Rotation;
            public int TiltX;
            public int TiltY;
        }

        #endregion

        #endregion

        //------------------------------------------------------------------
        #region ** interaction context structs

        #region MANIPULATION_TRANSFORM

        [StructLayout(LayoutKind.Sequential)]
        internal struct MANIPULATION_TRANSFORM
        {
            public float TranslationX;
            public float TranslationY;
            public float Scale;
            public float Expansion;
            public float Rotation;
        }

        #endregion

        #region MANIPULATION_VELOCITY

        [StructLayout(LayoutKind.Sequential)]
        internal struct MANIPULATION_VELOCITY
        {
            public float VelocityX;
            public float VelocityY;
            public float VelocityExpansion;
            public float VelocityAngular;
        }

        #endregion

        #region INTERACTION_ARGUMENTS_MANIPULATION

        [StructLayout(LayoutKind.Sequential)]
        internal struct INTERACTION_ARGUMENTS_MANIPULATION
        {
            [MarshalAs(UnmanagedType.Struct)]
            public MANIPULATION_TRANSFORM Delta;
            [MarshalAs(UnmanagedType.Struct)]
            public MANIPULATION_TRANSFORM Cumulative;
            [MarshalAs(UnmanagedType.Struct)]
            public MANIPULATION_VELOCITY Velocity;
            public MANIPULATION_RAILS_STATE RailsState;
        }

        #endregion

        #region INTERACTION_ARGUMENTS_TAP

        [StructLayout(LayoutKind.Sequential)]
        internal struct INTERACTION_ARGUMENTS_TAP
        {
            public int Count;
        }

        #endregion

        #region INTERACTION_ARGUMENTS_CROSS_SLIDE

        [StructLayout(LayoutKind.Sequential)]
        internal struct INTERACTION_ARGUMENTS_CROSS_SLIDE
        {
            public CROSS_SLIDE_FLAGS Flags;
        }

        #endregion

        #region INTERACTION_CONTEXT_OUTPUT

        [StructLayout(LayoutKind.Explicit)]
        internal struct INTERACTION_CONTEXT_OUTPUT
        {
            [FieldOffset(0)]
            public INTERACTION Interaction;
            [FieldOffset(4)]
            public INTERACTION_FLAGS InteractionFlags;
            [FieldOffset(8)]
            public POINTER_INPUT_TYPE InputType;
            [FieldOffset(12)]
            public float X;
            [FieldOffset(16)]
            public float Y;
            [FieldOffset(20)]
            [MarshalAs(UnmanagedType.Struct)]
            public INTERACTION_ARGUMENTS_MANIPULATION Manipulation;
            [FieldOffset(20)]
            [MarshalAs(UnmanagedType.Struct)]
            public INTERACTION_ARGUMENTS_TAP Tap;
            [FieldOffset(20)]
            [MarshalAs(UnmanagedType.Struct)]
            public INTERACTION_ARGUMENTS_CROSS_SLIDE CrossSlide;
        }

        #endregion

        #region INTERACTION_CONTEXT_CONFIGURATION

        [StructLayout(LayoutKind.Sequential)]
        internal struct INTERACTION_CONTEXT_CONFIGURATION
        {
            public INTERACTION Interaction;
            public INTERACTION_CONFIGURATION_FLAGS Enable;

            public INTERACTION_CONTEXT_CONFIGURATION(INTERACTION interaction, INTERACTION_CONFIGURATION_FLAGS enable)
            {
                Interaction = interaction;
                Enable = enable;
            }
        }

        #endregion

        #region CROSS_SLIDE_PARAMETER

        [StructLayout(LayoutKind.Sequential)]
        internal struct CROSS_SLIDE_PARAMETER
        {
            public CROSS_SLIDE_THRESHOLD Threshold;
            public float Distance;
        }

        #endregion

        #endregion

        //------------------------------------------------------------------
        #region ** misc methods

        [DllImport("gdi32.dll", ExactSpelling = true, EntryPoint = "CreateICW", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateIC(string lpszDriverName, IntPtr lpszDeviceName, IntPtr lpszOutput, IntPtr lpInitData);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        internal static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        internal static extern int GetDeviceCaps(IntPtr hDC, int index);

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, EntryPoint = "SystemParametersInfoW", CharSet = CharSet.Unicode)]
        internal static extern bool SystemParametersInfo(int uiAction, int uiParam, IntPtr pvParam, int fWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetWindowFeedbackSetting(IntPtr hWnd, FEEDBACK_TYPE feedback, int dwFlags, ref int pSize, out int config);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowFeedbackSetting(IntPtr hWnd, FEEDBACK_TYPE feedback, int dwFlags, int size, ref int config);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowFeedbackSetting(IntPtr hWnd, FEEDBACK_TYPE feedback, int dwFlags, int size, IntPtr config);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetCursorInfo(ref CURSORINFO ci);

        [DllImport("uxtheme.dll")]
        internal static extern bool BeginPanningFeedback(IntPtr hWnd);

        [DllImport("uxtheme.dll")]
        internal static extern bool UpdatePanningFeedback(IntPtr hWnd, int lTotalOverpanOffsetX, int lTotalOverpanOffsetY, bool fInInertia);

        [DllImport("uxtheme.dll")]
        internal static extern bool EndPanningFeedback(IntPtr hWnd, bool fAnimateBack);

        #endregion

        //------------------------------------------------------------------
        #region ** pointer methods

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool InitializeTouchInjection(int maxCount, TOUCH_FEEDBACK feedbackMode);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool InjectTouchInput(int count, [MarshalAs(UnmanagedType.LPArray), In] POINTER_TOUCH_INFO[] contacts);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerType(int pointerID, out POINTER_INPUT_TYPE pointerType);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerInfo(int pointerID, ref POINTER_INFO pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerInfoHistory(int pointerID, ref int entriesCount, [MarshalAs(UnmanagedType.LPArray), In, Out] POINTER_INFO[] pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerInfoHistory(int pointerID, ref int entriesCount, IntPtr pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFrameInfo(int pointerID, ref int pointerCount, [MarshalAs(UnmanagedType.LPArray), In, Out] POINTER_INFO[] pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFrameInfo(int pointerID, ref int pointerCount, IntPtr pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFrameInfoHistory(int pointerID, ref int entriesCount, ref int pointerCount,
            [MarshalAs(UnmanagedType.LPArray), In, Out] POINTER_INFO[] pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFrameInfoHistory(int pointerID, ref int entriesCount, ref int pointerCount, IntPtr pointerInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerTouchInfo(int pointerID, ref POINTER_TOUCH_INFO touchInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFrameTouchInfo(int pointerID, ref int pointerCount, [MarshalAs(UnmanagedType.LPArray), In, Out] POINTER_TOUCH_INFO[] touchInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFrameTouchInfo(int pointerID, ref int pointerCount, IntPtr touchInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerPenInfo(int pointerID, ref POINTER_PEN_INFO penInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFramePenInfo(int pointerID, ref int pointerCount, [MarshalAs(UnmanagedType.LPArray), In, Out] POINTER_PEN_INFO[] penInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetPointerFramePenInfo(int pointerID, ref int pointerCount, IntPtr penInfo);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SkipPointerFrameMessages(int pointerID);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool EnableMouseInPointer([MarshalAs(UnmanagedType.Bool), In] bool enable);

        [DllImport("user32.dll")]
        internal static extern bool IsMouseInPointerEnabled();

        #endregion

        //------------------------------------------------------------------
        #region ** interaction context methods

        [DllImport("ninput.dll", PreserveSig = false)]
        static extern void CreateInteractionContext(out IntPtr interactionContext);

        [DllImport("ninput.dll", PreserveSig = false)]
        static extern void DestroyInteractionContext(IntPtr interactionContext);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void INTERACTION_CONTEXT_OUTPUT_CALLBACK(IntPtr clientData, IntPtr output);

        [DllImport("ninput.dll", PreserveSig = false)]
        static extern void RegisterOutputCallbackInteractionContext(IntPtr interactionContext, IntPtr callbackFunc, IntPtr clientData);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void SetInteractionConfigurationInteractionContext(IntPtr interactionContext,
            int configurationCount, [MarshalAs(UnmanagedType.LPArray), In] INTERACTION_CONTEXT_CONFIGURATION[] configuration);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void GetInteractionConfigurationInteractionContext(IntPtr interactionContext,
            int configurationCount, [MarshalAs(UnmanagedType.LPArray), In, Out] INTERACTION_CONTEXT_CONFIGURATION[] configuration);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void SetPropertyInteractionContext(IntPtr interactionContext,
            INTERACTION_CONTEXT_PROPERTY contextProperty, int value);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void GetPropertyInteractionContext(IntPtr interactionContext,
            INTERACTION_CONTEXT_PROPERTY contextProperty, out int value);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void SetInertiaParameterInteractionContext(IntPtr interactionContext,
            INERTIA_PARAMETER inertiaParameter, float value);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void GetInertiaParameterInteractionContext(IntPtr interactionContext,
            INERTIA_PARAMETER inertiaParameter, out float value);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void SetCrossSlideParametersInteractionContext(IntPtr interactionContext,
            int parameterCount, [MarshalAs(UnmanagedType.LPArray), In] CROSS_SLIDE_PARAMETER[] crossSlideParameters);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void GetCrossSlideParameterInteractionContext(IntPtr interactionContext,
            CROSS_SLIDE_THRESHOLD threshold, out float distance);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void SetMouseWheelParameterInteractionContext(IntPtr interactionContext,
            MOUSE_WHEEL_PARAMETER parameter, float value);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void GetMouseWheelParameterInteractionContext(IntPtr interactionContext,
            MOUSE_WHEEL_PARAMETER parameter, out float value);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void ResetInteractionContext(IntPtr interactionContext);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void GetStateInteractionContext(IntPtr interactionContext, IntPtr pointerInfo, out INTERACTION_STATE state);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void AddPointerInteractionContext(IntPtr interactionContext, UInt32 pointerId);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void RemovePointerInteractionContext(IntPtr interactionContext, int pointerId);

        [DllImport("ninput.dll")]
        internal static extern IntPtr ProcessPointerFramesInteractionContext(IntPtr interactionContext,
            UInt32 entriesCount, UInt32 pointerCount, [MarshalAs(UnmanagedType.LPArray), In] POINTER_INFO[] pointerInfo);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void BufferPointerPacketsInteractionContext(IntPtr interactionContext,
            int entriesCount, [MarshalAs(UnmanagedType.LPArray), In] POINTER_INFO[] pointerInfo);

        [DllImport("ninput.dll")]
        internal static extern IntPtr ProcessBufferedPacketsInteractionContext(IntPtr interactionContext);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void ProcessInertiaInteractionContext(IntPtr interactionContext);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void StopInteractionContext(IntPtr interactionContext);

        [DllImport("ninput.dll", PreserveSig = false)]
        internal static extern void SetPivotInteractionContext(IntPtr interactionContext, float x, float y, float radius);

        #endregion
    }
}
