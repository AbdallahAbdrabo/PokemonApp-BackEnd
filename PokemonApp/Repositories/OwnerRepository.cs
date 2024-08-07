
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokemonApp.Modules;

namespace PokemonApp.Repositories
{
    public class OwnerRepository : IOwnerRepository
        
    {
        private readonly DataContext _dataContext;

        public OwnerRepository( DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public Owner GetOwner(int ownerId)
        {
           Owner? owner= _dataContext.Owners.FirstOrDefault(owner =>  owner.Id == ownerId);
            if (owner == null)
            {
                throw new InvalidOperationException("ownr is null");
            }
            return owner ;
        }

        public ICollection<Owner> GetOwners()
        {
            List<Owner> owners = _dataContext.Owners.ToList();

            return owners;
        }

        public ICollection<Owner?> GetOwnersofAPokemon(int pokemonId)
        {
            
            ICollection<Owner?>? owners = _dataContext.PokemonOwners
                .Where(po=> po.PokemonId == pokemonId).Select(pc=> pc.Owner)
                .ToList();

            if (owners == null)
            {
                throw new InvalidOperationException("ownr is null");
            }

            return owners;
        }

      
        public ICollection<Pokemon?> GetPokmonByOwner(int ownerId)
        {
        

            ICollection<Pokemon?> pokemons = _dataContext.PokemonOwners
            .Where(po => po.OwnerId == ownerId).Select(pc => pc.Pokemon)
            .ToList();
            if (pokemons == null || pokemons.Count == 0 )
            {
                throw new InvalidOperationException("ownr is null");
            }
            return pokemons;

        }

        public bool OwnerCreate(Owner owner )
        {
           _dataContext.Add(owner);
            return save();
        }

        public bool OwnerDelete(Owner owner)
        {
            _dataContext.Remove(owner);
            return save();
        }

        public bool OwnerExists(int ownerId)
        {
            return _dataContext.Owners.Any(o => o.Id == ownerId);
        }

        public bool OwnerUpdate(Owner owner)
        {
            _dataContext.Update(owner);
            return save();

        }

        public bool save()
        {
            int saved = _dataContext.SaveChanges();
            return saved > 0;
        }
    }
}
