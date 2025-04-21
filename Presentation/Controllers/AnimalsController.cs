using Application.DTOs;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Interfaces;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Mappers;

namespace Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Operations for managing animals in the zoo")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepo;
    private readonly IEnclosureRepository _enclosureRepo;
    private readonly IAnimalTransferService _transferService;

    public AnimalsController(
        IAnimalRepository animalRepo,
        IEnclosureRepository enclosureRepo,
        IAnimalTransferService transferService)
    {
        _animalRepo      = animalRepo;
        _enclosureRepo   = enclosureRepo;
        _transferService = transferService;
    }

    /// <summary>
    /// Retrieve all animals.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(
        Summary     = "Get all animals",
        Description = "Returns a list of all animals with their details.")]
    [ProducesResponseType(typeof(IEnumerable<AnimalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var animals = await _animalRepo.ListAsync();
        return Ok(animals.Select(a => a.ToDto()));
    }

    /// <summary>
    /// Retrieve a specific animal by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary     = "Get animal by ID",
        Description = "Returns the details of the animal with the specified ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Animal found", typeof(AnimalDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Animal not found")]
    public async Task<IActionResult> Get(
        [SwaggerParameter("ID of the animal to retrieve", Required = true)]
        Guid id)
    {
        var animal = await _animalRepo.GetByIdAsync(AnimalId.From(id));
        return Ok(animal.ToDto());
    }

    /// <summary>
    /// Create a new animal.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(
        Summary     = "Create new animal",
        Description = "Creates an animal and optionally assigns it to an enclosure.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Animal created", typeof(AnimalDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input")]
    public async Task<IActionResult> Create(
        [FromBody, SwaggerRequestBody("Data for the new animal", Required = true)]
        CreateAnimalDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var animal = dto.ToDomain();
        await _animalRepo.AddAsync(animal);

        // Try assign by InitialEnclosureId if provided
        if (dto.InitialEnclosureId.HasValue)
        {
            await TryAssignToEnclosure(animal, EnclosureId.From(dto.InitialEnclosureId.Value));
        }
        else
        {
            // Auto-assign to first free enclosure of preferred type
            var free = (await _enclosureRepo.ListAsync())
                .FirstOrDefault(e =>
                    e.Type == animal.PreferredEnclosureType &&
                    e.Animals.Count < e.Capacity);
            if (free != null)
                await TryAssignToEnclosure(animal, free.Id);
        }

        return CreatedAtAction(
            nameof(Get),
            new { id = animal.Id.Value },
            animal.ToDto()
        );
    }

    /// <summary>
    /// Mark an animal as sick.
    /// </summary>
    [HttpPost("{id:guid}/sick")]
    [SwaggerOperation(
        Summary     = "Mark animal sick",
        Description = "Sets the health status of the specified animal to Sick.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Animal marked as sick")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Animal not found")]
    public async Task<IActionResult> MarkSick(
        [SwaggerParameter("ID of the animal to mark sick", Required = true)]
        Guid id)
    {
        var animal = await _animalRepo.GetByIdAsync(AnimalId.From(id));
        animal.MarkSick();
        await _animalRepo.UpdateAsync(animal);
        return NoContent();
    }

    /// <summary>
    /// Heal an animal.
    /// </summary>
    [HttpPost("{id:guid}/treat")]
    [SwaggerOperation(
        Summary     = "Treat animal",
        Description = "Sets the health status of the specified animal to Healthy.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Animal treated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Animal not found")]
    public async Task<IActionResult> Treat(
        [SwaggerParameter("ID of the animal to treat", Required = true)]
        Guid id)
    {
        var animal = await _animalRepo.GetByIdAsync(AnimalId.From(id));
        animal.Treat();
        await _animalRepo.UpdateAsync(animal);
        return NoContent();
    }

    /// <summary>
    /// Delete an animal by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary     = "Delete animal",
        Description = "Deletes the specified animal.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Animal deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Animal not found")]
    public async Task<IActionResult> Delete(
        [SwaggerParameter("ID of the animal to delete", Required = true)]
        Guid id)
    {
        var animal = await _animalRepo.GetByIdAsync(AnimalId.From(id));
        await _animalRepo.RemoveAsync(animal);
        return NoContent();
    }

    /// <summary>
    /// Transfer an animal to another enclosure.
    /// </summary>
    [HttpPost("{id:guid}/transfer")]
    [SwaggerOperation(
        Summary     = "Transfer animal",
        Description = "Moves the specified animal from its current enclosure to the target enclosure.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Animal transferred")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid operation")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Animal or enclosure not found")]
    public async Task<IActionResult> Transfer(
        [SwaggerParameter("ID of the animal to transfer", Required = true)]
        Guid id,
        [FromQuery, SwaggerParameter("ID of the target enclosure", Required = true)]
        Guid targetEnclosureId)
    {
        await _transferService.TransferAsync(
            AnimalId.From(id),
            EnclosureId.From(targetEnclosureId)
        );
        return NoContent();
    }

    /// <summary>
    /// Helper: try to assign an animal to an enclosure, ignoring errors if full or not found.
    /// </summary>
    private async Task TryAssignToEnclosure(Animal animal, EnclosureId encId)
    {
        try
        {
            var enclosure = await _enclosureRepo.GetByIdAsync(encId);
            if (enclosure.Animals.Count >= enclosure.Capacity)
                return;

            enclosure.AddAnimal(animal);
            await _enclosureRepo.UpdateAsync(enclosure);

            animal.MoveTo(encId);
            await _animalRepo.UpdateAsync(animal);
        }
        catch { }
    }
}
