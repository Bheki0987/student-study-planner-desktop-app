using System;
using System.Globalization;
using System.Windows.Data;

namespace StudentStudyPlanner
{
    public class RowHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                return height / 6; // Divide by 6 because there are 6 rows in the calendar grid (excluding the header)
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
