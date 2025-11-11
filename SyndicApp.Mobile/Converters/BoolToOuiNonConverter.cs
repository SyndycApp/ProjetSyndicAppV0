// SyndicApp.Mobile/Converters/BoolToOuiNonConverter.cs
using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.Converters
{
    public sealed class BoolToOuiNonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b ? (b ? " Oui" : " Non") : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is string s && s.Equals(" Oui", StringComparison.OrdinalIgnoreCase);
    }
}
