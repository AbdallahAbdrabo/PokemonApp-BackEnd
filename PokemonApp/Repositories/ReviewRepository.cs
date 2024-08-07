
namespace PokemonApp.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _dataContext;

    public ReviewRepository( DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public bool ReviewDelete(List<Review> review)
    {
        _dataContext.RemoveRange(review);
        return save();
    }

    public bool CreateReview(Review review)
    {
        _dataContext.Reviews.Add(review);
        return save();
    }

    public Review? GetReview(int id)
    {
        Review? review = _dataContext.Reviews.FirstOrDefault(r => r.Id == id);

        return review;
    }

    public ICollection<Review> GetReviewOfPokemon(int pokemonId)
    {
       List<Review>? reviews = _dataContext.Reviews
            .Where(o=>  o.Pokemon  != null && o.Pokemon.Id == pokemonId)
            .ToList();

        return reviews;
    }

    public ICollection<Review> GetReviews()
    {
       List<Review> reviews = _dataContext.Reviews.ToList();

        return reviews;
    }

    public bool ReviewExists(int reviewId)
    {
        return _dataContext.Reviews.Any(r => r.Id == reviewId);
    }

    public bool save()
    {
        int saved = _dataContext.SaveChanges();
        return saved > 0;
    }
}
