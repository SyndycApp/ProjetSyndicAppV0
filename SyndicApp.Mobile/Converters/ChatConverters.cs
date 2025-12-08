using System;
using System.Globalization;

namespace SyndicApp.Mobile.Converters
{
    public class BubbleAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userId = value?.ToString();
            return userId == App.UserId ? LayoutOptions.End : LayoutOptions.Start;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }

    public class BubbleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userId = value?.ToString();

            if (string.IsNullOrEmpty(App.UserId))
                return "White";   // fallback

            return userId == App.UserId ? "#DCF8C6" : "#FFFFFF";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }

}
