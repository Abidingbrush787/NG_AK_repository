using System;

namespace UPojazd
{
    // Klasa bazowa Pojazd
    public class Pojazd
    {
        public string Nazwa { get; set; }
        public int MaxPredkosc { get; set; }

        public Pojazd(string nazwa, int maxPredkosc)
        {
            Nazwa = nazwa;
            MaxPredkosc = maxPredkosc;
        }

        public virtual string Opis()
        {
            return $"Pojazd {Nazwa} może jechać z maksymalną prędkością {MaxPredkosc} km/h.";
        }
    }

    // Klasa pochodna Samochod
    public class Samochod : Pojazd
    {
        public int IloscDrzwi { get; set; }

        public Samochod(string nazwa, int maxPredkosc, int iloscDrzwi)
            : base(nazwa, maxPredkosc)
        {
            IloscDrzwi = iloscDrzwi;
        }

        public override string Opis()
        {
            return $"{base.Opis()} Ma {IloscDrzwi} drzwi.";
        }
    }
}

