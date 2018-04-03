using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace MouseKeyboardLibrary
{

    /// <summary>
    /// Standard Keyboard Shortcuts used by most applications
    /// </summary>
    public enum StandardShortcut
    {
        Copy,
        Cut,
        Paste,
        SelectAll,
        Save,
        Open,
        New,
        Close,
        Print
    }

    /// <summary>
    /// Simulate keyboard key presses
    /// </summary>
    public static class KeyboardSimulator
    {

        #region Windows API Code

        const int KEYEVENTF_EXTENDEDKEY = 0x1;
        const int KEYEVENTF_KEYUP = 0x2;

        [DllImport("user32.dll")]
        static extern void keybd_event(byte key, byte scan, int flags, int extraInfo);

        #endregion

        #region Methods

        public static void KeyDown(Keys key)
        {
            keybd_event(ParseKey(key), 0, 0, 0);
        }

        public static void KeyUp(Keys key)
        {
            keybd_event(ParseKey(key), 0, KEYEVENTF_KEYUP, 0);
        }

        public static void KeyPress(Keys key)
        {
            KeyDown(key);
            Thread.Sleep(100);
            KeyUp(key);
        }

        /// <summary>
        /// 按鍵 						按鍵碼
        /// 退格鍵						{BACKSPACE}、{BS} 或 {BKSP}
        /// BREAK						{BREAK}
        /// CAPS LOCK					{CAPSLOCK}
        /// DEL 或 DELETE				DEL 或 DELETE
        /// 向下鍵						{DOWN}
        /// END							{END}
        /// ENTER						{ENTER}or ~
        /// ESC							{ESC}
        /// HELP						{HELP}
        /// HOME						{HOME}
        /// INS 或 INSERT				INS 或 INSERT
        /// 向左鍵						{LEFT}
        /// NUM LOCK					{NUMLOCK}
        /// PAGE DOWN					{PGDN}
        /// PAGE UP						{PGUP}
        /// PRINT SCREEN				{PRTSC} (保留供日後使用)
        /// 向右鍵						{RIGHT}
        /// SCROLL LOCK					{SCROLLLOCK}
        /// TAB							{TAB}
        /// 向上鍵						{UP}
        /// F1							{F1}
        /// F2							{F2}
        /// F3							{F3}
        /// F4							{F4}
        /// F5							{F5}
        /// F6							{F6}
        /// F7							{F7}
        /// F8							{F8}
        /// F9							{F9}
        /// F10							{F10}
        /// F11							{F11}
        /// F12							{F12}
        /// F13							{F13}
        /// F14							{F14}
        /// F15							{F15}
        /// F16							{F16}
        /// 數字鍵台加號				{ADD}
        /// 數字鍵台減號				{SUBTRACT}
        /// 數字鍵台乘號				{MULTIPLY}
        /// 數字鍵台除號				{DIVIDE}
        /// 
        /// 若要指定 SHIFT、CTRL 和 ALT 鍵任意組合的按鍵，請在按鍵碼之前使用一或多個下列的程式碼：
        /// 按鍵						按鍵碼
        /// SHIFT						+
        /// CTRL						^
        /// ALT							%
        /// </summary>
        /// <param name="keyCode"></param>
        public static void SendKey(string keyCode)
        {
            SendKeys.SendWait(keyCode);
        }

        public static void SimulateStandardShortcut(StandardShortcut shortcut)
        {
            switch (shortcut)
            {
                case StandardShortcut.Copy:
                    KeyDown(Keys.Control);
                    KeyPress(Keys.C);
                    KeyUp(Keys.Control);
                    break;
                case StandardShortcut.Cut:
                    KeyDown(Keys.Control);
                    KeyPress(Keys.X);
                    KeyUp(Keys.Control);
                    break;
                case StandardShortcut.Paste:
                    KeyDown(Keys.Control);
                    KeyPress(Keys.V);
                    KeyUp(Keys.Control);
                    break;
                case StandardShortcut.SelectAll:
                    KeyDown(Keys.Control);
                    KeyPress(Keys.A);
                    KeyUp(Keys.Control);
                    break;
                case StandardShortcut.Save:
                    KeyDown(Keys.Control);
                    KeyPress(Keys.S);
                    KeyUp(Keys.Control);
                    break;
                case StandardShortcut.Open:
                    KeyDown(Keys.Control);
                    KeyPress(Keys.O);
                    KeyUp(Keys.Control);
                    break;
                case StandardShortcut.New:
                    KeyDown(Keys.Control);
                    KeyPress(Keys.N);
                    KeyUp(Keys.Control);
                    break;
                case StandardShortcut.Close:
                    KeyDown(Keys.Alt);
                    KeyPress(Keys.F4);
                    KeyUp(Keys.Alt);
                    break;
                case StandardShortcut.Print:
                    KeyDown(Keys.Control);
                    KeyPress(Keys.P);
                    KeyUp(Keys.Control);
                    break;
            }
        }

        static byte ParseKey(Keys key)
        {

            // Alt, Shift, and Control need to be changed for API function to work with them
            switch (key)
            {
                case Keys.Alt:
                    return (byte)18;
                case Keys.Control:
                    return (byte)17;
                case Keys.Shift:
                    return (byte)16;
                default:
                    return (byte)key;
            }

        } 

        #endregion

    }

}
