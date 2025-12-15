using System.Globalization;

namespace SyndicApp.Mobile.Converters;

public class PlayPauseIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isPlaying)
            return isPlaying ? "pause_icon.png" : "play_icon.png";

        return "play_icon.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
