using PokemomReviewApp.Models;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using System.Reflection.Metadata;

namespace PokemonReviewApp.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private DataContext context;

        public ReviewRepository(DataContext context)
        {
            this.context = context;
        }

        public bool CreateReview(Review review)
        {
            context.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            context.RemoveRange(reviews);
            return Save();
        }

        public Review GetReview(int id)
        {
            return context.Reviews.Where(x => x.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return context.Reviews.OrderBy(x => x.Id).ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int id)
        {
            return context.Reviews.Where(x => x.Pokemon.Id == id).ToList();    
        }

        public bool ReviewExists(int id)
        {
            return context.Reviews.Any(x => x.Id == id);
        }

        public bool Save()
        {
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            context.Update(review);
            return Save();
        }
    }
}
