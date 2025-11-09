using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.Converters
{
    public sealed class ProgressConverter : IValueConverter
    {
        // value peut être null; retourne un double 0..1
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null) return 0d;

            try
            {
                var v = System.Convert.ToDouble(value, culture);
                if (double.IsNaN(v) || double.IsInfinity(v)) return 0d;

                // Si on te passe 0..100, normaliser vers 0..1
                if (v > 1d) v = v / 100d;

                return Math.Clamp(v, 0d, 1d);
            }
            catch
            {
                return 0d;
            }
        }

        // Ici on ne reconvertit pas; renvoyer la valeur telle quelle est suffisant
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value;
    }
}
