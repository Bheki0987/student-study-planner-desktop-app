using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace StudentStudyPlanner
{
    public class DateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                if (date.Date == DateTime.Today.Date)
                    return new SolidColorBrush(Colors.Red);
                else if (date.Date == DateTime.Today.AddDays(1).Date)
                    return new SolidColorBrush(Colors.Green);
                else
                    return new SolidColorBrush(Colors.Blue);
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}