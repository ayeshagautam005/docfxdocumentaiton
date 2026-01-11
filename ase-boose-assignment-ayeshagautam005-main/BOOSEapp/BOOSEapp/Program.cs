using BOOSEapp;
using System;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// The main entry point for the BOOSE Drawing Application.
    /// This class contains the static Main method that initializes and runs the Windows Forms application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// This method performs the following operations:
        /// 1. Enables visual styles for Windows Forms controls.
        /// 2. Sets the default text rendering compatibility mode.
        /// 3. Creates and runs the main application form.
        /// </summary>
        /// <remarks>
        /// The <see cref="STAThreadAttribute"/> is required for Windows Forms applications
        /// that use components like the Clipboard or drag-and-drop functionality.
        /// </remarks>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}