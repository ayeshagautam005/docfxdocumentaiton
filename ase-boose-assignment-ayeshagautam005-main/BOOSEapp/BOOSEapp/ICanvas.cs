namespace BOOSEapp
{
    /// <summary>
    /// Defines a contract for drawing operations on a canvas.
    /// </summary>
    public interface ICanvas
    {
        /// <summary>
        /// Gets the current X-coordinate of the pen.
        /// </summary>
        int Xpos { get; }

        /// <summary>
        /// Gets the current Y-coordinate of the pen.
        /// </summary>
        int Ypos { get; }

        /// <summary>
        /// Moves the pen to the specified coordinates without drawing.
        /// </summary>
        /// <param name="x">The X-coordinate to move the pen to.</param>
        /// <param name="y">The Y-coordinate to move the pen to.</param>
        void MoveTo(int x, int y);

        /// <summary>
        /// Draws a line from the current pen position to the specified coordinates.
        /// </summary>
        /// <param name="x">The destination X-coordinate.</param>
        /// <param name="y">The destination Y-coordinate.</param>
        void DrawTo(int x, int y);

        /// <summary>
        /// Draws a circle centered at the current pen position with the specified radius.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        void DrawCircle(int radius);

        /// <summary>
        /// Draws a rectangle starting at the current pen position with the specified dimensions.
        /// </summary>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        void DrawRectangle(int width, int height);

        /// <summary>
        /// Sets the pen color using RGB values.
        /// </summary>
        /// <param name="r">The red component (0–255).</param>
        /// <param name="g">The green component (0–255).</param>
        /// <param name="b">The blue component (0–255).</param>
        void SetPenColor(int r, int g, int b);

        /// <summary>
        /// Writes the specified text at the current pen position.
        /// </summary>
        /// <param name="text">The text to write.</param>
        void WriteText(string text);

        /// <summary>
        /// Clears the canvas and resets its drawing surface.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets a bitmap snapshot of the current canvas content.
        /// </summary>
        /// <returns>A <see cref="System.Drawing.Bitmap"/> that represents the current canvas.</returns>
        System.Drawing.Bitmap GetBitmap();
    }
}
