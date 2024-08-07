

using Microsoft.AspNetCore.Http.HttpResults;
using PokemonApp.Interfaces;
using PokemonApp.Modules;

namespace PokemonApp.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private  DataContext _dataContext;

    public CategoryRepository( DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public bool CategoryDelete(Category category)
    {
        _dataContext.Ctegories.Remove(category);
        return Save();
    }

    public bool CategoryExists(int id)
    {
        return _dataContext.Ctegories.Any(c => c.Id == id);
    }

    public bool CategoryUpdate(Category category)
    {

      
        _dataContext.Ctegories.Update(category);
        return Save();
    }

    public bool CreateCategory(Category category)
    {
     
       _dataContext.Ctegories.Add(category);
        
       
        return Save() ; 
    }

    public ICollection<Category> GetCategories()
    {
        ICollection<Category> categories =
            _dataContext.Ctegories.ToList();
        return categories;
    }

    public Category GetCategory(int id)
    {
       Category? catrgory = _dataContext.Ctegories.AsNoTracking().FirstOrDefault(c => c.Id == id);
        if (catrgory is null)
        {
            return new Category(); 
        }
        return catrgory ;
    }

    public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
    {
        ICollection<Pokemon?>? pokemons = 
            _dataContext.PokemonCategories.Where(pc => pc.CategoryId == categoryId)
            .Select(pc => pc.Pokemon).ToList();
       
        return pokemons ;
    }

    public bool Save()
    {
        int saved = _dataContext.SaveChanges();
        return saved > 0 ? true :false ;
    }
}
