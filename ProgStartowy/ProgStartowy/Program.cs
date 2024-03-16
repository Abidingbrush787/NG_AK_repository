using Point;
using System;

class Program
{
    static void Main()
    {
        // Utworzenie instancji klasy Point2D
        Point2D punkt = new Point2D(3.0, 4.0);

        // Wyświetlenie współrzędnych punktu
        Console.WriteLine($"Punkt ma współrzędne: {punkt}");

        // Zmiana współrzędnych punktu
        punkt.X = 5.0;
        punkt.Y = 6.0;

        // Ponowne wyświetlenie zmienionych współrzędnych punktu
        Console.WriteLine($"Punkt po zmianie ma współrzędne: {punkt}");

        // Oczekiwanie na naciśnięcie klawisza przed zamknięciem konsoli
        Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć...");
        Console.ReadKey();
    }
}
