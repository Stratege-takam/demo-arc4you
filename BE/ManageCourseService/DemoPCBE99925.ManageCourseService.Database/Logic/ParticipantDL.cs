using Arc4u;
using Arc4u.Dependency.Attribute;
using Arc4u.EfCore;
using Arc4u.Threading;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


namespace EG.DemoPCBE99925.ManageCourseService.Database.Logic;

[Export(typeof(IParticipantDL)), Scoped]
public class  ParticipantDL : IParticipantDL
{
    public ParticipantDL(DatabaseContext databaseContext)
    {
        ArgumentNullException.ThrowIfNull(databaseContext, nameof(databaseContext));

        _databaseContext = databaseContext;
    }

    private readonly DatabaseContext _databaseContext;

    public async Task<Participant?> GetByIdAsync(Guid id, Graph<Participant> graph, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(graph, nameof(graph));
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        return await graph.ApplySetReferences(_databaseContext.Participants).SingleOrDefaultAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async IAsyncEnumerable<Participant> GetAllAsync(Graph<Participant> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var entity in graph.ApplySetReferences(_databaseContext.Participants)
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false))
            {
                yield return entity;
            }
    }

    public async Task SaveAsync(Participant entity, CancellationToken cancellationToken)
    {
        _databaseContext.ChangeTracker.TrackGraph(entity, e => ChangeGraphTracker.Tracker(e));

        await _databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _databaseContext.ChangeTracker.Clear();
    }

    public async Task SaveAsync(ICollection<Participant> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
            _databaseContext.ChangeTracker.TrackGraph(entity, e => ChangeGraphTracker.Tracker(e));

        await _databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _databaseContext.ChangeTracker.Clear();
    }
}