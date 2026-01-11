using System;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that handles parsing and reporting of WHILE loop constructs.
    /// </summary>
    public class AppWhile : ICommand
    {
        private readonly string commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppWhile"/> class with the specified WHILE loop text.
        /// </summary>
        /// <param name="commandLine">The textual representation of the WHILE loop command.</param>
        public AppWhile(string commandLine)
        {
            this.commandLine = commandLine;
        }

        /// <summary>
        /// Executes the WHILE command by reporting the detected WHILE loop to the main form output.
        /// </summary>
        /// <param name="canvas">The canvas associated with the current execution context. This parameter is not used.</param>
        public void Execute(ICanvas canvas)
        {
            MainForm mainForm = Application.OpenForms["MainForm"] as MainForm;
            if (mainForm != null)
            {
                mainForm.AppendOutput($"[WHILE] Detected while loop: {commandLine}\r\n");
            }
        }

        /// <summary>
        /// Returns a string that represents the current WHILE command.
        /// </summary>
        /// <returns>A string that describes the WHILE command, including its original text.</returns>
        public override string ToString()
        {
            return $"While({commandLine})";
        }
    }
}
