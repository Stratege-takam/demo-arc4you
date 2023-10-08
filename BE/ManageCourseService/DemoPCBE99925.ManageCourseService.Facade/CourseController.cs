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
///  CourseController class.  
/// </summary>
[Authorize]
[ApiController]
[Route("ManageCourseService/facade/[controller]")]
[ApiExplorerSettings(GroupName = "facade")]
[ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status400BadRequest)]
public class CourseController : ControllerBase
{
	private readonly ICourseBL _courseBL;

	/// <summary>
	/// Api for Course.
	/// </summary>
	public CourseController(ICourseBL courseBL)
	{
		_courseBL = courseBL;
	}

	/// <summary>
	/// Access to the business layer.
	/// </summary>
	protected ICourseBL CourseBL => _courseBL;

	/// <summary>
	/// Fetch a Course based on its id.
	/// </summary>
	/// <response code="200"  nullable="true">The Course is found.</response>
	/// <returns>The CourseDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
	[HttpGet("id/{id:Guid}")]
	public async Task<IActionResult> GetByIdAsync([FromServices] IMapper mapper, Guid id, CancellationToken cancellation)
	{
        var result = mapper.Map<CourseDto>(await _courseBL.GetByIdAsync(id, new Graph<Domain.Course>(), cancellation).ConfigureAwait(true));

		return Ok(result);
	}

	/// <summary>
	/// Fetch all the entities of type Course.
	/// </summary>
	/// <response code="200">The list of Course entities is found.</response>
	/// <returns>The collection of CourseDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
	[HttpGet("~/managecourseservice/facade/courses")]
	public async Task<IActionResult> GetAllAsync([FromServices] IMapper mapper, CancellationToken cancellation)
	{
		var entities = await _courseBL.GetAllAsync(new Graph<Domain.Course>(), cancellation).ToListAsync(cancellation).ConfigureAwait(true);
		return Ok(mapper.Map<IEnumerable<CourseDto>>(entities));
	}

	/// <summary>
	/// Save entity Course.
	/// </summary>
	/// <response code="200">The Course is saved.</response>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
	[HttpPut]
	public async Task<IActionResult> SaveAsync([FromServices] IMapper mapper, [FromBody] CourseDto entity, CancellationToken cancellation)
	{
		var domainEntity = mapper.Map<Domain.Course>(entity);
		await _courseBL.SaveAsync(domainEntity, cancellation).ConfigureAwait(false);
		return Ok();
	}
}
