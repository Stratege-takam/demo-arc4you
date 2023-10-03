using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using Arc4u;

namespace EG.DemoPCBE99925.ManageCourseService.IBusiness;

public interface  ITeacherBL 
{
    Task<Teacher?> GetByIdAsync(Guid Id, Graph<Teacher> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Teacher> GetAllAsync(Graph<Teacher> graph, CancellationToken cancellationToken);

    Task SaveAsync(Teacher entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Teacher> entities, CancellationToken cancellationToken);
}