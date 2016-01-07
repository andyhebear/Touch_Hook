namespace TouchHook
{
    using System;

    public class PanGestureEventArgs : EventArgs
    {
        public int distance;
        public POINT ptFirst;
        public POINT ptSecond;
        public GestureState state;
    }
}

