namespace PokemonApp.Interfaces;

public interface IOwnerRepository
{
    ICollection<Owner> GetOwners();

    Owner GetOwner(int ownerId);
    ICollection<Owner?> GetOwnersofAPokemon(int pokemonId);
    ICollection<Pokemon?> GetPokmonByOwner(int ownerId);
    bool OwnerExists(int ownerId);
    bool OwnerUpdate(Owner owner);
    bool OwnerDelete(Owner owner);
    bool OwnerCreate( Owner owner );
    bool save();
}
