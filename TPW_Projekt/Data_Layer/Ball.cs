using System.Windows.Media;


namespace Data_Layer
{
    public class Ball
    {
        private System.Windows.Media.Color color;

        public double X { get; set; }
        public double Y { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public double Radius { get; set; }
        public Color Color { get; set; }

        public Ball(double x, double y, double velocityX, double velocityY, double radius, Color color)
        {
            X = x;
            Y = y;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Radius = radius;
            this.Color = color;
        }   
    }

    public class BallRepository : IBallRepository
    {
        public List<Ball> balls = new List<Ball>();

        public void AddBall(Ball ball) 
        {
            balls.Add(ball); 
        }

        public void RemoveBall(Ball ball)
        {
            balls.Remove(ball);
        }

        public List<Ball> GetAllBalls()
        {
            return balls;
        }

        public void RemoveAllBalls()
        {
            balls.Clear();
        }
    }
}
