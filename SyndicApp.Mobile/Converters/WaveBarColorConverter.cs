using System.Globalization;

namespace SyndicApp.Mobile.Converters;

public class WaveBarColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double barIndex || parameter is not double progress)
            return Colors.LightGray;

        return barIndex <= progress
            ? Color.FromArgb("#2563EB") // bleu actif
            : Color.FromArgb("#CBD5E1"); // gris
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
