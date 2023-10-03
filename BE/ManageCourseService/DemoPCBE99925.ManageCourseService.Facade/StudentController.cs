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
///  StudentController class.  
/// </summary>
[Authorize]
[ApiController]
[Route("ManageCourseService/facade/[controller]")]
[ApiExplorerSettings(GroupName = "facade")]
[ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status400BadRequest)]
public class StudentController : ControllerBase
{
	private readonly IStudentBL _studentBL;

	/// <summary>
	/// Api for Student.
	/// </summary>
	public StudentController(IStudentBL studentBL)
	{
		_studentBL = studentBL;
	}

	/// <summary>
	/// Access to the business layer.
	/// </summary>
	protected IStudentBL StudentBL => _studentBL;

	/// <summary>
	/// Fetch a Student based on its id.
	/// </summary>
	/// <response code="200"  nullable="true">The Student is found.</response>
	/// <returns>The StudentDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
	[HttpGet("id/{id:Guid}")]
	public async Task<IActionResult> GetByIdAsync([FromServices] IMapper mapper, Guid id, CancellationToken cancellation)
	{
		var result = mapper.Map<StudentDto>(await _studentBL.GetByIdAsync(id, new Graph<Domain.Student>(), cancellation).ConfigureAwait(true));

		return Ok(result);
	}

	/// <summary>
	/// Fetch all the entities of type Student.
	/// </summary>
	/// <response code="200">The list of Student entities is found.</response>
	/// <returns>The collection of StudentDto.</returns>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
	[HttpGet("~/managecourseservice/facade/students")]
	public async Task<IActionResult> GetAllAsync([FromServices] IMapper mapper, CancellationToken cancellation)
	{
		var entities = await _studentBL.GetAllAsync(new Graph<Domain.Student>(), cancellation).ToListAsync(cancellation).ConfigureAwait(true);
		return Ok(mapper.Map<IEnumerable<StudentDto>>(entities));
	}

	/// <summary>
	/// Save entity Student.
	/// </summary>
	/// <response code="200">The Student is saved.</response>
	[Authorize(nameof(Access.AccessApplication))]
	[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
	[HttpPut]
	public async Task<IActionResult> SaveAsync([FromServices] IMapper mapper, [FromBody] StudentDto entity, CancellationToken cancellation)
	{
		var domainEntity = mapper.Map<Domain.Student>(entity);
		await _studentBL.SaveAsync(domainEntity, cancellation).ConfigureAwait(false);
		return Ok();
	}
}