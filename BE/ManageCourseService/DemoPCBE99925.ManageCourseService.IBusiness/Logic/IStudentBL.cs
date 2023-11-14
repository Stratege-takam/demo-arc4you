using EG.DemoPCBE99925.ManageCourseService.Domain;
using Arc4u;
using EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic.Help;

namespace EG.DemoPCBE99925.ManageCourseService.IBusiness;

public interface  IStudentBL 
{
    Task<Student?> GetByIdAsync(Guid Id, Graph<Student> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Student> GetUnflowCourseByIdAsync(Guid courseId,
        Graph<Student> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Student> GetAllAsync(Graph<Student> graph, CancellationToken cancellationToken);

    Task SaveAsync(Student entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Student> entities, CancellationToken cancellationToken);

    /// <summary>
    /// To test virtualization in WPF
    /// </summary>
    /// <param name="graph"></param>
    /// <returns></returns>
    Task<IListResult<Student>> GetAllLazyAsync(Graph<Student> graph, int take, int skip, CancellationToken cancellationToken);
}
