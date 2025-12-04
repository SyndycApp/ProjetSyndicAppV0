using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Converters
{
    public class ProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal montantPaye &&
                parameter is AppelDeFondsDto appel &&
                appel.MontantTotal > 0)
            {
                return (double)(montantPaye / appel.MontantTotal);
            }

            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
