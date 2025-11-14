using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.Converters
{
    public class BoolToOuiNonConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? "Oui" : "Non";
            return "Non";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (string.Equals(s, "Oui", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (string.Equals(s, "Non", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return false;
        }
    }
}
