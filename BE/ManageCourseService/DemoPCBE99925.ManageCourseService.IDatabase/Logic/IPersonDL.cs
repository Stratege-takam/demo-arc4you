using Arc4u;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic;

public interface IPersonDL 
{
    Task SaveAsync(Person entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Person> entities, CancellationToken cancellationToken);

    Task<Person?> GetByIdAsync(Guid id, Graph<Person> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Person> GetAllAsync(Graph<Person> graph, CancellationToken cancellationToken);
}