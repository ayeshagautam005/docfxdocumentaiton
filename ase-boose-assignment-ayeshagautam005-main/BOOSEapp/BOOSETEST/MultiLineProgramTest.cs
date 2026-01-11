using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSEapp;

namespace BOOSETEST
{
    /// <summary>
    /// Contains tests that simulate multi-step drawing sequences on the canvas.
    /// </summary>
    [TestClass]
    public class MultiLineProgramTest
    {
        /// <summary>
        /// Verifies that executing a sequence of MoveTo and DrawTo calls
        /// correctly updates the final pen position on the canvas.
        /// </summary>
        [TestMethod]
        public void MultiLine_ValidProgram_Works()
        {
            // Arrange - create canvas for testing
            using var canvas = new AppCanvas(800, 600);

            // Act - execute sequence of drawing commands
            // Move to starting position
            canvas.MoveTo(100, 100);

            // Draw horizontal line
            canvas.DrawTo(200, 100);

            // Draw vertical line
            canvas.DrawTo(200, 200);

            // Assert - check final pen position
            Assert.AreEqual(200, canvas.Xpos);
            Assert.AreEqual(200, canvas.Ypos);
        }
    }
}
