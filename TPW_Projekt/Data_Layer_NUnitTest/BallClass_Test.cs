using Data_Layer;
using System.Diagnostics;
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
            static double mass = 10;

            Ball ball = new Ball(x, y, velocityX, velocityY, radius, color, mass);
        

        [Test]
        public void Check_Values()
        {
            Assert.That(ball.X, Is.EqualTo(x));
            Assert.That(ball.Y, Is.EqualTo(y));
            Assert.That(ball.VelocityX, Is.EqualTo(velocityX));
            Assert.That(ball.VelocityY, Is.EqualTo(velocityY));
            Assert.That(ball.Radius, Is.EqualTo(radius));
            Assert.That(ball.Color, Is.EqualTo(color));
            Assert.That(ball.Mass, Is.EqualTo(mass));


        }
    }

    [TestFixture]
    public class LoggerTests
    {
        private Logger _logger;
        private string _logFilePath;

        [SetUp]
        public void Setup()
        {
            _logger = new Logger();
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string logDirectory = Path.Combine(projectDirectory, "Logs");
            _logFilePath = Path.Combine(logDirectory, "log.txt");

            Debug.WriteLine($"Path: {_logFilePath}");
        }


        /*
        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }
        }
        */

        [Test]
        public async Task Log_SingleMessage_Test()
        {
            // Arrange
            var message = "Test log message";

            // Act
            _logger.Log(message);

            // Allow some time for the async log to complete
            await Task.Delay(100);

            // Assert
            Assert.IsTrue(File.Exists(_logFilePath));
            var logContent = File.ReadAllText(_logFilePath);
            Assert.That(logContent, Does.Contain(message));
        }

        [Test]
        public async Task LogBalls_Test()
        {
            // Arrange
            var balls = new List<Ball>
        {
            new Ball(0, 0, 1, 1, 10, Colors.Red, 1),
            new Ball(10, 10, -1, -1, 10, Colors.Blue, 2)
        };

            // Act
            _logger.LogBalls(balls);

            // Allow some time for the async log to complete
            await Task.Delay(100);

            // Assert
            Assert.IsTrue(File.Exists(_logFilePath));
            var logContent = File.ReadAllText(_logFilePath);
            foreach (var ball in balls)
            {
                Assert.That(logContent, Does.Contain(ball.X.ToString()));
                Assert.That(logContent, Does.Contain(ball.Y.ToString()));
                Assert.That(logContent, Does.Contain(ball.VelocityX.ToString()));
                Assert.That(logContent, Does.Contain(ball.VelocityY.ToString()));
                Assert.That(logContent, Does.Contain(ball.Radius.ToString()));
                Assert.That(logContent, Does.Contain(ball.Color.ToString()));
                Assert.That(logContent, Does.Contain(ball.Mass.ToString()));
            }
        }

       
    }
}