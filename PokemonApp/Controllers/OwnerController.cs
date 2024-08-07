
namespace PokemonApp.Controllers;

[Route("apu/[controller]")]
[ApiController]
public class OwnerController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly ICountryRepository _countryRepository;

    public OwnerController( IOwnerRepository ownerRepository, IMapper mapper , ICountryRepository countryRepository)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _countryRepository = countryRepository;
    }

    [HttpPost]
    public IActionResult OwnerCreate([FromQuery] int  id  , [FromBody] OwnerDto ownerCrate)
    {
        if (ownerCrate is null)
            return BadRequest(ModelState);

        Owner? owner = _ownerRepository.GetOwners()
                             .Where(o => o.FirstName is not null && o.FirstName.Trim().ToUpper().Equals(ownerCrate.FirstName?.TrimEnd().ToUpper()))
                               .FirstOrDefault();


        if (owner is not null)
        {
            ModelState.AddModelError("", "owner already Exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ownerMap = _mapper.Map<Owner>(ownerCrate);
        ownerMap.Country = _countryRepository.GetCountry(id);
        if (!_ownerRepository.OwnerCreate(ownerMap))
        {
            ModelState.AddModelError("", "something went wrong while saving");
            return BadRequest(ModelState);
        }

        return Created();
    }


    [HttpGet]
    public IActionResult GetOwner()
    {
        var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
        if(!owners.Any())
            return NotFound();

        if (!ModelState.IsValid)
        {
           return BadRequest(ModelState);
        }



        return Ok(owners);
    }

    [HttpGet("owner/{ownerid}")]
    public IActionResult GetOwner(int ownerid)
    {
        if(!_ownerRepository.OwnerExists(ownerid))
            return NotFound();

        OwnerDto? owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerid));
        
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owner);

    }

    [HttpGet("pokemonid/{pokemonId}")]

    public IActionResult GetOwnersofAPokemon(int pokemonId)
    {
        var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnersofAPokemon(pokemonId));

        if(!owners.Any())
            return NotFound();

        if (!ModelState.IsValid) { 
        return BadRequest(ModelState);}

        return Ok(owners);
    }
    [HttpGet("pokebyowner/{ownerId}")]
    public IActionResult GetPokmonByOwner(int ownerId)
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokmonByOwner(ownerId));

        if (!pokemons.Any())
            return NotFound();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(pokemons);
    }
    [HttpPut]
    public IActionResult OwnerUpdate([FromBody] OwnerDto owenrUpdate)
    {
        if (owenrUpdate == null)
        {
            return BadRequest();
        }



        if (!_ownerRepository.OwnerExists(owenrUpdate.Id))
        {
            ModelState.AddModelError("", " Country is not Exist");
            return BadRequest(ModelState);

        }
        Owner? ownerMap = _mapper.Map<Owner>(owenrUpdate);

        if (!_ownerRepository.OwnerUpdate(ownerMap))
           
        {
            ModelState.AddModelError("", " something went wrong while saving");
            return BadRequest(ModelState);
        }

        return Ok(ownerMap);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {

        if (!_ownerRepository.OwnerExists(id))
        {
            return NotFound();
        }

        Owner? owner = _ownerRepository.GetOwner(id);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!_ownerRepository.OwnerDelete(owner))
        {
            ModelState.AddModelError("", " something went wrong while delete  ");
            return BadRequest(ModelState);
        }

        return Ok(owner);
    }
}

