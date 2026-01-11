using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that clears the drawing canvas.
    /// </summary>
    public class ClearCommand : ICommand
    {
        /// <summary>
        /// Executes the command by clearing the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas to clear.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="canvas"/> is <see langword="null"/>.
        /// </exception>
        public void Execute(ICanvas canvas)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            canvas.Clear();
            Console.WriteLine("Clear: Canvas cleared");
        }
    }
}
