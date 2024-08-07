namespace PokemonApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review? GetReview(int id);
        ICollection<Review> GetReviewOfPokemon(int pokemonId);
        bool ReviewExists(int reviewId);
        bool ReviewDelete(List<Review> review);
        bool CreateReview(Review review);
        bool save();

    }
}
