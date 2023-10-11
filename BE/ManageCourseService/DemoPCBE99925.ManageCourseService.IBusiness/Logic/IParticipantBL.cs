using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using Arc4u;

namespace EG.DemoPCBE99925.ManageCourseService.IBusiness;

public interface  IParticipantBL 
{
    Task<Participant?> GetByIdAsync(Guid Id, Graph<Participant> graph, CancellationToken cancellationToken);

    IAsyncEnumerable<Participant> GetAllAsync(Graph<Participant> graph, CancellationToken cancellationToken);

    Task SaveAsync(Participant entity, CancellationToken cancellationToken);

    Task SaveAsync(ICollection<Participant> entities, CancellationToken cancellationToken);
}