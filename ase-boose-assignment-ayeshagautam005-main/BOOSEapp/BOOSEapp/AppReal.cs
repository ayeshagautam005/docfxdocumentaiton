using System;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that declares real (floating-point) variables in the application context.
    /// </summary>
    public class AppReal : ICommand
    {
        private readonly string commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppReal"/> class with the specified declaration text.
        /// </summary>
        /// <param name="commandLine">The textual representation of the real number declaration command.</param>
        public AppReal(string commandLine)
        {
            this.commandLine = commandLine;
        }

        /// <summary>
        /// Executes the real number declaration by forwarding the command text to the main form for processing.
        /// </summary>
        /// <param name="canvas">The canvas associated with the current execution context. This parameter is not used.</param>
        public void Execute(ICanvas canvas)
        {
            if (string.IsNullOrEmpty(commandLine))
                return;

            MainForm mainForm = Application.OpenForms["MainForm"] as MainForm;
            if (mainForm == null)
                return;

            mainForm.ProcessRealDeclaration(commandLine);
        }

        /// <summary>
        /// Returns a string that represents the current real number declaration command.
        /// </summary>
        /// <returns>A string that describes the real number declaration command, including its original text.</returns>
        public override string ToString()
        {
            return $"AppReal('{commandLine}')";
        }
    }
}
