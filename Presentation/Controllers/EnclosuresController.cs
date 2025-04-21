using Application.DTOs;
using Domain.ValueObjects;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Mappers;

namespace Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing enclosures in the zoo")]
public class EnclosuresController : ControllerBase
{
    private readonly IEnclosureRepository _enclosureRepo;
    private readonly IAnimalRepository    _animalRepo;

    public EnclosuresController(
        IEnclosureRepository enclosureRepo,
        IAnimalRepository    animalRepo)
    {
        _enclosureRepo = enclosureRepo;
        _animalRepo    = animalRepo;
    }

    /// <summary>
    /// Get all enclosures.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
        Summary     = "Get all enclosures",
        Description = "Returns a list of all enclosures with their details.")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of enclosures", typeof(IEnumerable<EnclosureDto>))]
    public async Task<IActionResult> GetAll()
    {
        var enclosures = await _enclosureRepo.ListAsync();
        return Ok(enclosures.Select(e => e.ToDto()));
    }

    /// <summary>
    /// Get a specific enclosure by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary     = "Get enclosure by ID",
        Description = "Returns the details of the enclosure with the specified ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Enclosure found", typeof(EnclosureDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Enclosure not found")]
    public async Task<IActionResult> Get(
        [SwaggerParameter("ID of the enclosure to retrieve", Required = true)]
        Guid id)
    {
        var enclosure = await _enclosureRepo.GetByIdAsync(EnclosureId.From(id));
        return Ok(enclosure.ToDto());
    }

    /// <summary>
    /// Create a new enclosure.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(
        Summary     = "Create new enclosure",
        Description = "Creates a new enclosure with specified type, size, and capacity.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Enclosure created", typeof(EnclosureDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input")]
    public async Task<IActionResult> Create(
        [FromBody, SwaggerRequestBody("Data for the new enclosure", Required = true)]
        CreateEnclosureDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var enclosure = dto.ToDomain();
        await _enclosureRepo.AddAsync(enclosure);
        return CreatedAtAction(
            nameof(Get),
            new { id = enclosure.Id.Value },
            enclosure.ToDto()
        );
    }

    /// <summary>
    /// Delete an enclosure and free all animals in it.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary     = "Delete enclosure",
        Description = "Deletes the specified enclosure and frees all animals in it.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Enclosure deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Enclosure not found")]
    public async Task<IActionResult> Delete(
        [SwaggerParameter("ID of the enclosure to delete", Required = true)]
        Guid id)
    {
        var encId    = EnclosureId.From(id);
        var enclosure = await _enclosureRepo.GetByIdAsync(encId);

        var animalIds = enclosure.Animals.ToList();
        foreach (var aId in animalIds)
        {
            var animal = await _animalRepo.GetByIdAsync(aId);
            animal.RemoveFromEnclosure();
            await _animalRepo.UpdateAsync(animal);
        }

        await _enclosureRepo.RemoveAsync(enclosure);
        return NoContent();
    }
}
