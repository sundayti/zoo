using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for gathering zoo-wide statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statsService;

    public StatisticsController(IStatisticsService statsService)
        => _statsService = statsService;

    /// <summary>
    /// Retrieve current zoo statistics.
    /// </summary>
    /// <returns>
    /// Statistics including total number of animals, total enclosures,
    /// occupied enclosures, and free enclosures.
    /// </returns>
    [HttpGet]
    [SwaggerOperation(
        Summary     = "Get zoo statistics",
        Description = "Returns aggregate statistics: total animals, total enclosures, occupied enclosures, and free enclosures."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Statistics retrieved successfully", typeof(ZooStatisticsDto))]
    public async Task<IActionResult> Get()
    {
        var dto = await _statsService.GetStatisticsAsync();
        return Ok(dto);
    }
}
