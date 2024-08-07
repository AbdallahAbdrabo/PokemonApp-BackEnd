using Microsoft.AspNetCore.Mvc;
using PokemonApp.Modules;

namespace PokemonApp.Controllers;

[Route("api/[controller]")]
public class CountryController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController( ICountryRepository countryRepository , IMapper mapper)
    {
        _countryRepository = countryRepository;

        _mapper = mapper;
    }


    [HttpPost]
    public IActionResult CountryCreate([FromBody] CountryDto countryCreate)
    {
        if (countryCreate == null)
        {
            return BadRequest();
        }

        Country? country = _countryRepository.GetCountries()
            .Where(c => c.Name != null && c.Name.Trim().ToUpper().Equals(countryCreate.Name?.TrimEnd().ToUpper()))
            .FirstOrDefault();

        if (country is not  null)
        {
            ModelState.AddModelError("", " Country Already Exist");
            return BadRequest(ModelState);

        }
       var countryMap = _mapper.Map<Country>(countryCreate);
        if (!_countryRepository.CountryCreate(countryMap)) {
            ModelState.AddModelError("", " something went wrong while saving");
            return BadRequest(ModelState);
        }

        return Ok(countryMap);
    }

    [HttpGet]
    public IActionResult GetCountried() {

        List<CountryDto>? countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(countries);


    }
    [HttpGet("country/{countryId}")]
    public IActionResult GetCountry(int countryId) {

        if(!_countryRepository.CountryExists(countryId))
        {
            return NotFound();
        }

        CountryDto? country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(country);


    }

    [HttpGet("owmerId")]
    public IActionResult GetCountrybyOnwner(int ownerId)
    {
        CountryDto country = _mapper.Map<CountryDto>(_countryRepository.GetCountrybyOnwner(ownerId));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(country);
    }

    [HttpGet("ownerbycountry/{countryId}")]
    public IActionResult GetOwnersFromACounry(int countryId)
    {

        if (!_countryRepository.CountryExists(countryId))
        {
            return NotFound();
        }

        List<OwnerDto> country = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersFromACounry(countryId));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(country);


    }
    [HttpPut]
    public IActionResult CountryUpdate([FromBody] CountryDto countryUpdate)
    {
        if (countryUpdate == null)
        {
            return BadRequest();
        }

        

        if (!_countryRepository.CountryExists(countryUpdate.Id))
        {
            ModelState.AddModelError("", " Country is not Exist");
            return BadRequest(ModelState);

        }
        var countryMap = _mapper.Map<Country>(countryUpdate);

        if (!_countryRepository.CountryUpdate(countryMap))
        {
            ModelState.AddModelError("", " something went wrong while saving");
            return BadRequest(ModelState);
        }

        return Ok(countryMap);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {

        if (!_countryRepository.CountryExists(id))
        {
            return NotFound();
        }

        var category = _countryRepository.GetCountry(id);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!_countryRepository.CountryDelete(category))
        {
            ModelState.AddModelError("", " something went wrong while delete  ");
            return BadRequest(ModelState);
        }

        return Ok();
    }

}