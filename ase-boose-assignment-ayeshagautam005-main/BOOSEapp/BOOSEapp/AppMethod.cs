using System;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that handles parsing and reporting of method definitions.
    /// </summary>
    public class AppMethod : ICommand
    {
        private readonly string commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppMethod"/> class with the specified method definition text.
        /// </summary>
        /// <param name="commandLine">The textual representation of the method definition command.</param>
        public AppMethod(string commandLine)
        {
            this.commandLine = commandLine;
        }

        /// <summary>
        /// Executes the method command by reporting the detected method definition to the main form output.
        /// </summary>
        /// <param name="canvas">The canvas associated with the current execution context. This parameter is not used.</param>
        public void Execute(ICanvas canvas)
        {
            MainForm mainForm = Application.OpenForms["MainForm"] as MainForm;
            if (mainForm != null)
            {
                mainForm.AppendOutput($"[METHOD] Processing method definition: {commandLine}\r\n");
            }
        }

        /// <summary>
        /// Returns a string that represents the current method command.
        /// </summary>
        /// <returns>A string that describes the method command, including its original definition text.</returns>
        public override string ToString()
        {
            return $"Method({commandLine})";
        }
    }
}
