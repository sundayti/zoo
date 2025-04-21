using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.DTOs;
/// <summary>
/// Data required to create a new animal.
/// </summary>
[SwaggerSchema("Schema for creating a new animal")]
public record CreateAnimalDto
{
    /// <summary>
    /// Species of the animal (e.g., "Lion").
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Species of the animal (e.g., \"Lion\")")]
    public string Species { get; init; }

    /// <summary>
    /// Name (nickname) of the animal.
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Name (nickname) of the animal")]
    public string Name { get; init; }

    /// <summary>
    /// Birth date in ISO 8601 format (UTC).
    /// </summary>
    [Required]
    [SwaggerSchema(Format = "date-time", Description = "Birth date in ISO 8601 format (UTC)")]
    public DateTime BirthDate { get; init; }

    /// <summary>
    /// Gender of the animal.
    /// Allowed values: "Male", "Female", "Unknown".
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Gender of the animal. Allowed values: Male, Female, Unknown")]
    public string Gender { get; init; }

    /// <summary>
    /// Favorite food of the animal (e.g., "Meat", "Grass").
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Favorite food of the animal (e.g., \"Meat\", \"Grass\")")]
    public string FavoriteFood { get; init; }

    /// <summary>
    /// Preferred enclosure type for the animal.
    /// Allowed values: "Predator", "Herbivore", "Aviary", "Aquarium".
    /// </summary>
    [Required]
    [SwaggerSchema(Description = "Preferred enclosure type. Allowed values: Predator, Herbivore, Aviary, Aquarium")]
    public string PreferredEnclosureType { get; init; }

    /// <summary>
    /// Optional ID of an enclosure for initial assignment.
    /// If omitted, the system may auto-assign based on preferred type.
    /// </summary>
    [SwaggerSchema(Description = "Optional ID of an enclosure for initial assignment")]
    public Guid? InitialEnclosureId { get; init; }
}