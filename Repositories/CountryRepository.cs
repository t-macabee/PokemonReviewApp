using PokemomReviewApp.Models;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using System.ComponentModel;

namespace PokemonReviewApp.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private DataContext context;

        public CountryRepository(DataContext context)
        {
            this.context = context;
        }

        public bool CountryExists(int id)
        {
            return context.Countries.Any(x => x.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            context.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return context.Countries.ToList();
        }

        public Country GetCountryById(int id)
        {
            return context.Countries.Where(x => x.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int id)
        {
            return context.Owners.Where(x => x.Id == id).Select(y => y.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int id)
        {
            return context.Owners.Where(x => x.Country.Id == id).ToList();
        }

        public bool Save()
        {
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            context.Update(country);
            return Save();
        }
    }
}
