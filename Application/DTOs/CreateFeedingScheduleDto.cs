using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.DTOs;
/// <summary>
/// Data required to create a new feeding schedule entry.
/// </summary>
[SwaggerSchema("Schema for scheduling a new feeding event")]
public record CreateFeedingScheduleDto
{
    /// <summary>
    /// ID of the animal to be fed.
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "ID of the animal to be fed")]
    public Guid AnimalId { get; init; }

    /// <summary>
    /// Scheduled feeding time in ISO 8601 format (UTC).
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "date-time", Description = "Scheduled feeding time in ISO 8601 format (UTC)")]
    public DateTimeOffset Time { get; init; }

    /// <summary>
    /// Type of food to feed the animal (e.g., "Meat", "Grass").
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Type of food for feeding (e.g., \"Meat\", \"Grass\")")]
    public string FoodType { get; init; }
}
