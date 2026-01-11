using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that draws a line from the current canvas position to the specified coordinates.
    /// </summary>
    public class AppDrawTo : ICommand
    {
        private readonly int x;
        private readonly int y;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDrawTo"/> class with the specified target coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate to draw to.</param>
        /// <param name="y">The Y-coordinate to draw to.</param>
        public AppDrawTo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Executes the command by drawing a line on the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the line.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="canvas"/> is <see langword="null"/>.
        /// </exception>
        public void Execute(ICanvas canvas)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            canvas.DrawTo(x, y);
            Console.WriteLine($"DrawTo: Drew line to position ({x}, {y})");
        }

        /// <summary>
        /// Returns a string that represents the current draw-to command.
        /// </summary>
        /// <returns>A string that describes the command, including its target coordinates.</returns>
        public override string ToString()
        {
            return $"DrawTo({x}, {y})";
        }
    }
}
