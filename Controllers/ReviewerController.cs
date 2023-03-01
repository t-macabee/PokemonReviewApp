using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemomReviewApp.Controllers;
using PokemomReviewApp.Models;
using PokemonReviewApp.DTOs;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    public class ReviewerController : BaseAPIController
    {
        private IReviewerRepository reviewerRepository;
        private IMapper mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            this.reviewerRepository = reviewerRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetReviewers()
        {
            var result = mapper.Map<List<ReviewerDto>>(reviewerRepository.GetReviewers());
            return Ok(result);
        }

        [HttpGet("id")]
        public IActionResult GetReviewer(int id)
        {
            var result = mapper.Map<ReviewerDto>(reviewerRepository.GetReviewer(id));
            return Ok(result);
        }

        [HttpGet("reviewByReviewers")]
        public IActionResult GetReviewsByReviewer(int id)
        {
            var result = mapper.Map<List<ReviewDto>>(reviewerRepository.GetReviewsByReviewer(id));
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreatePokemon([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                throw new Exception("Field is empty!");

            var reviewers = reviewerRepository.GetReviewers()
                .Where(x => x.LastName.ToLower() == reviewerCreate.LastName.ToLower())
                .FirstOrDefault();

            if (reviewers != null)
                throw new Exception("Reviewer already exists!");

            var reviewerMap = mapper.Map<Reviewer>(reviewerCreate);

            if (!reviewerRepository.CreateReviewer(reviewerMap))
                throw new Exception("Something went wrong while saving");

            return Ok("Success!");
        }

        [HttpPut("id")]
        public IActionResult UpdateReviewer(int id, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                throw new Exception("Field is empty!");

            if (id != updatedReviewer.Id)
                throw new Exception("ID mismatch!");

            if (!reviewerRepository.ReviewerExists(id))
                throw new Exception("Category not found!");

            var reviewerMap = mapper.Map<Reviewer>(updatedReviewer);

            if (!reviewerRepository.UpdateReviewer(reviewerMap))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }

        [HttpDelete("id")]
        public IActionResult DeleteReviewer(int id)
        {
            if (!reviewerRepository.ReviewerExists(id))
                throw new Exception("Reviewer not found!");

            var reviewerToDelete = reviewerRepository.GetReviewer(id);            

            if (!reviewerRepository.DeleteReviewer(reviewerToDelete))
                ModelState.AddModelError("", "Something went wrong when deleting reviewers");

            return NoContent();
        }
    }
}
