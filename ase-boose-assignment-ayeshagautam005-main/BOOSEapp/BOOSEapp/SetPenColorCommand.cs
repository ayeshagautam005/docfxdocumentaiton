using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a command that sets the current pen color on the canvas using RGB components.
    /// </summary>
    public class SetPenColorCommand : ICommand
    {
        private readonly int r;
        private readonly int g;
        private readonly int b;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetPenColorCommand"/> class with the specified RGB components.
        /// </summary>
        /// <param name="r">The red component of the color (0–255).</param>
        /// <param name="g">The green component of the color (0–255).</param>
        /// <param name="b">The blue component of the color (0–255).</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="r"/>, <paramref name="g"/>, or <paramref name="b"/> is outside the range 0–255.
        /// </exception>
        public SetPenColorCommand(int r, int g, int b)
        {
            if (r < 0 || r > 255) throw new ArgumentException("Red must be 0-255", nameof(r));
            if (g < 0 || g > 255) throw new ArgumentException("Green must be 0-255", nameof(g));
            if (b < 0 || b > 255) throw new ArgumentException("Blue must be 0-255", nameof(b));

            this.r = r;
            this.g = g;
            this.b = b;
        }

        /// <summary>
        /// Executes the command by setting the pen color of the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to set the pen color.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="canvas"/> is <see langword="null"/>.
        /// </exception>
        public void Execute(ICanvas canvas)
        {
            if (canvas == null)
                throw new ArgumentNullException(nameof(canvas));

            canvas.SetPenColor(r, g, b);
            Console.WriteLine($"SetPenColor: Changed pen to RGB({r}, {g}, {b})");
        }

        /// <summary>
        /// Returns a string that represents the current pen color command.
        /// </summary>
        /// <returns>A string that describes the RGB components of the pen color.</returns>
        public override string ToString()
        {
            return $"SetPenColor(R={r}, G={g}, B={b})";
        }
    }
}
