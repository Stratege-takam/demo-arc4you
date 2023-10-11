using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using Arc4u;
using System.Runtime.CompilerServices;

namespace EG.DemoPCBE99925.ManageCourseService.IBusiness;

public interface  IStudentBL 
{
    Task<Student?> GetByIdAsync(Guid Id, Graph<Student> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Student> GetUnflowCourseByIdAsync(Guid courseId,
        Graph<Student> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Student> GetAllAsync(Graph<Student> graph, CancellationToken cancellationToken);

    Task SaveAsync(Student entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Student> entities, CancellationToken cancellationToken);
}
