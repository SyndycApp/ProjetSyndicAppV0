using System.Globalization;

namespace SyndicApp.Mobile.Converters;
public class WaveBarColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double progress || parameter is not double barIndex)
            return Colors.LightGray;

        const int totalBars = 25;
        var normalizedIndex = barIndex / totalBars;

        return normalizedIndex <= progress
            ? Color.FromArgb("#2563EB")   // bleu
            : Color.FromArgb("#CBD5E1"); // gris
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

