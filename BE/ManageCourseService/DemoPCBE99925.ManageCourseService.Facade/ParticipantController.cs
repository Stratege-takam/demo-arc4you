using Arc4u;
using Arc4u.ServiceModel;
using AutoMapper;
using EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;
using EG.DemoPCBE99925.ManageCourseService.IBusiness;
using EG.DemoPCBE99925.Rights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EG.DemoPCBE99925.ManageCourseService.Facade;

/// <summary>  
///  ParticipantController class.  
/// </summary>
[Authorize]
[ApiController]
[Route("ManageCourseService/facade/[controller]")]
[ApiExplorerSettings(GroupName = "facade")]
[ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status400BadRequest)]
public class ParticipantController : ControllerBase
{
	private readonly IParticipantBL _participantBL;

	/// <summary>
	/// Api for Participant.
	/// </summary>
	public ParticipantController(IParticipantBL participantBL)
	{
		_participantBL = participantBL;
	}

	/// <summary>
	/// Access to the business layer.
	/// </summary>
	protected IParticipantBL ParticipantBL => _participantBL;

	/// <summary>
	/// Fetch a Participant based on its id.
	/// </summary>
	/// <response code="200"  nullable="true">The Participant is found.</response>
	/// <returns>The ParticipantDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ParticipantDto), StatusCodes.Status200OK)]
	[HttpGet("id/{id:Guid}")]
	public async Task<IActionResult> GetByIdAsync([FromServices] IMapper mapper, Guid id, CancellationToken cancellation)
	{
		var result = mapper.Map<ParticipantDto>(await _participantBL.GetByIdAsync(id, new Graph<Domain.Participant>(), cancellation).ConfigureAwait(true));

		return Ok(result);
	}

	/// <summary>
	/// Fetch all the entities of type Participant.
	/// </summary>
	/// <response code="200">The list of Participant entities is found.</response>
	/// <returns>The collection of ParticipantDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(IEnumerable<ParticipantDto>), StatusCodes.Status200OK)]
	[HttpGet("~/managecourseservice/facade/participants")]
	public async Task<IActionResult> GetAllAsync([FromServices] IMapper mapper, CancellationToken cancellation)
	{
		var entities = await _participantBL.GetAllAsync(new Graph<Domain.Participant>(), cancellation).ToListAsync(cancellation).ConfigureAwait(true);
		return Ok(mapper.Map<IEnumerable<ParticipantDto>>(entities));
	}

	/// <summary>
	/// Save entity Participant.
	/// </summary>
	/// <response code="200">The Participant is saved.</response>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
	[HttpPut]
	public async Task<IActionResult> SaveAsync([FromServices] IMapper mapper, [FromBody] ParticipantDto entity, CancellationToken cancellation)
	{
		var domainEntity = mapper.Map<Domain.Participant>(entity);
		await _participantBL.SaveAsync(domainEntity, cancellation).ConfigureAwait(false);
		return Ok();
	}
}