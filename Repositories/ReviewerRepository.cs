using Microsoft.EntityFrameworkCore;
using PokemomReviewApp.Models;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using System.Threading.Tasks.Sources;

namespace PokemonReviewApp.Repositories
{
    public class ReviewerRepository : IReviewerRepository
    {
        private DataContext context;

        public ReviewerRepository(DataContext context)
        {
            this.context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            context.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            context.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int id)
        {
            return context.Reviewers.Where(x => x.Id == id).Include(y => y.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return context.Reviewers.OrderBy(x => x.Id).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int id)
        {
            return context.Reviews.Where(x => x.Reviewer.Id == id).ToList();
        }

        public bool ReviewerExists(int id)
        {
            return context.Reviewers.Any(x => x.Id == id);
        }

        public bool Save()
        {
            var result = context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            context.Update(reviewer);
            return Save();
        }
    }
}
