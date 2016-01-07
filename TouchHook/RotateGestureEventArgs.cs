namespace TouchHook
{
    using System;

    public class RotateGestureEventArgs : EventArgs
    {
        public double initialRotation;
        public POINT ptCenter;
        public double rotation;
        public GestureState state;
    }
}

