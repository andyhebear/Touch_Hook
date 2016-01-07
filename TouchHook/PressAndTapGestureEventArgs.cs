namespace TouchHook
{
    using System;

    public class PressAndTapGestureEventArgs : EventArgs
    {
        public POINT ptDelta;
        public POINT ptFirst;
        public GestureState state;
    }
}

