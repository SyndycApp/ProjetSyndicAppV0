using System;
using System.Globalization;

namespace SyndicApp.Mobile.Converters
{
    public class MessageSeenIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSeen = value is bool b && b;

            return isSeen ? "double_tick_blue.png" : "double_tick_grey.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
