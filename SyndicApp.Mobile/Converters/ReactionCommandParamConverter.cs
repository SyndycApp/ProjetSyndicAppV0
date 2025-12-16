using System.Globalization;
using SyndicApp.Mobile.Models;

namespace SyndicApp.Mobile.Converters;

public class ReactionCommandParamConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MessageDto msg || parameter is not string emoji)
            return null!;

        return new ReactionCommandParam
        {
            Message = msg,
            Emoji = emoji
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
