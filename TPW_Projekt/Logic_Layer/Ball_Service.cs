using Data_Layer;
using Logic_Layer.Interfaces;
using System.Windows.Media;

public class BallService : IBallService
{
    private readonly IBallRepository _ballsRepository;
    private readonly Logger _logger;
    private readonly Random _random = new Random();
    private double _canvasWidth;
    private double _canvasHeight;
    private Boundary _boundary;
    private List<Task> _tasks = new List<Task>();
    private DateTime _lastLogTime = DateTime.UtcNow;

    public Color[] D_colors = new Color[]
    {
        Colors.Red,
        Colors.Green,
        Colors.Blue,
        Colors.Yellow,
        Colors.Orange,
        Colors.Purple,
        Colors.Cyan,
        Colors.Magenta
    };

    public BallService(Boundary boundary, IBallRepository ballsRepository, Logger logger)
    {
        _boundary = boundary ?? throw new ArgumentNullException(nameof(boundary));
        _ballsRepository = ballsRepository ?? throw new ArgumentNullException(nameof(ballsRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private int _nextBallId = 0;

    public Ball CreateBall()
    {
        double radius = 10;
        double margin = 20;
        double x = _random.NextDouble() * (_boundary.Width - 2 * radius - 2 * margin) + radius + margin + _boundary.X;
        double y = _random.NextDouble() * (_boundary.Height - 2 * radius - 2 * margin) + radius + margin + _boundary.Y;
        double mass = (_random.NextDouble() * 0.2) + 1;
        double velocityX = (_random.NextDouble() * 2 - 1) / mass;
        double velocityY = (_random.NextDouble() * 2 - 1) / mass;
        Color color = GetRandomColor();

        Ball ball = new Ball(x, y, velocityX, velocityY, radius, color, mass);
        ball.Id = _nextBallId++;
        _ballsRepository.AddBall(ball);
        return ball;
    }

    private bool _isUpdating = false;

    public async Task UpdateBallPositions(double timeFactor)
    {
        _isUpdating = true;
        _tasks.Clear();

        var allBalls = _ballsRepository.GetAllBalls().OrderBy(ball => ball.Id).ToList();

        foreach (var ball in allBalls)
        {
            _tasks.Add(Task.Run(() =>
            {
                ball.X += ball.VelocityX * timeFactor;
                ball.Y += ball.VelocityY * timeFactor;
                CheckCollisionWithBounds(ball);

                foreach (var otherBall in allBalls)
                {
                    if (otherBall != ball && IsCollision(ball, otherBall))
                    {
                        if (ball.Id < otherBall.Id)
                        {
                            lock (ball)
                            {
                                lock (otherBall)
                                {
                                    HandleCollision(ball, otherBall);
                                }
                            }
                        }
                        else
                        {
                            lock (otherBall)
                            {
                                lock (ball)
                                {
                                    HandleCollision(ball, otherBall);
                                }
                            }
                        }
                    }
                }
            }));
        }

        await Task.WhenAll(_tasks);

        // Logowanie stanu kul co okreœlony czas deltaT
        if ((DateTime.UtcNow - _lastLogTime).TotalMilliseconds >= 2000) // deltaT = 1000 ms (1 sekunda)
        {
            _logger.LogBalls(allBalls);
            _lastLogTime = DateTime.UtcNow;
        }

        _isUpdating = false;
    }

    public void ClearBalls()
    {
        while (_isUpdating)
        {
            // Czekaj, a¿ aktualizacja pozycji kul zostanie zakoñczona
        }

        _ballsRepository.RemoveAllBalls();
        _tasks.Clear();
    }

    public IEnumerable<Ball> GetAllBalls()
    {
        return _ballsRepository.GetAllBalls();
    }

    private void CheckCollisionWithBounds(Ball ball)
    {
        double margin = ball.Radius;

        if (ball.X - ball.Radius < _boundary.X || ball.X + ball.Radius > _boundary.X + _boundary.Width - 2 * margin)
        {
            ball.VelocityX = -ball.VelocityX;
            _logger.Log($"Ball {ball.Id} bounced off the vertical wall at position ({ball.X}, {ball.Y}).");
        }

        if (ball.Y - ball.Radius < _boundary.Y || ball.Y + ball.Radius > _boundary.Y + _boundary.Height - 2 * margin)
        {
            ball.VelocityY = -ball.VelocityY;
            _logger.Log($"Ball {ball.Id} bounced off the horizontal wall at position ({ball.X}, {ball.Y}).");
        }
    }

    public void SetCanvasSize(double width, double height)
    {
        _canvasWidth = width;
        _canvasHeight = height;
    }

    private Color GetRandomColor()
    {
        return D_colors[_random.Next(D_colors.Length)];
    }

    private bool IsCollision(Ball ball1, Ball ball2)
    {
        double dx = ball1.X - ball2.X;
        double dy = ball1.Y - ball2.Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);
        return distance < ball1.Radius + ball2.Radius;
    }

    private void HandleCollision(Ball ball1, Ball ball2)
    {
        double dx = ball1.X - ball2.X;
        double dy = ball1.Y - ball2.Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        if (distance < ball1.Radius + ball2.Radius)
        {
            double nx = dx / distance;
            double ny = dy / distance;
            double p = 2 * (ball1.VelocityX * nx + ball1.VelocityY * ny - ball2.VelocityX * nx - ball2.VelocityY * ny) / (ball1.Mass + ball2.Mass);

            ball1.VelocityX -= p * ball2.Mass * nx;
            ball1.VelocityY -= p * ball2.Mass * ny;
            ball2.VelocityX += p * ball1.Mass * nx;
            ball2.VelocityY += p * ball1.Mass * ny;

            double overlap = 0.5 * (distance - ball1.Radius - ball2.Radius);
            ball1.X -= overlap * nx;
            ball1.Y -= overlap * ny;
            ball2.X += overlap * nx;
            ball2.Y += overlap * ny;

            _logger.Log($"Ball {ball1.Id} collided with Ball {ball2.Id} at position ({ball1.X}, {ball1.Y}).");
        }
    }
}
