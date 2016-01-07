namespace TouchHook
{
    using System;

    public class TouchEventArgs : EventArgs
    {
        public int contactX;
        public int contactY;
        public int flags;
        public int id;
        public IntPtr lparam;
        public int mask;
        public int time;
        public IntPtr wparam;
        public int x;
        public int y;

        public TouchEventArgs()
        {
        }

        public TouchEventArgs(TouchEventArgs tea)
        {
            this.x = tea.x;
            this.y = tea.y;
            this.id = tea.id;
            this.mask = tea.mask;
            this.flags = tea.flags;
            this.time = tea.time;
            this.contactX = tea.contactX;
            this.contactY = tea.contactY;
            this.wparam = tea.wparam;
            this.lparam = tea.lparam;
        }

        public string NiceOutput()
        {
            string str = new string(' ', 1);
            return (((((((((("x=" + this.x.ToString() + "\n") + "y=" + this.y.ToString() + "\n") + "id=" + this.id.ToString() + "\n") + "mask=0x" + this.mask.ToString("X") + "\n") + "flags=0x" + this.flags.ToString("X") + "\n") + "time=" + this.time.ToString() + "\n") + "contactX=" + this.contactX.ToString() + "\n") + "contactY=" + this.contactY.ToString() + "\n") + "wparam=0x" + this.wparam.ToString("X") + "\n") + "lparam=0x" + this.lparam.ToString("X") + "\n");
        }

        public bool IsPrimaryContact
        {
            get
            {
                return ((this.flags & 0x10L) != 0L);
            }
        }
    }
}

