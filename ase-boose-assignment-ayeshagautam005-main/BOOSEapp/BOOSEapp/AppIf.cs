using System;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that handles parsing and reporting of IF statement constructs.
    /// </summary>
    public class AppIf : ICommand
    {
        private readonly string commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppIf"/> class with the specified IF statement text.
        /// </summary>
        /// <param name="commandLine">The textual representation of the IF statement command.</param>
        public AppIf(string commandLine)
        {
            this.commandLine = commandLine;
        }

        /// <summary>
        /// Executes the IF command by reporting the detected IF statement to the main form output.
        /// </summary>
        /// <param name="canvas">The canvas associated with the current execution context. This parameter is not used.</param>
        public void Execute(ICanvas canvas)
        {
            MainForm mainForm = Application.OpenForms["MainForm"] as MainForm;
            if (mainForm != null)
            {
                mainForm.AppendOutput($"[IF] Detected IF statement: {commandLine}\r\n");
            }
        }

        /// <summary>
        /// Returns a string that represents the current IF command.
        /// </summary>
        /// <returns>A string that describes the IF command, including its original text.</returns>
        public override string ToString()
        {
            return $"IF({commandLine})";
        }
    }
}
