using Arc4u;
using Arc4u.Data;
using Arc4u.Dependency.Attribute;
using Arc4u.Diagnostics;
using Arc4u.Security.Principal;
using Arc4u.ServiceModel;
using Arc4u.Transaction;
using EG.DemoPCBE99925.ManageCourseService.Business.Logic.Validators;
using EG.DemoPCBE99925.ManageCourseService.Domain;
using EG.DemoPCBE99925.ManageCourseService.IBusiness;
using EG.DemoPCBE99925.ManageCourseService.IDatabase.Logic;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MessageCategory = Arc4u.ServiceModel.MessageCategory;
using MessageType = Arc4u.ServiceModel.MessageType;

namespace EG.DemoPCBE99925.ManageCourseService.Business;

[Export(typeof(IStudentBL)), Scoped]
public class StudentBL : IStudentBL
{
	private readonly IStudentDL _studentDL;
	private readonly ILogger<StudentBL> _logger;
	private readonly IApplicationContext _applicationContext;

	public StudentBL(IStudentDL studentDL, ILogger<StudentBL> logger, IApplicationContext applicationContext)
	{
		_studentDL = studentDL;
		_logger = logger;
		_applicationContext = applicationContext;
	}
	public async Task<Student?> GetByIdAsync(Guid id, Graph<Student> graph, CancellationToken cancellationToken)
	{
		Student? result = await _studentDL.GetByIdAsync(id, graph, cancellationToken).ConfigureAwait(false);

		if (result is not null)
			(await ApplyRulesAsync(result).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

		return result;
	}

    public async IAsyncEnumerable<Student> GetUnflowCourseByIdAsync(Guid courseId,
        Graph<Student> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var entity in _studentDL.GetUnflowCourseByIdAsync(courseId, graph, cancellationToken))
        {
            (await ApplyRulesAsync(entity).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

            yield return entity;
        }
    }


    public async IAsyncEnumerable<Student> GetAllAsync(Graph<Student> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await foreach (var entity in _studentDL.GetAllAsync(graph, cancellationToken))
		{
			(await ApplyRulesAsync(entity).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			yield return entity;
		}
	}

	public async Task SaveAsync(ICollection<Student> entities, CancellationToken cancellationToken)
	{
		using (var transaction = new ReadCommittedTransactionScope())
		{
			(await PersistAsync(entities, cancellationToken).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			transaction.Complete();
		}
	}


	public async Task SaveAsync(Student entity, CancellationToken cancellationToken)
	{
		using (var transaction = new ReadCommittedTransactionScope())
		{
			(await PersistAsync(entity, cancellationToken).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			transaction.Complete();
		}
	}

	protected async Task<Messages> ApplyRulesAsync(ICollection<Student> entities)
	{
		var msgs = new Messages();

		foreach (var entity in entities)
			msgs.AddRange(await ApplyRulesAsync(entity).ConfigureAwait(false));

		return msgs;
	}

	protected async Task<Messages> ApplyRulesAsync(Student entity)
	{
		var msgs = new Messages();
		try
		{
			switch (entity.PersistChange)
			{
				case PersistChange.Insert:
				case PersistChange.Update:
					entity.AuditedOn = DateTime.UtcNow;
					entity.AuditedBy = _applicationContext.Principal.Profile.Name;
					msgs.AddRange(await ApplySaveRulesAsync(entity).ConfigureAwait(false));
					break;
				case PersistChange.Delete:
					msgs.AddRange(await ApplyDeleteRulesAsync(entity).ConfigureAwait(false));
					break;
				case PersistChange.None:
					msgs.AddRange(await ApplyGetRulesAsync(entity).ConfigureAwait(false));
					break;
			}
		}
		catch (Exception exc)
		{
			// Catch & log Application error (DB errors,...)
			_logger.Technical().Exception(exc).Log();
			msgs.Add(new Message(MessageCategory.Technical, MessageType.Error, exc.Message));
		}
		return msgs;
	}

	protected async Task<Messages> PersistAsync(Student entity, CancellationToken cancellationToken)
	{
		var errorList = await ApplyRulesAsync(entity);
		if (!errorList.Any())
			await _studentDL.SaveAsync(entity, cancellationToken).ConfigureAwait(false);

		return errorList;
	}

	protected async Task<Messages> PersistAsync(ICollection<Student> entities, CancellationToken cancellationToken)
	{
		var errorList = await ApplyRulesAsync(entities);
		if (!errorList.Any())
			await _studentDL.SaveAsync(entities, cancellationToken).ConfigureAwait(false);

		return errorList;
	}

	private async Task<Messages> ApplyGetRulesAsync(Student entity)
	{
		var errorList = new Messages(true);
		await new StudentGetValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
	private async Task<Messages> ApplySaveRulesAsync(Student entity)
	{
		var errorList = new Messages(true);
		await new StudentSaveValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
	private async Task<Messages> ApplyDeleteRulesAsync(Student entity)
	{
		var errorList = new Messages(true);
		await new StudentDeleteValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
}
