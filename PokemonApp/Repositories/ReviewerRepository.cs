
namespace PokemonApp.Repositories
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public ICollection<Reviewer> GetReviewers()
        {
            List<Reviewer> reviews = _dataContext.Reviewers.ToList();
         return reviews;
        }
    }
}
