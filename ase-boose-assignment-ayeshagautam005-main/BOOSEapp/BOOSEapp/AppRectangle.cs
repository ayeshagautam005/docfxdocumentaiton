using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that draws a rectangle on the canvas using the specified dimensions.
    /// </summary>
    public class AppRectangle : ICommand
    {
        private readonly int width;
        private readonly int height;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppRectangle"/> class with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the rectangle to draw.</param>
        /// <param name="height">The height of the rectangle to draw.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="width"/> or <paramref name="height"/> is less than or equal to zero.
        /// </exception>
        public AppRectangle(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentException("Width must be positive", nameof(width));
            if (height <= 0)
                throw new ArgumentException("Height must be positive", nameof(height));

            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Executes the command by drawing a rectangle on the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the rectangle.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="canvas"/> is <see langword="null"/>.
        /// </exception>
        public void Execute(ICanvas canvas)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            canvas.DrawRectangle(width, height);
            Console.WriteLine($"Rectangle: Drew rectangle {width}x{height}");
        }

        /// <summary>
        /// Returns a string that represents the current rectangle command.
        /// </summary>
        /// <returns>A string that describes the rectangle command, including its width and height.</returns>
        public override string ToString()
        {
            return $"Rectangle({width}x{height})";
        }
    }
}
