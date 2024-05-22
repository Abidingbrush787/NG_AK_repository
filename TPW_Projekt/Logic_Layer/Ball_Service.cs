using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using Data_Layer;
using Logic_Layer.Interfaces;

namespace Logic_Layer
{
    public class BallService : IBallService
    {
        private readonly IBallRepository _ballsRepository;
        private readonly Random _random = new Random();
        private double _canvasWidth;
        private double _canvasHeight;
        private Boundary _boundary;
        private List<Task> _tasks = new List<Task>(); // Dodajemy listê zadañ

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

        public BallService(Boundary boundary, IBallRepository ballsRepository)
        {
            _boundary = boundary ?? throw new ArgumentNullException(nameof(boundary));
            _ballsRepository = ballsRepository ?? throw new ArgumentNullException(nameof(ballsRepository));
        }

        private int _nextBallId = 0;

        public Ball CreateBall()
        {
            double radius = 10; // Sta³y promieñ
            double margin = 20; // Dodajemy margines
            double x = _random.NextDouble() * (_boundary.Width - 2 * radius - 2 * margin) + radius + margin;
            double y = _random.NextDouble() * (_boundary.Height - 2 * radius - 2 * margin) + radius + margin;
            double mass = (_random.NextDouble()) + 1;
            double velocityX = (_random.NextDouble() * 2 - 1) / mass; // Prêdkoœæ w zakresie -1 do 1 podzielona przez masê
            double velocityY = (_random.NextDouble() * 2 - 1) / mass; // Prêdkoœæ w zakresie -1 do 1 podzielona przez masê
            Color color = GetRandomColor(); // Losowy kolor

            Ball ball = new Ball(x, y, velocityX, velocityY, radius, color, mass);
            ball.Id = _nextBallId++;
            _ballsRepository.AddBall(ball);
            return ball;
        }

        private bool _isUpdating = false;

        public async Task UpdateBallPositions(double timeFactor)
        {
            _isUpdating = true;
            _tasks.Clear(); // Resetujemy listê zadañ na pocz¹tku ka¿dej aktualizacji

            var allBalls = _ballsRepository.GetAllBalls().OrderBy(ball => ball.Id).ToList();

            foreach (var ball in allBalls)
            {
                _tasks.Add(Task.Run(() =>
                {
                    ball.X += ball.VelocityX * timeFactor;
                    ball.Y += ball.VelocityY * timeFactor;
                    CheckCollisionWithBounds(ball);

                    // SprawdŸ zderzenia z innymi kulkami...
                    foreach (var otherBall in allBalls)
                    {
                        if (otherBall != ball && IsCollision(ball, otherBall))
                        {
                            // Sekcja krytyczna: aktualizuj prêdkoœci po zderzeniu...
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

            _isUpdating = false;
        }


        public void ClearBalls()
        {
            while (_isUpdating)
            {
                // Czekaj, a¿ aktualizacja pozycji kul zostanie zakoñczona
            }

            _ballsRepository.RemoveAllBalls();
            _tasks.Clear(); // Resetujemy listê zadañ po usuniêciu wszystkich kulek
        }

        public IEnumerable<Ball> GetAllBalls()
        {
            return _ballsRepository.GetAllBalls();
        }

        private void CheckCollisionWithBounds(Ball ball)
        {
            double margin = ball.Radius; // Dodajemy margines równy promieniowi kuli

            if (ball.X - ball.Radius < _boundary.X || ball.X + ball.Radius > _boundary.X + _boundary.Width - 2 * margin)
            {
                ball.VelocityX = -ball.VelocityX;
            }

            if (ball.Y - ball.Radius < _boundary.Y || ball.Y + ball.Radius > _boundary.Y + _boundary.Height - 2 * margin)
            {
                ball.VelocityY = -ball.VelocityY;
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
            // Oblicz ró¿nicê w pozycjach
            double dx = ball1.X - ball2.X;
            double dy = ball1.Y - ball2.Y;

            // Oblicz odleg³oœæ miêdzy kulkami
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // SprawdŸ, czy kule siê zderzaj¹
            if (distance < ball1.Radius + ball2.Radius)
            {
                // Oblicz wektor normalny
                double nx = dx / distance;
                double ny = dy / distance;

                // Oblicz sk³adow¹ prêdkoœci wzd³u¿ wektora normalnego (prêdkoœæ wzglêdna)
                double p = 2 * (ball1.VelocityX * nx + ball1.VelocityY * ny - ball2.VelocityX * nx - ball2.VelocityY * ny) / (ball1.Mass + ball2.Mass);

                // Zaktualizuj prêdkoœci kulek
                ball1.VelocityX -= p * ball2.Mass * nx;
                ball1.VelocityY -= p * ball2.Mass * ny;
                ball2.VelocityX += p * ball1.Mass * nx;
                ball2.VelocityY += p * ball1.Mass * ny;

                // Korekta pozycji, aby zapobiec przenikaniu
                double overlap = 0.5 * (distance - ball1.Radius - ball2.Radius);
                ball1.X -= overlap * nx;
                ball1.Y -= overlap * ny;
                ball2.X += overlap * nx;
                ball2.Y += overlap * ny;
            }
        }
    }
}
