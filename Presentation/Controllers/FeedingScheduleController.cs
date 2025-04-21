using Application.DTOs;
using Application.Interfaces;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Mappers;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Manage feeding schedules for animals")]
public class FeedingScheduleController : ControllerBase
{
    private readonly IFeedingOrganizationService _feedingService;

    public FeedingScheduleController(IFeedingOrganizationService feedingService)
        => _feedingService = feedingService;

    /// <summary>
    /// Retrieve feeding schedules.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
        Summary     = "Get feeding schedule",
        Description = "Returns all feeding schedule entries, or only those for the specified date (UTC) if provided.")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of feeding schedule entries", typeof(IEnumerable<FeedingScheduleDto>))]
    public async Task<IActionResult> Get(
        [FromQuery, SwaggerParameter("Filter schedules by date (UTC)", Required = false)]
        DateTimeOffset? date = null)
    {
        var list = await _feedingService.GetScheduleAsync(date);
        return Ok(list.Select(fs => fs.ToDto()));
    }

    /// <summary>
    /// Create a new feeding schedule entry.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(
        Summary     = "Schedule new feeding",
        Description = "Creates a feeding schedule entry for the specified animal at the given time and food type.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Feeding scheduled successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input or animal not found")]
    public async Task<IActionResult> Schedule(
        [FromBody, SwaggerRequestBody("Data for the new feeding schedule entry", Required = true)]
        CreateFeedingScheduleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _feedingService.ScheduleFeedingAsync(
            AnimalId.From(dto.AnimalId),
            dto.Time,
            FoodType.From(dto.FoodType));
        return NoContent();
    }

    /// <summary>
    /// Mark a feeding schedule entry as completed.
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    [SwaggerOperation(
        Summary     = "Complete feeding",
        Description = "Marks the specified feeding schedule entry as completed.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Feeding marked as completed")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Feeding schedule entry not found")]
    public async Task<IActionResult> Complete(
        [SwaggerParameter("ID of the feeding schedule entry to complete", Required = true)]
        Guid id)
    {
        await _feedingService.MarkFeedingCompletedAsync(id);
        return NoContent();
    }
}
