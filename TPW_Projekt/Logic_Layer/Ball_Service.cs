using System;
using System.Collections.Generic;
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

        public BallService(double canvasWidth, double canvasHeight, IBallRepository ballsRepository)
        {
            SetCanvasSize(canvasWidth, canvasHeight);
            _ballsRepository = ballsRepository ?? throw new ArgumentNullException(nameof(ballsRepository));
        }

        public Ball CreateBall()
        {
            double x = _random.NextDouble() * _canvasWidth;
            double y = _random.NextDouble() * _canvasHeight;
            double velocityX = _random.NextDouble() * 2 - 1; // Prêdkoœæ w zakresie -1 do 1
            double velocityY = _random.NextDouble() * 2 - 1;
            double radius = 10; // Sta³y promieñ
            Color color = GetRandomColor(); // Losowy kolor

            Ball ball = new Ball(x, y, velocityX, velocityY, radius, color);
            _ballsRepository.AddBall(ball);
            return ball;
        }

        public void UpdateBallPositions(double timeFactor)
        {
            foreach (var ball in _ballsRepository.GetAllBalls())
            {
                ball.X += ball.VelocityX * timeFactor;
                ball.Y += ball.VelocityY * timeFactor;
                CheckCollisionWithBounds(ball);
            }
        }

        public IEnumerable<Ball> GetAllBalls()
        {
            return _ballsRepository.GetAllBalls();
        }

        public void ClearBalls()
        {
            _ballsRepository.RemoveAllBalls();
        }

        private void CheckCollisionWithBounds(Ball ball)
        {
            if (ball.X - ball.Radius < 0 || ball.X + ball.Radius > _canvasWidth)
            {
                ball.VelocityX = -ball.VelocityX;
            }

            if (ball.Y - ball.Radius < 0 || ball.Y + ball.Radius > _canvasHeight)
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
    }
}
