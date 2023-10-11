using Arc4u;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


namespace EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic;

public interface IStudentDL 
{
    Task SaveAsync(Student entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Student> entities, CancellationToken cancellationToken);

    Task<Student?> GetByIdAsync(Guid id, Graph<Student> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Student> GetUnflowCourseByIdAsync(Guid courseId,
        Graph<Student> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Student> GetAllAsync(Graph<Student> graph, CancellationToken cancellationToken);
}
