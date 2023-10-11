using Arc4u;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic;

public interface ICourseDL 
{
    Task SaveAsync(Course entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Course> entities, CancellationToken cancellationToken);

    Task<Course?> GetByIdAsync(Guid id, Graph<Course> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Course> GetAllAsync(Graph<Course> graph, CancellationToken cancellationToken);
}