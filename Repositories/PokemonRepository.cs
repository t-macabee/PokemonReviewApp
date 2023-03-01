using Microsoft.AspNetCore.Mvc;
using PokemomReviewApp.Models;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private DataContext context;

        public PokemonRepository(DataContext context)
        {
            this.context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = context.Owners.Where(x => x.Id == ownerId).FirstOrDefault();
            var category = context.Categories.Where(x => x.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };

            context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };

            context.Add(pokemonCategory);

            context.Add(pokemon);
            
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemonById(int id)
        {
            return context.Pokemon.Where(x => x.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemonByName(string name)
        {
            return context.Pokemon.Where(x => x.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int id)
        {
            var result = context.Reviews.Where(x => x.Id == id);

            if(result.Count() <= 0)
                return 0;

            return ((decimal)result.Sum(x => x.Rating) / result.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return context.Pokemon.OrderBy(x => x.Id).ToList();
        }

        public bool PokemonExists(int id)
        {
            return context.Pokemon.Any(x => x.Id == id);
        }

        public bool Save()
        {
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            context.Update(pokemon);
            return Save();
        }
    }
}
