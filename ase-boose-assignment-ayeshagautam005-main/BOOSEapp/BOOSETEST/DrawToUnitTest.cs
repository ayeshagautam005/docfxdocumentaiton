using Microsoft.VisualStudio.TestTools.UnitTesting;
using BOOSEapp;

namespace BOOSETEST
{
    /// <summary>
    /// Contains unit tests for the <see cref="AppCanvas.DrawTo(int, int)"/> method.
    /// </summary>
    [TestClass]
    public class DrawToUnitTest
    {
        /// <summary>
        /// Verifies that calling <see cref="AppCanvas.DrawTo(int, int)"/> with valid coordinates
        /// moves the pen position on the canvas to the specified destination.
        /// </summary>
        [TestMethod]
        public void DrawTo_ValidCoordinates_MovesPen()
        {
            using var canvas = new AppCanvas(800, 600);
            canvas.MoveTo(100, 100);

            canvas.DrawTo(300, 300);

            Assert.AreEqual(300, canvas.Xpos);
            Assert.AreEqual(300, canvas.Ypos);
        }
    }
}
