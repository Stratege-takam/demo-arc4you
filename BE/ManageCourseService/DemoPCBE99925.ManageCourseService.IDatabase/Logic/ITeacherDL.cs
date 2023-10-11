using Arc4u;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic;

public interface ITeacherDL 
{
    Task SaveAsync(Teacher entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Teacher> entities, CancellationToken cancellationToken);

    Task<Teacher?> GetByIdAsync(Guid id, Graph<Teacher> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Teacher> GetAllAsync(Graph<Teacher> graph, CancellationToken cancellationToken);
}