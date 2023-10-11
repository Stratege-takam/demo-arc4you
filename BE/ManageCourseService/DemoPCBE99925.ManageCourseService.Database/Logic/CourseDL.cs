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

[Export(typeof(ICourseDL)), Scoped]
public class  CourseDL : ICourseDL
{
    public CourseDL(DatabaseContext databaseContext)
    {
        ArgumentNullException.ThrowIfNull(databaseContext, nameof(databaseContext));

        _databaseContext = databaseContext;
    }

    private readonly DatabaseContext _databaseContext;

    public async Task<Course?> GetByIdAsync(Guid id, Graph<Course> graph, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(graph, nameof(graph));
        ArgumentNullException.ThrowIfNull(id, nameof(id));
         
        return await graph.ApplySetReferences(_databaseContext.Courses)
            .Include(e => e.Owner)
            .Include(e => e.CoursePeople).ThenInclude(e => e.Lead)
            .Include(e => e.CoursePeople).ThenInclude(e => e.Participants).ThenInclude(e => e.Student)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken).ConfigureAwait(false);
    }

    public async IAsyncEnumerable<Course> GetAllAsync(Graph<Course> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var entity in graph.ApplySetReferences(_databaseContext.Courses)
                .Select(c => new Course()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Unity =c.Unity,
                    Description = c.Description,
                    IsTeacher = c.Owner is Teacher,
                    OwnerFullname = c.Owner.FirstName + " " + c.Owner.LastName,
                    CanDelete = !c.CoursePeople.Any(),
                    OwnerId = c.OwnerId,
                    CanLead = !c.CoursePeople.Any()
                })
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false))
            {
                yield return entity;
            }
    }

    public async Task SaveAsync(Course entity, CancellationToken cancellationToken)
    {
        _databaseContext.ChangeTracker.TrackGraph(entity, e => ChangeGraphTracker.Tracker(e));

        await _databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _databaseContext.ChangeTracker.Clear();
    }

    public async Task SaveAsync(ICollection<Course> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
            _databaseContext.ChangeTracker.TrackGraph(entity, e => ChangeGraphTracker.Tracker(e));

        await _databaseContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        _databaseContext.ChangeTracker.Clear();
    }
}
