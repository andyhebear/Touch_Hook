namespace TouchHook
{
    using System;

    public class ZoomGestureEventArgs : EventArgs
    {
        public int distance;
        public POINT ptFirst;
        public POINT ptSecond;
        public GestureState state;
        public double zoom;
    }
}

