namespace PokemonApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);

        Country GetCountrybyOnwner(int ownerId);
        ICollection<Owner> GetOwnersFromACounry( int CouuntryId );
        bool CountryExists( int id );
        bool CountryUpdate(Country country);
        bool CountryDelete(Country country);
        bool CountryCreate( Country country );
        bool save();
    }
}
