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

[Export(typeof(IStudentDL)), Scoped]
public class  StudentDL : IStudentDL
{
    public StudentDL(DatabaseContext databaseContext)
    {
        ArgumentNullException.ThrowIfNull(databaseContext, nameof(databaseContext));

        _databaseContext = databaseContext;
    }

    private readonly DatabaseContext _databaseContext;

    public async Task<Student?> GetByIdAsync(Guid id, Graph<Student> graph, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(graph, nameof(graph));
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        return await graph.ApplySetReferences(_databaseContext.Students).SingleOrDefaultAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async IAsyncEnumerable<Student> GetAllAsync(Graph<Student> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var entity in graph.ApplySetReferences(_databaseContext.Students)
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false))
            {
                yield return entity;
            }
    }

    public async Task SaveAsync(Student entity, CancellationToken cancellationToken)
    {
        _databaseContext.ChangeTracker.TrackGraph(entity, e => ChangeGraphTracker.Tracker(e));

        await _databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _databaseContext.ChangeTracker.Clear();
    }

    public async Task SaveAsync(ICollection<Student> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
            _databaseContext.ChangeTracker.TrackGraph(entity, e => ChangeGraphTracker.Tracker(e));

        await _databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _databaseContext.ChangeTracker.Clear();
    }
}