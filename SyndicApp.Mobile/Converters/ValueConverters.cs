using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.Converters
{
    /// <summary>
    /// Retourne TRUE si la valeur == 0
    /// </summary>
    public class EqualsZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d)
                return d == 0;

            if (value is int i)
                return i == 0;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Retourne TRUE si la valeur > 0
    /// </summary>
    public class GreaterThanZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d)
                return d > 0;

            if (value is int i)
                return i > 0;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
