using Arc4u.ServiceModel;
using EG.DemoPCBE99925.Yarp.Domain;
using EG.DemoPCBE99925.Yarp.IBusiness;
using EG.DemoPCBE99925.Rights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EG.DemoPCBE99925.Yarp.Facade;

/// <summary>
/// Services about the application information.
/// </summary>
[Authorize]
[ApiController]
[Route("/facade/[controller]")]
[ApiExplorerSettings(GroupName = "facade")]
[ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status400BadRequest)]
public class EnvironmentController : ControllerBase
{
    private readonly IEnvironmentInfoBL _environmentInfoBL;

    /// <summary>
    /// Provide information about your Environment.
    /// </summary>
    public EnvironmentController(IEnvironmentInfoBL environmentInfoBL)
    {
        _environmentInfoBL = environmentInfoBL;
    }

    /// <summary>
    /// Access to the business layer.
    /// </summary>
    protected IEnvironmentInfoBL EnvironmentInfoBL => _environmentInfoBL;

    /// <summary>
    /// Return details information about the application.
    /// </summary>
    /// <response code="200" nullable="true">Return the environment information.</response>
    /// <response code="400">Error during the process of the request.</response>
    /// <returns>The Environment name.</returns>
    [Authorize(nameof(Access.AccessApplication))]
    [ProducesResponseType(typeof(EnvironmentInfo), StatusCodes.Status200OK)]
    [HttpGet("")]
    public async Task<IActionResult> GetAsync()
    {
        var result = await EnvironmentInfoBL.GetEnvironmentInfoAsync().ConfigureAwait(false);

        return Ok(result);    
    }
}