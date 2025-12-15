using System.Globalization;

namespace SyndicApp.Mobile.Converters;

public class MultiplyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double v && double.TryParse(parameter.ToString(), out var m))
            return v * m;

        return 10;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
