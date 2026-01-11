using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that writes text onto the canvas at the current pen position.
    /// </summary>
    public class WriteTextCommand : ICommand
    {
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteTextCommand"/> class with the specified text.
        /// </summary>
        /// <param name="text">The text to write to the canvas.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        public WriteTextCommand(string text)
        {
            this.text = text ?? throw new ArgumentNullException(nameof(text));
        }

        /// <summary>
        /// Executes the command by writing the configured text onto the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to write the text.</param>
        public void Execute(ICanvas canvas)
        {
            if (canvas is AppCanvas appCanvas)
            {
                appCanvas.WriteText(text);
                Console.WriteLine($"WriteText: Wrote '{text}'");
            }
        }
    }
}
