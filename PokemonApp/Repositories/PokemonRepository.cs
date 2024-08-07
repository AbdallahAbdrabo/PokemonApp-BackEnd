

namespace PokemonApp.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _dataContext;

        public PokemonRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            Owner? pokemonOwnerEntity = _dataContext.Owners.FirstOrDefault(o  => o.Id == ownerId);

            Category? pokemonCategoryEntity = _dataContext.Ctegories.FirstOrDefault(c => c.Id == categoryId);

            if (pokemonOwnerEntity is null && pokemonCategoryEntity is null) {
                return false;
                    }

            PokemonOwner? pokemonowner = new()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,
            };
            _dataContext.Add(pokemonowner);

            PokemonCategory? pokemonCategory = new()
            {
                category = pokemonCategoryEntity,
                Pokemon = pokemon,
            };

            _dataContext.Add(pokemonCategory);
            _dataContext.Add(pokemon);
            return save();
        }

        public ICollection<Pokemon> GetAll()
        {
            List<Pokemon> pokemons = _dataContext.Pokemons.OrderBy(pokemons => pokemons.Id ).ToList();
            return pokemons;
        }

        public Pokemon GetPogkemonName(string name)
        {
            Pokemon? pokemon = _dataContext.Pokemons.Where(pokemon => pokemon.Name == name).FirstOrDefault();

            if (pokemon == null)
            {
                throw new KeyNotFoundException($"No Pokemon found with name {name}");
            }

            return pokemon;
        }

        public Pokemon GetPokemonId(int id)
        {
            Pokemon? pokemon = _dataContext.Pokemons.FirstOrDefault(pokemon => pokemon.Id == id);

            if (pokemon == null)
            {
                throw new KeyNotFoundException($"No Pokemon found with ID {id}");
            }

            return pokemon;
        }

        public decimal GetPokemonRating(int id)
        {
           List<Review>? review = _dataContext.Reviews.Where(review => review.Pokemon != null && review.Pokemon.Id == id).ToList();
            
            if(review.Count()<0)
                return 0;
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public bool PokemonDelete(Pokemon pokemon)
        {
            _dataContext.Remove(pokemon);
            return save();
        }

        public bool PokemonExists(int id)
        {
            return _dataContext.Pokemons.Any(p => p.Id == id);
        }

        public bool PokemonUpdate(Pokemon pokemon)
        {
            _dataContext.Update(pokemon);
            return save();
        }

        public bool save()
        {
           int saved = _dataContext.SaveChanges();
            return saved > 0;
        }
    }
}
