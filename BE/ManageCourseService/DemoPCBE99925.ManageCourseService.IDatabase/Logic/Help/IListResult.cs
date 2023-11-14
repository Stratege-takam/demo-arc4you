namespace EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic.Help;
public interface IListResult<T> where T : class
{
    #region Properties
     int Count { get; set; }
     IEnumerable<T> Results { get; set; }
    #endregion Properties
}
