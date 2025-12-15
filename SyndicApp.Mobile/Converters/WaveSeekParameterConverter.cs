using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Converters
{
    public class WaveSeekParameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3)
                return null;

            if (values[0] is not MessageDto message)
                return null;

            if (values[1] is not double barIndex)
                return null;

            if (values[2] is not int totalBars || totalBars <= 1)
                return null;

            // progress entre 0 et 1
            var progress = barIndex / (totalBars - 1);

            progress = Math.Clamp(progress, 0, 1);

            return (message, progress);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
