using Arc4u;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic;

public interface ICoursePersonDL 
{
    Task SaveAsync(CoursePerson entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<CoursePerson> entities, CancellationToken cancellationToken);

    Task<CoursePerson?> GetByIdAsync(Guid id, Graph<CoursePerson> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<CoursePerson> GetAllAsync(Graph<CoursePerson> graph, CancellationToken cancellationToken);
}