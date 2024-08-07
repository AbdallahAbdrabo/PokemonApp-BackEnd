

using PokemonApp.Interfaces;
using PokemonApp.Repositories;

namespace PokemonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController( IPokemonRepository pokemonRepository , IMapper mapper , IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        [HttpPost]
        public IActionResult PokemonCreate([FromBody] PokemonDto? pokemonCreate , [FromQuery] int categoryId , [FromQuery] int ownerId)
        {
            if (pokemonCreate == null || ownerId == 0 || categoryId == 0) return BadRequest();

            Pokemon? pokemon = _pokemonRepository.GetAll().FirstOrDefault(p => p.Name is not null && p.Name.Trim().ToUpper().Equals(pokemonCreate?.Name?.TrimEnd().ToUpper()));

            if(pokemon is not null)
            {
                ModelState.AddModelError("", " Pokemon Already Exists");
                return BadRequest(ModelState);
            }

            Pokemon pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", " something went wrong while saving");
                return BadRequest(ModelState);
            }
            return Created();

        }


        [HttpGet]
        public IActionResult GetPokemons()
        {
            List<PokemonDto>? pokemons =  _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetAll());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
        [HttpGet("{PokemonId}")]
        public IActionResult GetPokemonId(int PokemonId)
        {
            PokemonDto? pokemon  = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemonId(PokemonId));
            if (pokemon == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("name/{PokemonName}")]
        public IActionResult GetPokemonName(string PokemonName)
        {
            PokemonDto? pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPogkemonName(PokemonName));
            if (pokemon == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("Rating/{id}")]
        public IActionResult GetPokemonRating(int id)
        {
            
            decimal rating = _pokemonRepository.GetPokemonRating(id);
            return Ok(rating);
        }

        [HttpPut]
        public IActionResult PokemonUpdate([FromBody] PokemonDto pokemonUpdate)
        {
            if (pokemonUpdate == null)
            {
                return BadRequest();
            }



            if (!_pokemonRepository.PokemonExists(pokemonUpdate.Id))
            {
                ModelState.AddModelError("", " Country is not Exist");
                return BadRequest(ModelState);

            }
            var pokemonMap = _mapper.Map<Pokemon>(pokemonUpdate);

            if (!_pokemonRepository.PokemonUpdate(pokemonMap))
            {
                ModelState.AddModelError("", " something went wrong while saving");
                return BadRequest(ModelState);
            }

            return Ok(pokemonMap);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            if (!_pokemonRepository.PokemonExists(id))
            {
                return NotFound();
            }

            Pokemon? pokemon = _pokemonRepository.GetPokemonId(id);
            var reviews = _reviewRepository.GetReviewOfPokemon(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewRepository.ReviewDelete(reviews.ToList()))
            {
                ModelState.AddModelError("", " something went wrong while delete  ");
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonDelete(pokemon))
            {
                ModelState.AddModelError("", " something went wrong while delete  ");
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }


    }
}
