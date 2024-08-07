using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace PokemonApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{

 
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IPokemonRepository _pokemonRepository;

    public ReviewController(IReviewRepository reviewRepository, IMapper mapper,
        IReviewerRepository reviewerRepository , IPokemonRepository pokemonRepository )
    {

        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _reviewerRepository = reviewerRepository;
        _pokemonRepository = pokemonRepository;
    }

    [HttpPost]
    public IActionResult CreateReview([FromBody]ReviewDto reviewCreate, [FromQuery] int reviewerId , [FromQuery] int pokemonId)

    {
        if (reviewCreate == null)
        {
            throw new InvalidOperationException("Review is null");
        }

        Reviewer? reviewerEntity = _reviewerRepository.GetReviewers().FirstOrDefault(r=> r is not null && r.Id == reviewerId);
        Pokemon? pokemonEntity = _pokemonRepository.GetPokemonId(pokemonId);

        var review = _mapper.Map<Review>(reviewCreate);

        review.Pokemon = pokemonEntity;
        review.Reviewer = reviewerEntity;
        if(!_reviewRepository.CreateReview(review))
        {
            ModelState.AddModelError("", " something went wrong while saving");
            return BadRequest(ModelState);
        }
        return Created();
        
    }

    [HttpGet]
    public IActionResult GetReviews()
    {

        List<ReviewDto> reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

        if (!reviews.Any())
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviews);

    }

    [HttpGet("reviewid/{reveiwId}")]
    public IActionResult GetReview(int reveiwId)
    {
        if(!_reviewRepository.ReviewExists(reveiwId))
            return NotFound();

        ReviewDto review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reveiwId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(review);

    }

    [HttpGet("pokemonId/{pokomonId}")]
    public IActionResult GetReviewOfPokemon(int pokomonId)
    {

        List<ReviewDto> reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewOfPokemon(pokomonId));

        if (!reviews.Any())
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviews);

    }



}
