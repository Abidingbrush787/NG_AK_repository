using System;
using System.Collections.Generic;
using System.Windows.Media;
using Data_Layer;
using Logic_Layer.Interfaces;



namespace Logic_Layer
{
    public class Ball_Service : IBallService
    {
        public List<Ball> balls = new List<Ball>();
        private Random random = new Random();
        private readonly double canvasWidth;
        private readonly double canvasHeight;

        public Color[] D_colors = new Color[] {
                ColorsDefinitions.Red,
                ColorsDefinitions.Green,
                ColorsDefinitions.Blue,
                ColorsDefinitions.Yellow,
                ColorsDefinitions.Orange,
                ColorsDefinitions.Purple,
                ColorsDefinitions.Cyan,
                ColorsDefinitions.Magenta
            };


        public Ball_Service(double canvasWidth, double canvasHeight)
        {
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
        }

        public Ball CreateBall()
        {
            double x = random.NextDouble() * canvasWidth;
            double y = random.NextDouble() * canvasHeight;
            double velocityX = random.NextDouble() * 2 - 1; // Prêdkoœæ w zakresie -1 do 1
            double velocityY = random.NextDouble() * 2 - 1;
            double radius = 10; // Sta³y promieñ
            Color color = GetRandomColor(); // Losowy kolor

            Ball ball = new Ball(x, y, velocityX, velocityY, radius, color);
            balls.Add(ball);
            return ball;
        }

        public void UpdateBallPositions(double timeFactor)
        {
            foreach (var ball in balls)
            {
                ball.X += ball.VelocityX * timeFactor;
                ball.Y += ball.VelocityY * timeFactor;
                CheckCollisionWithBounds(ball);
            }
        }

        public void ClearBalls()  //na razie nie potrzebne
        {
            balls.Clear();
        }

        public IEnumerable<Ball> GetAllBalls()
        {
            return balls;
        }

        private void CheckCollisionWithBounds(Ball ball)
        {
            if (ball.X - ball.Radius < 0 || ball.X + ball.Radius > canvasWidth)
            {
                ball.VelocityX = -ball.VelocityX;
            }
            if (ball.Y - ball.Radius < 0 || ball.Y + ball.Radius > canvasHeight)
            {
                ball.VelocityY = -ball.VelocityY;
            }
        }

        private Color GetRandomColor()
        {
           

            return D_colors[random.Next(D_colors.Length)];
        }
    }
}
