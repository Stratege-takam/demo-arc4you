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

[Export(typeof(ICoursePersonBL)), Scoped]
public class CoursePersonBL : ICoursePersonBL
{
	private readonly ICoursePersonDL _coursePersonDL;
	private readonly ILogger<CoursePersonBL> _logger;
	private readonly IApplicationContext _applicationContext;

	public CoursePersonBL(ICoursePersonDL coursePersonDL, ILogger<CoursePersonBL> logger, IApplicationContext applicationContext)
	{
		_coursePersonDL = coursePersonDL;
		_logger = logger;
		_applicationContext = applicationContext;
	}
	public async Task<CoursePerson?> GetByIdAsync(Guid id, Graph<CoursePerson> graph, CancellationToken cancellationToken)
	{
		CoursePerson? result = await _coursePersonDL.GetByIdAsync(id, graph, cancellationToken).ConfigureAwait(false);

		if (result is not null)
			(await ApplyRulesAsync(result).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

		return result;
	}

	public async IAsyncEnumerable<CoursePerson> GetAllAsync(Graph<CoursePerson> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await foreach (var entity in _coursePersonDL.GetAllAsync(graph, cancellationToken))
		{
			(await ApplyRulesAsync(entity).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			yield return entity;
		}
	}

	public async Task SaveAsync(ICollection<CoursePerson> entities, CancellationToken cancellationToken)
	{
		using (var transaction = new ReadCommittedTransactionScope())
		{
			(await PersistAsync(entities, cancellationToken).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			transaction.Complete();
		}
	}


	public async Task SaveAsync(CoursePerson entity, CancellationToken cancellationToken)
	{
		using (var transaction = new ReadCommittedTransactionScope())
		{
			(await PersistAsync(entity, cancellationToken).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			transaction.Complete();
		}
	}

	protected async Task<Messages> ApplyRulesAsync(ICollection<CoursePerson> entities)
	{
		var msgs = new Messages();

		foreach (var entity in entities)
			msgs.AddRange(await ApplyRulesAsync(entity).ConfigureAwait(false));

		return msgs;
	}

	protected async Task<Messages> ApplyRulesAsync(CoursePerson entity)
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

	protected async Task<Messages> PersistAsync(CoursePerson entity, CancellationToken cancellationToken)
	{
		var errorList = await ApplyRulesAsync(entity);
		if (!errorList.Any())
			await _coursePersonDL.SaveAsync(entity, cancellationToken).ConfigureAwait(false);

		return errorList;
	}

	protected async Task<Messages> PersistAsync(ICollection<CoursePerson> entities, CancellationToken cancellationToken)
	{
		var errorList = await ApplyRulesAsync(entities);
		if (!errorList.Any())
			await _coursePersonDL.SaveAsync(entities, cancellationToken).ConfigureAwait(false);

		return errorList;
	}

	private async Task<Messages> ApplyGetRulesAsync(CoursePerson entity)
	{
		var errorList = new Messages(true);
		await new CoursePersonGetValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
	private async Task<Messages> ApplySaveRulesAsync(CoursePerson entity)
	{
		var errorList = new Messages(true);
		await new CoursePersonSaveValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
	private async Task<Messages> ApplyDeleteRulesAsync(CoursePerson entity)
	{
		var errorList = new Messages(true);
		await new CoursePersonDeleteValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
}