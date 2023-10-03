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

[Export(typeof(IPersonBL)), Scoped]
public class PersonBL : IPersonBL
{
	private readonly IPersonDL _personDL;
	private readonly ILogger<PersonBL> _logger;
	private readonly IApplicationContext _applicationContext;

	public PersonBL(IPersonDL personDL, ILogger<PersonBL> logger, IApplicationContext applicationContext)
	{
		_personDL = personDL;
		_logger = logger;
		_applicationContext = applicationContext;
	}
	public async Task<Person?> GetByIdAsync(Guid id, Graph<Person> graph, CancellationToken cancellationToken)
	{
		Person? result = await _personDL.GetByIdAsync(id, graph, cancellationToken).ConfigureAwait(false);

		if (result is not null)
			(await ApplyRulesAsync(result).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

		return result;
	}

	public async IAsyncEnumerable<Person> GetAllAsync(Graph<Person> graph, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await foreach (var entity in _personDL.GetAllAsync(graph, cancellationToken))
		{
			(await ApplyRulesAsync(entity).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			yield return entity;
		}
	}

	public async Task SaveAsync(ICollection<Person> entities, CancellationToken cancellationToken)
	{
		using (var transaction = new ReadCommittedTransactionScope())
		{
			(await PersistAsync(entities, cancellationToken).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			transaction.Complete();
		}
	}


	public async Task SaveAsync(Person entity, CancellationToken cancellationToken)
	{
		using (var transaction = new ReadCommittedTransactionScope())
		{
			(await PersistAsync(entity, cancellationToken).ConfigureAwait(false)).LogAndThrowIfNecessary(_logger);

			transaction.Complete();
		}
	}

	protected async Task<Messages> ApplyRulesAsync(ICollection<Person> entities)
	{
		var msgs = new Messages();

		foreach (var entity in entities)
			msgs.AddRange(await ApplyRulesAsync(entity).ConfigureAwait(false));

		return msgs;
	}

	protected async Task<Messages> ApplyRulesAsync(Person entity)
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

	protected async Task<Messages> PersistAsync(Person entity, CancellationToken cancellationToken)
	{
		var errorList = await ApplyRulesAsync(entity);
		if (!errorList.Any())
			await _personDL.SaveAsync(entity, cancellationToken).ConfigureAwait(false);

		return errorList;
	}

	protected async Task<Messages> PersistAsync(ICollection<Person> entities, CancellationToken cancellationToken)
	{
		var errorList = await ApplyRulesAsync(entities);
		if (!errorList.Any())
			await _personDL.SaveAsync(entities, cancellationToken).ConfigureAwait(false);

		return errorList;
	}

	private async Task<Messages> ApplyGetRulesAsync(Person entity)
	{
		var errorList = new Messages(true);
		await new PersonGetValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
	private async Task<Messages> ApplySaveRulesAsync(Person entity)
	{
		var errorList = new Messages(true);
		await new PersonSaveValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
	private async Task<Messages> ApplyDeleteRulesAsync(Person entity)
	{
		var errorList = new Messages(true);
		await new PersonDeleteValidator(errorList, entity).ValidateAsync().ConfigureAwait(false);

		return errorList;
	}
}