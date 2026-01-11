using System;
using System.Windows.Forms;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that handles array creation requests,
    /// such as <c>array int scores 10</c>, within the BOOSE application.
    /// </summary>
    public class AppArray : ICommand
    {
        /// <summary>
        /// The raw command line text used to define the array.
        /// </summary>
        private readonly string commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppArray"/> class
        /// using the specified array declaration command line.
        /// </summary>
        /// <param name="commandLine">
        /// The user input that describes the array declaration
        /// (for example, <c>array int scores 10</c>).
        /// </param>
        public AppArray(string commandLine)
        {
            this.commandLine = commandLine;
        }

        /// <summary>
        /// Executes the array declaration command by delegating
        /// the processing to the main application form.
        /// </summary>
        /// <param name="canvas">
        /// The drawing canvas associated with the command execution.
        /// This parameter is not used by <see cref="AppArray"/>,
        /// but is required by the <see cref="ICommand"/> interface.
        /// </param>
        public void Execute(ICanvas canvas)
        {
            MainForm mainForm = Application.OpenForms["MainForm"] as MainForm;
            if (mainForm != null)
            {
                mainForm.ProcessArrayDeclaration(commandLine);
            }
        }

        /// <summary>
        /// Returns a human-readable description of this array
        /// declaration command.
        /// </summary>
        /// <returns>
        /// A string that describes the array declaration, including
        /// the original command line text.
        /// </returns>
        public override string ToString()
        {
            return $"Array declaration: {commandLine}";
        }
    }
}
