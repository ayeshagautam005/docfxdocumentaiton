using System;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that declares integer variables in the application context.
    /// </summary>
    public class AppInt : ICommand
    {
        private readonly string commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInt"/> class with the specified declaration text.
        /// </summary>
        /// <param name="commandLine">The textual representation of the integer declaration command.</param>
        public AppInt(string commandLine)
        {
            this.commandLine = commandLine;
        }

        /// <summary>
        /// Executes the integer declaration by forwarding the command text to the main form for processing.
        /// </summary>
        /// <param name="canvas">The canvas associated with the current execution context. This parameter is not used.</param>
        public void Execute(ICanvas canvas)
        {
            if (string.IsNullOrEmpty(commandLine))
                return;

            MainForm mainForm = Application.OpenForms["MainForm"] as MainForm;
            if (mainForm == null)
                return;

            mainForm.ProcessIntDeclaration(commandLine);
        }

        /// <summary>
        /// Returns a string that represents the current integer declaration command.
        /// </summary>
        /// <returns>A string that describes the integer declaration command, including its original text.</returns>
        public override string ToString()
        {
            return $"AppInt('{commandLine}')";
        }
    }
}
