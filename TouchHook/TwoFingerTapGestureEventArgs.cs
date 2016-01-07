namespace TouchHook
{
    using System;

    public class TwoFingerTapGestureEventArgs : EventArgs
    {
        public int distance;
        public POINT ptCenter;
        public GestureState state;
    }
}

