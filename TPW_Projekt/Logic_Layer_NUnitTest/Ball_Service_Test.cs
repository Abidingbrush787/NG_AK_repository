using Data_Layer;
using Logic_Layer;
using Logic_Layer.Interfaces;
using NUnit.Framework;
using System.Windows.Media;



namespace Logic_Layer_Tests
{

    [TestFixture]
    public class BallServiceTests
    {
        public Ball_Service _ballService;
        private double _canvasWidth = 800;
        private double _canvasHeight = 600;


        [SetUp]
        public void Setup()
        {
            _ballService = new Ball_Service(_canvasWidth, _canvasHeight);
        }

        [Test]
        public void CreateBall_Test()
        {
            var ball = _ballService.CreateBall();

            Assert.That(ball, Is.Not.Null);
            Assert.That(ball.X, Is.InRange(0, _canvasWidth));
            Assert.That(ball.Y, Is.InRange(0, _canvasHeight));

            //Checke Velocity 

            Assert.That(ball.VelocityX, Is.InRange(-1, 1));
            Assert.That(ball.VelocityX, Is.InRange(-1, 1));

            Assert.That(_ballService.D_colors, Contains.Item(ball.Color));

        }

        [Test]
        public void UpdateBallPositions_CheckPosition()
        {
            var ball = _ballService.CreateBall();
            var initialX = ball.X;
            var initialY = ball.Y;

            _ballService.UpdateBallPositions(1); // Argument to czas, którym mo¿esz manipulowaæ

            Assert.That(ball.X, Is.Not.EqualTo(initialX));
            Assert.That(ball.Y, Is.Not.EqualTo(initialY));
        }

        [Test]
        public void CHeck_Ball_List()
        {
            var ball = _ballService.CreateBall();
            var BallList = _ballService.balls;

            Assert.That(BallList.Count(), Is.GreaterThan(0));
        }
    }
}
