using PokemomReviewApp.Models;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext context;

        public CategoryRepository(DataContext context)
        {
            this.context = context;
        }

        public bool CategoryExists(int id)
        {
            return context.Categories.Any(x => x.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return context.Categories.Where(x => x.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int id)
        {
            return context.PokemonCategories.Where(x => x.CategoryId == id).Select(y => y.Pokemon).ToList();
        }

        public bool Save()
        {
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            context.Update(category);
            return Save();
        }
    }
}
