using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.DTOs;
/// <summary>
/// Data required to create a new enclosure.
/// </summary>
[SwaggerSchema("Schema for creating a new enclosure")]
public record CreateEnclosureDto
{
    /// <summary>
    /// Type of the enclosure.
    /// Allowed values: "Predator", "Herbivore", "Aviary", "Aquarium".
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Type of the enclosure. Allowed values: Predator, Herbivore, Aviary, Aquarium")]
    public string Type { get; init; }

    /// <summary>
    /// Physical size of the enclosure (e.g., in square meters).
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Physical size of the enclosure (e.g., square meters)")]
    public int Size { get; init; }

    /// <summary>
    /// Maximum number of animals this enclosure can hold.
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Maximum number of animals the enclosure can hold")]
    public int Capacity { get; init; }
}