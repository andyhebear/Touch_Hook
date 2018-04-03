using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ICTest;

namespace SDL2
{
    static class Program
    {
        #region Program Entry Point

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Application.Run(new SDL2WindowExample());
            OperatingSystem os = Environment.OSVersion;
            if (os.Platform != PlatformID.Win32NT || os.Version.CompareTo(new Version(6, 2)) < 0) {
                MessageBox.Show("This sample requires Windows 8 or better OS.", "Interaction Context Test", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Application.Run(new TouchForm());
        }

        #endregion
    }
}
