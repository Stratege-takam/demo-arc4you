using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using Arc4u;

namespace EG.DemoPCBE99925.ManageCourseService.IBusiness;

public interface  ICourseBL 
{
    Task<Course?> GetByIdAsync(Guid Id, Graph<Course> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Course> GetAllAsync(Graph<Course> graph, CancellationToken cancellationToken);

    Task SaveAsync(Course entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Course> entities, CancellationToken cancellationToken);
}