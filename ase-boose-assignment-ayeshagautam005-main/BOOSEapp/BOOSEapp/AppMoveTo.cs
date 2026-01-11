using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that moves the pen to the specified coordinates on the canvas.
    /// </summary>
    public class AppMoveTo : ICommand
    {
        private readonly int x;
        private readonly int y;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppMoveTo"/> class with the specified target coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate to move the pen to.</param>
        /// <param name="y">The Y-coordinate to move the pen to.</param>
        public AppMoveTo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Executes the command by moving the pen on the specified canvas to the target coordinates.
        /// </summary>
        /// <param name="canvas">The canvas on which to move the pen.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="canvas"/> is <see langword="null"/>.
        /// </exception>
        public void Execute(ICanvas canvas)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            canvas.MoveTo(x, y);
            Console.WriteLine($"MoveTo: Moved to position ({x}, {y})");
        }

        /// <summary>
        /// Returns a string that represents the current move-to command.
        /// </summary>
        /// <returns>A string that describes the move command, including its target coordinates.</returns>
        public override string ToString()
        {
            return $"MoveTo({x}, {y})";
        }
    }
}
