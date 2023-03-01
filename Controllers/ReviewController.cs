using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemomReviewApp.Controllers;
using PokemomReviewApp.Models;
using PokemonReviewApp.Data;
using PokemonReviewApp.DTOs;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    public class ReviewController : BaseAPIController
    {
        private IReviewRepository reviewRepository;
        private IReviewerRepository reviewerRepository;
        private IPokemonRepository pokemonRepository;
        private IMapper mapper;

        public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
        {
            this.reviewRepository = reviewRepository;
            this.reviewerRepository = reviewerRepository;
            this.pokemonRepository = pokemonRepository;
            this.mapper = mapper;            
        }

        [HttpGet]
        public IActionResult GetReviews()
        {
            var result = mapper.Map<List<ReviewDto>>(reviewRepository.GetReviews());
            return Ok(result);
        }

        [HttpGet("id")]
        public IActionResult GetReview(int id) 
        {
            var result = mapper.Map<ReviewDto>(reviewRepository.GetReview(id));
            return Ok(result);
        }

        [HttpGet("reviewsOfPokemon")]
        public IActionResult GetReviewsOfAPokemon(int id)
        {
            var result = mapper.Map<List<ReviewDto>>(reviewRepository.GetReviewsOfAPokemon(id));
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                throw new Exception("Field is empty!");

            var reviews = reviewRepository.GetReviews()
                .Where(x => x.Title.ToLower() == reviewCreate.Title.ToLower())
                .FirstOrDefault();

            if (reviews != null)
                throw new Exception("Review already exists!");

            var reviewMap = mapper.Map<Review>(reviewCreate);

            reviewMap.Pokemon = pokemonRepository.GetPokemonById(pokemonId);
            reviewMap.Reviewer = reviewerRepository.GetReviewer(reviewerId);

            if (!reviewRepository.CreateReview(reviewMap))
                throw new Exception("Something went wrong while saving");

            return Ok("Success!");
        }

        [HttpPut("id")]
        public IActionResult UpdateReview(int id, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                throw new Exception("Field is empty!");

            if (id != updatedReview.Id)
                throw new Exception("ID mismatch!");

            if (!reviewRepository.ReviewExists(id))
                throw new Exception("Category not found!");

            var reviewMap = mapper.Map<Review>(updatedReview);

            if (!reviewRepository.UpdateReview(reviewMap))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }

        [HttpDelete("id")]
        public IActionResult DeleteReview(int id)
        {
            if (!reviewRepository.ReviewExists(id))
                throw new Exception("Review not found!");

            var reviewToDelete = reviewRepository.GetReview(id);

            if (!reviewRepository.DeleteReview(reviewToDelete))
                ModelState.AddModelError("", "Something went wrong when deleting reviewers");

            return NoContent();
        }
    }
}
