using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using Arc4u;

namespace EG.DemoPCBE99925.ManageCourseService.IBusiness;

public interface  IPersonBL 
{
    Task<Person?> GetByIdAsync(Guid Id, Graph<Person> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Person> GetAllAsync(Graph<Person> graph, CancellationToken cancellationToken);

    Task SaveAsync(Person entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Person> entities, CancellationToken cancellationToken);
}