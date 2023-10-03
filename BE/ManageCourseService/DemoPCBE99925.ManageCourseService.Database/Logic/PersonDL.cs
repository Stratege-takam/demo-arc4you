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

[Export(typeof(IPersonDL)), Scoped]
public class  PersonDL : IPersonDL
{
    public PersonDL(DatabaseContext databaseContext)
    {
        ArgumentNullException.ThrowIfNull(databaseContext, nameof(databaseContext));

        _databaseContext = databaseContext;
    }

    private readonly DatabaseContext _databaseContext;

    public async Task<Person?> GetByIdAsync(Guid id, Graph<Person> graph, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(graph, nameof(graph));
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        return await graph.ApplySetReferences(_databaseContext.People).SingleOrDefaultAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async IAsyncEnumerable<Person> GetAllAsync(Graph<Person> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var entity in graph.ApplySetReferences(_databaseContext.People)
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false))
            {
                yield return entity;
            }
    }

    public async Task SaveAsync(Person entity, CancellationToken cancellationToken)
    {
        _databaseContext.ChangeTracker.TrackGraph(entity, e => ChangeGraphTracker.Tracker(e));

        await _databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _databaseContext.ChangeTracker.Clear();
    }

    public async Task SaveAsync(ICollection<Person> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
            _databaseContext.ChangeTracker.TrackGraph(entity, e => ChangeGraphTracker.Tracker(e));

        await _databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _databaseContext.ChangeTracker.Clear();
    }
}