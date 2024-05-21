using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace Data_Layer
{
    public class Boundary
    {
        public double X { get; set; } // Pozycja X lewego górnego rogu
        public double Y { get; set; } // Pozycja Y lewego górnego rogu
        public double Width { get; set; } // Szerokość prostokąta
        public double Height { get; set; } // Wysokość prostokąta
        public Color BorderColor { get; set; } // Kolor obwódki

        public Boundary(double x, double y, double width, double height, Color borderColor)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            BorderColor = borderColor;
        }
    }
}
