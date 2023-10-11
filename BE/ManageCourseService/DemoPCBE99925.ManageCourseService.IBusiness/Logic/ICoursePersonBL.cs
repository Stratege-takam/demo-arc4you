using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using Arc4u;

namespace EG.DemoPCBE99925.ManageCourseService.IBusiness;

public interface  ICoursePersonBL 
{
    Task<CoursePerson?> GetByIdAsync(Guid Id, Graph<CoursePerson> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<CoursePerson> GetAllAsync(Graph<CoursePerson> graph, CancellationToken cancellationToken);

    Task SaveAsync(CoursePerson entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<CoursePerson> entities, CancellationToken cancellationToken);
}