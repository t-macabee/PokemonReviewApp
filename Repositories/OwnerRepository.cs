using PokemomReviewApp.Models;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private DataContext context;

        public OwnerRepository(DataContext context)
        {
            this.context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            context.Remove(owner);
            return Save();
        }

        public Owner GetOwnerById(int id)
        {
            return context.Owners.Where(x => x.Id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerofAPokemon(int id)
        {
            return context.PokemonOwners.Where(x => x.Pokemon.Id == id).Select(y => y.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return context.Owners.OrderBy(x => x.Id).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int id)
        {
            return context.PokemonOwners.Where(x => x.Owner.Id == id).Select(y => y.Pokemon).ToList();
        }

        public bool OwnerExists(int id)
        {
            return context.Owners.Any(x => x.Id == id);
        }

        public bool Save()
        {
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            context.Update(owner);
            return Save();
        }
    }
}
