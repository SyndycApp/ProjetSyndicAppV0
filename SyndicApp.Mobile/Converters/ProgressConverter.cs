using System.Globalization;

namespace SyndicApp.Mobile.Converters;

public class ProgressConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal paye && parameter is decimal total && total > 0)
            return (double)(paye / total);
        return 0d;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}