using System.Windows.Media;
using Data_Layer;

namespace TPW_Projekt.ViewModel
{
    public class BallViewModel : ViewModelBase
    {
        private Ball _ball;
        public Ball Ball => _ball;
        public double Radius => _ball.Radius;
        public Color Color => _ball.Color;

        public BallViewModel(Ball ball)
        {
            _ball = ball;
        }

        public double X
        {
            get => _ball.X;
            set
            {
                if (_ball.X != value)
                {
                    _ball.X = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        public double Y
        {
            get => _ball.Y;
            set
            {
                if (_ball.Y != value)
                {
                    _ball.Y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        public void UpdatePosition(double timeFactor)
        {
            _ball.X += _ball.VelocityX * timeFactor;
            _ball.Y += _ball.VelocityY * timeFactor;
            System.Diagnostics.Debug.WriteLine($"Ball moving to X: {_ball.X}, Y: {_ball.Y}");
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }
    }
}
