using System;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that handles parsing and reporting of FOR loop constructs.
    /// </summary>
    public class AppFor : ICommand
    {
        private readonly string commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFor"/> class with the specified FOR loop text.
        /// </summary>
        /// <param name="commandLine">The textual representation of the FOR loop command.</param>
        public AppFor(string commandLine)
        {
            this.commandLine = commandLine;
        }

        /// <summary>
        /// Executes the FOR command by reporting the detected FOR loop to the main form output.
        /// </summary>
        /// <param name="canvas">The canvas associated with the current execution context. This parameter is not used.</param>
        public void Execute(ICanvas canvas)
        {
            MainForm mainForm = Application.OpenForms["MainForm"] as MainForm;
            if (mainForm != null)
            {
                mainForm.AppendOutput($"[FOR] Detected FOR loop: {commandLine}\r\n");
            }
        }

        /// <summary>
        /// Returns a string that represents the current FOR command.
        /// </summary>
        /// <returns>A string that describes the FOR command, including its original text.</returns>
        public override string ToString()
        {
            return $"FOR({commandLine})";
        }
    }
}
