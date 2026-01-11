using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSEapp;

namespace BOOSETEST
{
    /// <summary>
    /// Contains unit tests for the <see cref="AppCanvas.MoveTo(int, int)"/> method.
    /// </summary>
    [TestClass]
    public class MoveToUnitTest
    {
        /// <summary>
        /// Verifies that calling <see cref="AppCanvas.MoveTo(int, int)"/> with valid coordinates
        /// correctly updates the pen position on the canvas.
        /// </summary>
        [TestMethod]
        public void MoveTo_ValidCoordinates_UpdatesPosition()
        {
            // Arrange
            using var canvas = new AppCanvas(800, 600);

            // Act
            canvas.MoveTo(100, 200);

            // Assert
            Assert.AreEqual(100, canvas.Xpos);
            Assert.AreEqual(200, canvas.Ypos);
        }
    }
}
