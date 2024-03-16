using Microsoft.VisualStudio.TestTools.UnitTesting;
using Point;

namespace Point2DTests
{
    [TestClass]
    public class Point2DTest
    {
        [TestMethod]
        public void Constructor_SetsXAndY()
        {
            // Arrange
            var x = 10.0;
            var y = 20.0;

            // Act
            var point = new Point2D(x, y);

            // Assert
            Assert.AreEqual(x, point.X, "X coordinate is not set correctly.");
            Assert.AreEqual(y, point.Y, "Y coordinate is not set correctly.");
        }

        [TestMethod]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var point = new Point2D(1, 2);

            // Act
            var result = point.ToString();

            // Assert
            var expected = "Point2D(1, 2)";
            Assert.AreEqual(expected, result, "ToString does not return the expected format.");
        }
    }
}
