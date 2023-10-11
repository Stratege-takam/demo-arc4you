using Arc4u;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic;

public interface IParticipantDL 
{
    Task SaveAsync(Participant entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Participant> entities, CancellationToken cancellationToken);

    Task<Participant?> GetByIdAsync(Guid id, Graph<Participant> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Participant> GetAllAsync(Graph<Participant> graph, CancellationToken cancellationToken);
}