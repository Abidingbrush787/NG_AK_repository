using Data_Layer;
using Logic_Layer;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Logic_Layer_Tests
{
    [TestFixture]
    public class BallServiceTests
    {
        private BallService _ballService;
        private BallRepository _ballRepository;
        private Boundary _boundary;
        private Logger _logger;

        [SetUp]
        public void Setup()
        {
            _boundary = new Boundary(0, 0, 1000, 1000, Colors.Red);
            _ballRepository = new BallRepository();
            _logger = new Logger();
            _ballService = new BallService(_boundary, _ballRepository, _logger);
        }

        [Test]
        public void CreateBall_Test()
        {
            var ball = _ballService.CreateBall();

            Assert.That(ball, Is.Not.Null);
            Assert.That(ball.X, Is.InRange(_boundary.X, _boundary.X + _boundary.Width));
            Assert.That(ball.Y, Is.InRange(_boundary.Y, _boundary.Y + _boundary.Height));

            // Check Velocity
            Assert.That(ball.VelocityX, Is.InRange(-1 / ball.Mass, 1 / ball.Mass));
            Assert.That(ball.VelocityY, Is.InRange(-1 / ball.Mass, 1 / ball.Mass));

            Assert.That(_ballService.D_colors, Contains.Item(ball.Color));
        }

        [Test]
        public async Task BallMovement_Test()
        {
            var ball = _ballService.CreateBall();
            var initialX = ball.X;
            var initialY = ball.Y;

            await _ballService.UpdateBallPositions(1);

            // SprawdŸ, czy kula siê przemieœci³a
            Assert.That(ball.X, Is.Not.EqualTo(initialX));
            Assert.That(ball.Y, Is.Not.EqualTo(initialY));
        }

        [Test]
        public async Task BallCollision_Test()
        {
            // Utwórz dwie kule
            var ball1 = _ballService.CreateBall();
            var ball2 = _ballService.CreateBall();

            // Ustaw kule na tych samych pozycjach, symuluj¹c kolizjê
            ball2.X = ball1.X;
            ball2.Y = ball1.Y;

            // Aktualizuj pozycje kul
            await _ballService.UpdateBallPositions(1);

            // SprawdŸ, czy kule siê odbi³y (czyli czy ich prêdkoœci zmieni³y znak)
            Assert.That(ball1.VelocityX, Is.Not.EqualTo(ball2.VelocityX));
            Assert.That(ball1.VelocityY, Is.Not.EqualTo(ball2.VelocityY));
        }

        [Test]
        public void Check_Ball_List()
        {
            var ball = _ballService.CreateBall();
            var ballList = _ballRepository.GetAllBalls();

            Assert.That(ballList.Count, Is.GreaterThan(0));
        }
    }
}
