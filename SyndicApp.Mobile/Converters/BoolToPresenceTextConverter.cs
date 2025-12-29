using System.Globalization;

namespace SyndicApp.Mobile.Converters;

public class BoolToPresenceTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is true ? "🟢 En service" : "🔴 Hors service";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
