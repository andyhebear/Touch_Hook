using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main2()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            OperatingSystem os = Environment.OSVersion;
            if (os.Platform != PlatformID.Win32NT || os.Version.CompareTo(new Version(6, 2)) < 0)
            {
                MessageBox.Show("This sample requires Windows 8 or better OS.", "Interaction Context Test", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Application.Run(new TouchForm());
        }
    }
}
