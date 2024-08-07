
using PokemonApp.data;
using PokemonApp.Modules;

namespace PokemonApp.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private  DataContext _dataContext;

        public CountryRepository( DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool CountryCreate(Country country)
        {
            _dataContext.Countries.Add(country);
            return save();
        }

        public bool CountryDelete(Country country)
        {
            _dataContext.Countries.Remove(country);
            return save();

        }

        public bool CountryExists(int id)
        {
            bool countryExists = _dataContext.Countries.Any(c => c.Id == id);
            return countryExists;
        }

        public bool CountryUpdate(Country country)
        {
            _dataContext.Countries.Update(country);
            return save();
        }

        public ICollection<Country> GetCountries()
        {
           ICollection<Country> countries= _dataContext.Countries.ToList();
            return countries;
        }

        public Country GetCountry(int id)
        {
            Country? country = _dataContext.Countries.FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                country = new Country();
            }
            return country;
        }

        public Country GetCountrybyOnwner(int ownerId)
        {
            Country? country = _dataContext.Owners
                .Where(owner => owner.Id == ownerId)
                .Select(c => c.Country).FirstOrDefault();
            if (country == null)
            {
                country = new Country();
            }
            return country;
        }

        public ICollection<Owner> GetOwnersFromACounry(int CouuntryId)
        {
            ICollection<Owner> owners = _dataContext.Owners
                .Where(o=> o.Country.Id == CouuntryId).ToList();

           
            return owners;
        }

        public bool save()
        {
            int saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
