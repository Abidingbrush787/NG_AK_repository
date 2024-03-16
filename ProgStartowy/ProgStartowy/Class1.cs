namespace Point;
public class Point2D
{
    // Właściwości automatyczne dla współrzędnych x i y.
    public double X { get; set; }
    public double Y { get; set; }

    // Konstruktor, który inicjalizuje współrzędne punktu.
    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    // Metoda, która zwraca reprezentację punktu jako tekst.
    public override string ToString()
    {
        return $"Point2D({X}, {Y})";
    }
}