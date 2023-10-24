using System.Windows.Media;
using System.Windows;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Controls.Helpers;
public static class WindowHelper
{
    public static Window GetParentWindow(this DependencyObject dependencyObject)
    {
        while (true)
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null)
                return null;

            var window = parent as Window;
            if (window != null)
                return window;

            dependencyObject = parent;
        }
    }
}
