using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Converters;

/// <summary>
/// The converter checks if a type of ICollection contains Rows or not. if no rows exists, we will return a Visibility.Hidden.
/// It is also possible to giv parameter enabling you to Collapse or Hide following the collection is null or empty.
/// </summary>
public class CollectionIsNullOrEmptyToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        Visibility valueToReturnIfNullOrEmpty = Visibility.Hidden;
        Visibility valueToReturnIfNotEmpty = Visibility.Visible;

        ICollection collection = value as ICollection;

        if (parameter != null)
        {
            string[] parameters = parameter.ToString().Split(';');

            for (int i = 0; i < parameters.Length; i++)
            {
                string[] subParameters = parameters[i].Split(':');
                string key = subParameters[0].ToLower();
                if (key.Equals("Empty", StringComparison.InvariantCultureIgnoreCase))
                    valueToReturnIfNullOrEmpty = (Visibility)Enum.Parse(typeof(Visibility), subParameters[1]);
                else if (key.Equals("NotEmpty", StringComparison.InvariantCultureIgnoreCase))
                    valueToReturnIfNotEmpty = (Visibility)Enum.Parse(typeof(Visibility), subParameters[1]);
                else
                    throw new NotImplementedException();
            }
        }

        if (collection == null || collection.Count == 0)
            return valueToReturnIfNullOrEmpty;

        return valueToReturnIfNotEmpty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}