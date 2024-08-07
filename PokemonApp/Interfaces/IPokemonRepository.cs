namespace PokemonApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetAll();
        Pokemon GetPokemonId(int id);
        Pokemon GetPogkemonName(string name);
        decimal GetPokemonRating(int id);
        bool PokemonExists(int id);
        bool PokemonUpdate(Pokemon pokemon);
        bool PokemonDelete(Pokemon pokemon);
        bool CreatePokemon(int ownerId , int CategoryId , Pokemon pokemon);
        bool save();
        
    }
}
