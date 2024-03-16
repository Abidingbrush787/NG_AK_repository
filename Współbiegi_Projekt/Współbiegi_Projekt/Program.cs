using System;
using UPojazd;
class Program
    {
        static void Main(string[] args)
        {
            var samochod1 = new Samochod("Toyota", 180, 4);
            var samochod2 = new Samochod("BMW", 160, 2);
            
        Console.WriteLine(samochod1.Opis());
        Console.WriteLine(samochod2.Opis());

    }
    }

