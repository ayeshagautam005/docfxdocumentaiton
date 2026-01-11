using BOOSE;
using System;
using System.Drawing;

namespace BOOSEapp
{
    /// <summary>
    /// Represents a drawing surface that implements the <see cref="ICanvas"/> interface.
    /// Provides methods for drawing shapes, lines, and text.
    /// </summary>
    public class AppCanvas : ICanvas, IDisposable
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private Pen currentPen;
        private Brush currentBrush;
        private Font textFont;
        private int xPos;
        private int yPos;
        private readonly int width;
        private readonly int height;

        /// <summary>
        /// Gets the current X-coordinate of the pen.
        /// </summary>
        public int Xpos => xPos;

        /// <summary>
        /// Gets the current Y-coordinate of the pen.
        /// </summary>
        public int Ypos => yPos;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCanvas"/> class with the specified dimensions.
        /// </summary>
        /// <param name="width">The width of the canvas in pixels.</param>
        /// <param name="height">The height of the canvas in pixels.</param>
        /// <exception cref="CanvasException">Thrown when the provided width or height is less than or equal to zero.</exception>
        public AppCanvas(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new CanvasException($"Invalid canvas dimensions: {width}x{height}");

            this.width = width;
            this.height = height;
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            currentPen = new Pen(Color.Black, 2);
            currentBrush = new SolidBrush(Color.Black);
            textFont = new Font("Arial", 12);
            xPos = 0;
            yPos = 0;

            graphics.Clear(Color.White);
        }

        /// <summary>
        /// Moves the pen to the specified coordinates without drawing.
        /// </summary>
        /// <param name="x">The X-coordinate to move the pen to.</param>
        /// <param name="y">The Y-coordinate to move the pen to.</param>
        /// <exception cref="CanvasException">Thrown when the provided coordinates are outside the canvas bounds.</exception>
        public void MoveTo(int x, int y)
        {
            ValidateCoordinates(x, y);
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Draws a straight line from the current position to the specified coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate of the line end point.</param>
        /// <param name="y">The Y-coordinate of the line end point.</param>
        /// <exception cref="CanvasException">Thrown when the coordinates are outside the canvas bounds.</exception>
        public void DrawTo(int x, int y)
        {
            ValidateCoordinates(x, y);
            graphics.DrawLine(currentPen, xPos, yPos, x, y);
            xPos = x;
            yPos = y;
        }

        /// <summary>
        /// Draws a circle centered at the current position with the specified radius.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        /// <exception cref="CanvasException">Thrown when the radius is invalid or the circle exceeds canvas boundaries.</exception>
        public void DrawCircle(int radius)
        {
            if (radius <= 0)
                throw new CanvasException($"Invalid radius: {radius}");

            int x = xPos - radius;
            int y = yPos - radius;
            int diameter = radius * 2;

            ValidateDrawingBounds(x, y, diameter, diameter);
            graphics.DrawEllipse(currentPen, x, y, diameter, diameter);
        }

        /// <summary>
        /// Draws a rectangle starting from the current position with the specified dimensions.
        /// </summary>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <exception cref="CanvasException">Thrown when the specified dimensions are invalid.</exception>
        public void DrawRectangle(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new CanvasException($"Invalid rectangle dimensions: {width}x{height}");

            ValidateDrawingBounds(xPos, yPos, width, height);
            graphics.DrawRectangle(currentPen, xPos, yPos, width, height);
        }

        /// <summary>
        /// Sets the pen and brush color using RGB values.
        /// </summary>
        /// <param name="r">Red component (0–255).</param>
        /// <param name="g">Green component (0–255).</param>
        /// <param name="b">Blue component (0–255).</param>
        /// <exception cref="CanvasException">Thrown when any RGB value is outside the valid range.</exception>
        public void SetPenColor(int r, int g, int b)
        {
            if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
                throw new CanvasException($"Invalid RGB values: ({r},{g},{b})");

            currentPen.Color = Color.FromArgb(r, g, b);
            currentBrush = new SolidBrush(currentPen.Color);
        }

        /// <summary>
        /// Draws the specified text at the current pen position.
        /// </summary>
        /// <param name="text">The text string to draw.</param>
        public void WriteText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            ValidateCoordinates(xPos, yPos);
            graphics.DrawString(text, textFont, currentBrush, xPos, yPos);
        }

