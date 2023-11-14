using EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic.Help;

namespace EG.DemoPCBE99925.ManageCourseService.Database.Logic.Help;
public class ListResult<T>: IListResult<T> where T : class
{
    #region Properties
     public int Count { get; set; }
     public IEnumerable<T> Results { get; set; }
    #endregion Properties
}
