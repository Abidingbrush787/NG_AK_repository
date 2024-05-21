using System;
using System.Globalization;
using System.Windows.Data;

namespace TPW_Projekt.Helpers
{
    public class RadiusToDiameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double radius)
            {
                return radius * 2; // Zwróć średnicę, która jest dwukrotnością promienia.
            }
            return null; // Możesz zwrócić null lub rzucić wyjątek, jeśli preferujesz.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Konwersja z powrotem nie jest potrzebna w tym scenariuszu.
        }
    }
}

