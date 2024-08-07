namespace PokemonApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
        bool CategoryExists(int id);
        bool CategoryUpdate(Category category);
        bool CategoryDelete(Category category);
        bool CreateCategory(Category category);
        bool Save();


    }
}
