using Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Presentation_Layer.Model
{
    public static class BallExtensions
    {
        public static Ellipse ToEllipse(this Ball ball)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = ball.Radius * 2,
                Height = ball.Radius * 2,
                Fill = new SolidColorBrush(ball.Color)
            };

            // Ustaw pozycję kuli na Canvas
            Canvas.SetLeft(ellipse, ball.X - ball.Radius);
            Canvas.SetTop(ellipse, ball.Y - ball.Radius);

            return ellipse;
        }


    }

}
