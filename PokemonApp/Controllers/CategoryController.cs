using Microsoft.AspNetCore.Mvc;
using PokemonApp.Interfaces;
using System.Runtime.InteropServices.Marshalling;

namespace PokemonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate is null)
                return BadRequest(ModelState);
            Category? category = _categoryRepository.GetCategories()
                                    .Where(c => c.Name != null && c.Name.Trim().ToUpper().Equals(categoryCreate.Name?.TrimEnd().ToUpper()))
                                    .FirstOrDefault();


            if (category is not null)
            {
                ModelState.AddModelError("", "Category already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return BadRequest(ModelState);
            }

            return Created();
        }

        [HttpGet]
        public IActionResult GetCategory()
        {
            ICollection<CategoryDto>? categories =
                _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }
        [HttpGet("category/{categoryid}")]
        public IActionResult GetCategory(int categoryid)
        {
            if (!_categoryRepository.CategoryExists(categoryid)) {
                return NotFound();
            }
            CategoryDto? category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryid));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }
        [HttpGet("pokemonbycategoryid/{categoryid}")]

        public IActionResult GetPokemonsByCategory(int categoryid)
        {
            List<PokemonDto> pokemon = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryid));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);

        }

        [HttpPut]
        public IActionResult CategoryUpdate([FromBody] CategoryDto categoryUpdate)
        {
            if (categoryUpdate is null)
            {
                ModelState.AddModelError("", " category is null");
                return BadRequest(ModelState);
            }

            var ctegory = _categoryRepository.GetCategory(categoryUpdate.Id);

            if (ctegory is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Category? categoryMap = _mapper.Map<Category>(categoryUpdate);

            if (!_categoryRepository.CategoryUpdate(categoryMap))
            {

                ModelState.AddModelError("", " something  went wrong while update");
                return BadRequest(ModelState);

            }
            return Ok();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound();
            }

            var category = _categoryRepository.GetCategory(id);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryDelete(category))
            {
                ModelState.AddModelError("", " something went wrong while delete  ");
                return BadRequest(ModelState);
            }

            return Ok();
        }



    }
}
