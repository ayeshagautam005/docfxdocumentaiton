using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that resets the canvas state, including the pen position.
    /// </summary>
    public class ResetCommand : ICommand
    {
        /// <summary>
        /// Executes the command by resetting the specified canvas pen position to the origin.
        /// </summary>
        /// <param name="canvas">The canvas to reset.</param>
        public void Execute(ICanvas canvas)
        {
            if (canvas is AppCanvas appCanvas)
            {
                appCanvas.Reset();
                Console.WriteLine("Reset: Pen position reset to (0,0)");
            }
        }
    }
}
