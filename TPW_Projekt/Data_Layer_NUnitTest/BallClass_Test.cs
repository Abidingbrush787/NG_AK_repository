using Data_Layer;
using System.Windows.Media;

namespace Data_Layer_NUnitTest
{
    public class Ball_Constructor_Tests
    {
        //Setup parameters
        
            static double x = 10;
            static double y = 20;
            static double velocityX = 1;
            static double velocityY = 1;
            static double radius = 10;
            static Color color = ColorsDefinitions.Blue;

            Ball ball = new Ball(x, y, velocityX, velocityY, radius, color);
        

        [Test]
        public void Check_Values()
        {
            Assert.That(ball.X, Is.EqualTo(x));
            Assert.That(ball.Y, Is.EqualTo(y));
            Assert.That(ball.VelocityX, Is.EqualTo(velocityX));
            Assert.That(ball.VelocityY, Is.EqualTo(velocityY));
            Assert.That(ball.Radius, Is.EqualTo(radius));
            Assert.That(ball.Color, Is.EqualTo(color));

        }
    }
}