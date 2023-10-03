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

[Export(typeof(ICourseBL)), Scoped]
public class CourseBL : ICourseBL
{
	private readonly ICourseDL _courseDL;
	private readonly ILogger<CourseBL> _logger;
	private readonly IApplicationContext _applicationContext;

	public CourseBL(ICourseDL courseDL, ILogger<CourseBL> logger, IApplicationContext applicationContext)
	{
		_courseDL = courseDL;
		_logger = logger;
		_applicationContext = applicationContext;
	}
	public async Task<Course?> GetByIdAsync(Guid id, Graph<Course> graph, CancellationToken cancellationToken)
	{
		Course? result = await _courseDL.GetByIdAsync(id, graph, cancellationToken).ConfigureAwait(false);

		if (result is not null)
			(await ApplyRulesAsync(result).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

		return result;
	}

	public async IAsyncEnumerable<Course> GetAllAsync(Graph<Course> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await foreach (var entity in _courseDL.GetAllAsync(graph, cancellationToken))
		{
			(await ApplyRulesAsync(entity).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			yield return entity;
		}
	}

	public async Task SaveAsync(ICollection<Course> entities, CancellationToken cancellationToken)
	{
		using (var transaction = new ReadCommittedTransactionScope())
		{
			(await PersistAsync(entities, cancellationToken).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			transaction.Complete();
		}
	}


	public async Task SaveAsync(Course entity, CancellationToken cancellationToken)
	{
		using (var transaction = new ReadCommittedTransactionScope())
		{
			(await PersistAsync(entity, cancellationToken).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			transaction.Complete();
		}
	}

	protected async Task<Messages> ApplyRulesAsync(ICollection<Course> entities)
	{
		var msgs = new Messages();

		foreach (var entity in entities)
			msgs.AddRange(await ApplyRulesAsync(entity).ConfigureAwait(false));

		return msgs;
	}

	protected async Task<Messages> ApplyRulesAsync(Course entity)
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

	protected async Task<Messages> PersistAsync(Course entity, CancellationToken cancellationToken)
	{
		var errorList = await ApplyRulesAsync(entity);
		if (!errorList.Any())
			await _courseDL.SaveAsync(entity, cancellationToken).ConfigureAwait(false);

		return errorList;
	}

	protected async Task<Messages> PersistAsync(ICollection<Course> entities, CancellationToken cancellationToken)
	{
		var errorList = await ApplyRulesAsync(entities);
		if (!errorList.Any())
			await _courseDL.SaveAsync(entities, cancellationToken).ConfigureAwait(false);

		return errorList;
	}

	private async Task<Messages> ApplyGetRulesAsync(Course entity)
	{
		var errorList = new Messages(true);
		await new CourseGetValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
	private async Task<Messages> ApplySaveRulesAsync(Course entity)
	{
		var errorList = new Messages(true);
		await new CourseSaveValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
	private async Task<Messages> ApplyDeleteRulesAsync(Course entity)
	{
		var errorList = new Messages(true);
		await new CourseDeleteValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
}