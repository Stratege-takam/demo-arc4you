using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Data;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Common.Menu
{
    public class MenuConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
                return null;

            if (null == parameter)
                throw new ArgumentNullException("parameter");

            var menuItem = value as MenuItem;
            if (null == menuItem)
                throw new InvalidCastException("value argument cannot be casted to MenuItem.");
			
            if (menuItem.IsSeparator)
                return String.Empty;

            if (String.IsNullOrEmpty(menuItem.ResourceType))
            {
                if (parameter.ToString().Equals("Tooltip", StringComparison.InvariantCultureIgnoreCase))
                    return menuItem.ToolTip;

                return menuItem.Text;
            }

            var man = new ResourceManager(Type.GetType(menuItem.ResourceType));

            var field = parameter.ToString();
            if (field.Equals("Text", StringComparison.InvariantCultureIgnoreCase))
            {
                if (String.IsNullOrEmpty(menuItem.Text))
                    throw new NullReferenceException(
                        String.Format("The text property in the menuitem (key = {0}) should have a value.",
                                      menuItem.Key));
                return man.GetString(menuItem.Text);
            }

            if (field.Equals("Tooltip", StringComparison.InvariantCultureIgnoreCase))
            {
                if (String.IsNullOrEmpty(menuItem.ToolTip))
                    return String.Empty;
                return man.GetString(menuItem.ToolTip);
            }

            throw new ArgumentException("parameter should be set to Text or Tooltip.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
