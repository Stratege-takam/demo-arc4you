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
///  TeacherController class.  
/// </summary>
[Authorize]
[ApiController]
[Route("ManageCourseService/facade/[controller]")]
[ApiExplorerSettings(GroupName = "facade")]
[ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status400BadRequest)]
public class TeacherController : ControllerBase
{
	private readonly ITeacherBL _teacherBL;

	/// <summary>
	/// Api for Teacher.
	/// </summary>
	public TeacherController(ITeacherBL teacherBL)
	{
		_teacherBL = teacherBL;
	}

	/// <summary>
	/// Access to the business layer.
	/// </summary>
	protected ITeacherBL TeacherBL => _teacherBL;

	/// <summary>
	/// Fetch a Teacher based on its id.
	/// </summary>
	/// <response code="200"  nullable="true">The Teacher is found.</response>
	/// <returns>The TeacherDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
	[HttpGet("id/{id:Guid}")]
	public async Task<IActionResult> GetByIdAsync([FromServices] IMapper mapper, Guid id, CancellationToken cancellation)
	{
		var result = mapper.Map<TeacherDto>(await _teacherBL.GetByIdAsync(id, new Graph<Domain.Teacher>(), cancellation).ConfigureAwait(true));

		return Ok(result);
	}

	/// <summary>
	/// Fetch all the entities of type Teacher.
	/// </summary>
	/// <response code="200">The list of Teacher entities is found.</response>
	/// <returns>The collection of TeacherDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(IEnumerable<TeacherDto>), StatusCodes.Status200OK)]
	[HttpGet("~/managecourseservice/facade/teachers")]
	public async Task<IActionResult> GetAllAsync([FromServices] IMapper mapper, CancellationToken cancellation)
	{
		var entities = await _teacherBL.GetAllAsync(new Graph<Domain.Teacher>(), cancellation).ToListAsync(cancellation).ConfigureAwait(true);
		return Ok(mapper.Map<IEnumerable<TeacherDto>>(entities));
	}

	/// <summary>
	/// Save entity Teacher.
	/// </summary>
	/// <response code="200">The Teacher is saved.</response>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
	[HttpPut]
	public async Task<IActionResult> SaveAsync([FromServices] IMapper mapper, [FromBody] TeacherDto entity, CancellationToken cancellation)
	{
		var domainEntity = mapper.Map<Domain.Teacher>(entity);
		await _teacherBL.SaveAsync(domainEntity, cancellation).ConfigureAwait(false);
		return Ok();
	}
}