        /// <summary>
        /// Clears the canvas and resets the pen position and color to default values.
        /// </summary>
        public void Clear()
        {
            graphics.Clear(Color.White);
            xPos = 0;
            yPos = 0;
            currentPen.Color = Color.Black;
            currentBrush = new SolidBrush(Color.Black);
        }

        /// <summary>
        /// Returns a copy of the current canvas bitmap.
        /// </summary>
        /// <returns>A clone of the current <see cref="Bitmap"/> representing the drawing surface.</returns>
        public Bitmap GetBitmap()
        {
            return (Bitmap)bitmap.Clone();
        }

        /// <summary>
        /// Draws a circle at the current position, optionally filled.
        /// </summary>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="fill">If true, fills the circle with the current brush color.</param>
        public void Circle(int radius, bool fill = false)
        {
            if (radius <= 0)
                throw new CanvasException($"Invalid radius: {radius}");

            int x = xPos - radius;
            int y = yPos - radius;
            int diameter = radius * 2;

            ValidateDrawingBounds(x, y, diameter, diameter);

            if (fill)
                graphics.FillEllipse(currentBrush, x, y, diameter, diameter);
            else
                graphics.DrawEllipse(currentPen, x, y, diameter, diameter);
        }

        /// <summary>
        /// Draws a rectangle at the current position, optionally filled.
        /// </summary>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="fill">If true, fills the rectangle with the current brush color.</param>
        public void Rect(int width, int height, bool fill = false)
        {
            if (width <= 0 || height <= 0)
                throw new CanvasException($"Invalid rectangle dimensions: {width}x{height}");

            ValidateDrawingBounds(xPos, yPos, width, height);

            if (fill)
                graphics.FillRectangle(currentBrush, xPos, yPos, width, height);
            else
                graphics.DrawRectangle(currentPen, xPos, yPos, width, height);
        }

        /// <summary>
        /// Sets the pen color using RGB values (alias for <see cref="SetPenColor"/>).
        /// </summary>
        /// <param name="r">Red component (0–255).</param>
        /// <param name="g">Green component (0–255).</param>
        /// <param name="b">Blue component (0–255).</param>
        public void SetColour(int r, int g, int b) => SetPenColor(r, g, b);

        /// <summary>
        /// Resets the pen position to the origin (0,0).
        /// </summary>
        public void Reset()
        {
            xPos = 0;
            yPos = 0;
        }

        /// <summary>
        /// Returns a copy of the current bitmap (lowercase alias for <see cref="GetBitmap"/>).
        /// </summary>
        /// <returns>A clone of the current <see cref="Bitmap"/>.</returns>
        public Bitmap getBitmap() => GetBitmap();

        /// <summary>
        /// Validates that the provided coordinates are within the canvas boundaries.
        /// </summary>
        /// <param name="x">The X-coordinate to validate.</param>
        /// <param name="y">The Y-coordinate to validate.</param>
        /// <exception cref="CanvasException">Thrown when coordinates are outside the canvas bounds.</exception>
        private void ValidateCoordinates(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                throw new CanvasException($"Coordinates ({x},{y}) are out of bounds");
        }

        /// <summary>
        /// Ensures that the drawing operation fits within the canvas bounds, adjusting as needed.
        /// </summary>
        /// <param name="x">The starting X-coordinate of the drawing area.</param>
        /// <param name="y">The starting Y-coordinate of the drawing area.</param>
        /// <param name="width">The width of the drawing area.</param>
        /// <param name="height">The height of the drawing area.</param>
        private void ValidateDrawingBounds(int x, int y, int width, int height)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x + width > this.width) width = this.width - x;
            if (y + height > this.height) height = this.height - y;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AppCanvas"/> instance.
        /// </summary>
        public void Dispose()
        {
            currentPen?.Dispose();
            currentBrush?.Dispose();
            textFont?.Dispose();
            graphics?.Dispose();
            bitmap?.Dispose();
        }
    }
}
