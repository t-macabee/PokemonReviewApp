using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemomReviewApp.Controllers;
using PokemomReviewApp.Models;
using PokemonReviewApp.DTOs;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    public class PokemonController : BaseAPIController
    {
        private IPokemonRepository pokemonRepository;
        private IReviewRepository reviewRepository;
        private IMapper mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            this.pokemonRepository = pokemonRepository;
            this.reviewRepository = reviewRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPokemons()
        {
            var result = mapper.Map<List<PokemonDto>>(pokemonRepository.GetPokemons());
            return Ok(result);
        }

        [HttpGet("id")]
        public IActionResult GetPokemonById(int id) 
        {
            if (!pokemonRepository.PokemonExists(id))
                return NotFound();
            var result = mapper.Map<PokemonDto>(pokemonRepository.GetPokemonById(id));
            return Ok(result);
        }

        [HttpGet("name")]
        public IActionResult GetPokemonByName(string name)
        {
            var result = mapper.Map<PokemonDto>(pokemonRepository.GetPokemonByName(name));
            return Ok(result);
        }

        [HttpGet("rating")]
        public IActionResult GetPokemonRating(int id)
        {
            if (!pokemonRepository.PokemonExists(id))
                return NotFound();
            var result = pokemonRepository.GetPokemonRating(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                throw new Exception("Field is empty!");

            var pokemons = pokemonRepository.GetPokemons()
                .Where(x => x.Name.ToLower() == pokemonCreate.Name.ToLower())
                .FirstOrDefault();
            if (pokemons != null)
                throw new Exception("Pokemon already exists!");

            var pokemonMap = mapper.Map<Pokemon>(pokemonCreate);            

            if (!pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
                throw new Exception("Something went wrong while saving");

            return Ok("Success!");
        }

        [HttpPut("id")]
        public IActionResult UpdatePokemon(int id, [FromQuery]int ownerId, [FromQuery]int categoryId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                throw new Exception("Field is empty!");

            if (id != updatedPokemon.Id)
                throw new Exception("ID mismatch!");

            if (!pokemonRepository.PokemonExists(id))
                throw new Exception("Category not found!");

            var pokemonMap = mapper.Map<Pokemon>(updatedPokemon);

            if (!pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }

        [HttpDelete("id")]
        public IActionResult DeletePokemon(int id)
        {
            if (!pokemonRepository.PokemonExists(id))
                throw new Exception("Pokemon not found!");

            var reviewsToDelete = reviewRepository.GetReviewsOfAPokemon(id);
            var pokemonToDelete = pokemonRepository.GetPokemonById(id);

            if (!reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
                ModelState.AddModelError("", "Something went wrong when deleting reviews");

            if (!pokemonRepository.DeletePokemon(pokemonToDelete))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }
    }
}
