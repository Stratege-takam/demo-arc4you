using System;
using System.Globalization;
using System.Windows.Data;


namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Converters;

public class DebugConverter : IValueConverter
{
    #region IValueConverter Members
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
    #endregion
}