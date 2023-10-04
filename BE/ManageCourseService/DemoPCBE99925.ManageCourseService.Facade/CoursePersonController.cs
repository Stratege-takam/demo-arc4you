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
///  CoursePersonController class.  
/// </summary>
[Authorize]
[ApiController]
[Route("ManageCourseService/facade/[controller]")]
[ApiExplorerSettings(GroupName = "facade")]
[ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status400BadRequest)]
public class CoursePersonController : ControllerBase
{
	private readonly ICoursePersonBL _coursePersonBL;

	/// <summary>
	/// Api for CoursePerson.
	/// </summary>
	public CoursePersonController(ICoursePersonBL coursePersonBL)
	{
		_coursePersonBL = coursePersonBL;
	}

	/// <summary>
	/// Access to the business layer.
	/// </summary>
	protected ICoursePersonBL CoursePersonBL => _coursePersonBL;

	/// <summary>
	/// Fetch a CoursePerson based on its id.
	/// </summary>
	/// <response code="200"  nullable="true">The CoursePerson is found.</response>
	/// <returns>The CoursePersonDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(CoursePersonDto), StatusCodes.Status200OK)]
	[HttpGet("id/{id:Guid}")]
	public async Task<IActionResult> GetByIdAsync([FromServices] IMapper mapper, Guid id, CancellationToken cancellation)
	{
		var result = mapper.Map<CoursePersonDto>(await CoursePersonBL.GetByIdAsync(id, new Graph<Domain.CoursePerson>(), cancellation).ConfigureAwait(true));

		return Ok(result);
	}

	/// <summary>
	/// Fetch all the entities of type CoursePerson.
	/// </summary>
	/// <response code="200">The list of CoursePerson entities is found.</response>
	/// <returns>The collection of CoursePersonDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(IEnumerable<CoursePersonDto>), StatusCodes.Status200OK)]
	[HttpGet("~/managecourseservice/facade/coursepeople")]
	public async Task<IActionResult> GetAllAsync([FromServices] IMapper mapper, CancellationToken cancellation)
	{
		var entities = await CoursePersonBL.GetAllAsync(new Graph<Domain.CoursePerson>(), cancellation).ToListAsync(cancellation).ConfigureAwait(true);
		return Ok(mapper.Map<IEnumerable<CoursePersonDto>>(entities));
	}

	/// <summary>
	/// Save entity CoursePerson.
	/// </summary>
	/// <response code="200">The CoursePerson is saved.</response>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
	[HttpPut]
	public async Task<IActionResult> SaveAsync([FromServices] IMapper mapper, [FromBody] CoursePersonDto entity, CancellationToken cancellation)
	{
		var domainEntity = mapper.Map<Domain.CoursePerson>(entity);
		await CoursePersonBL.SaveAsync(domainEntity, cancellation).ConfigureAwait(false);
		return Ok();
	}
}
