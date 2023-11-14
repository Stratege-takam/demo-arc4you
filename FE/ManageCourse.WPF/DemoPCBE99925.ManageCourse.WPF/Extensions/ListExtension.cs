

namespace EG.DemoPCBE99925.ManageCourse.WPF.Extensions;
public static class ListExtension
{
    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
}
