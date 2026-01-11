using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that draws a circle on a canvas.
    /// </summary>
    public class AppCircle : ICommand
    {
        private readonly int radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCircle"/> class with the specified radius.
        /// </summary>
        /// <param name="radius">The radius of the circle to draw.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="radius"/> is less than or equal to zero.</exception>
        public AppCircle(int radius)
        {
            if (radius <= 0)
                throw new ArgumentException("Radius must be positive", nameof(radius));

            this.radius = radius;
        }

        /// <summary>
        /// Executes the command by drawing a circle on the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the circle.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="canvas"/> is <see langword="null"/>.</exception>
        public void Execute(ICanvas canvas)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            canvas.DrawCircle(radius);
            Console.WriteLine($"Circle: Drew circle with radius {radius}");
        }

        /// <summary>
        /// Returns a string that represents the current circle command.
        /// </summary>
        /// <returns>A string that describes the circle command, including its radius.</returns>
        public override string ToString()
        {
            return $"Circle(radius={radius})";
        }
    }
}
