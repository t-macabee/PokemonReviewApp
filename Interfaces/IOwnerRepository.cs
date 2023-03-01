using PokemomReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwnerById(int id);
        ICollection<Owner> GetOwnerofAPokemon(int id);
        ICollection<Pokemon> GetPokemonByOwner(int id);
        bool OwnerExists(int id);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}
