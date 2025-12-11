using System;
using System.Globalization;
using System.Linq;

namespace SyndicApp.Mobile.Converters
{
    public class FirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string fullName || string.IsNullOrWhiteSpace(fullName))
                return "?";

            // Nettoyage espaces
            var parts = fullName
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return "?";

            // 1 seule partie → initiale simple
            if (parts.Length == 1)
                return parts[0][0].ToString().ToUpper();

            // Plusieurs parties → première lettre du premier + première lettre du dernier
            string first = parts[0][0].ToString().ToUpper();
            string last = parts[^1][0].ToString().ToUpper();

            return first + last;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
