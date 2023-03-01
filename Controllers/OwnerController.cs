using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemomReviewApp.Controllers;
using PokemomReviewApp.Models;
using PokemonReviewApp.DTOs;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    public class OwnerController : BaseAPIController
    {
        private IOwnerRepository ownerRepository;
        private ICountryRepository countryRepository;
        private IMapper mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            this.ownerRepository = ownerRepository;
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetOwners()
        {
            var result = mapper.Map<List<OwnerDto>>(ownerRepository.GetOwners());
            return Ok(result);
        }

        [HttpGet("id")]
        public IActionResult GetOwner(int id) 
        {
            var result = mapper.Map<OwnerDto>(ownerRepository.GetOwnerById(id));
            return Ok(result);
        }

        [HttpGet("pokemonByOwner")]
        public IActionResult GetPokemonByOwner(int id)
        {
            var result = mapper.Map<List<PokemonDto>>(ownerRepository.GetPokemonByOwner(id));
            return Ok(result);
        }

        [HttpGet("ownerOfPokemon")]
        public IActionResult GetOwnerofAPokemon(int id)
        {
            var result = mapper.Map<List<OwnerDto>>(ownerRepository.GetOwnerofAPokemon(id));
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateOwner([FromQuery] int CountryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
                throw new Exception("Field is empty!");

            var owners = ownerRepository.GetOwners()
                .Where(x => x.FirstName.ToLower() == ownerCreate.FirstName.ToLower())
                .FirstOrDefault();
            if (owners != null)
                throw new Exception("Category already exists!");

            var ownerMap = mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = countryRepository.GetCountryById(CountryId);

            if (!ownerRepository.CreateOwner(ownerMap))
                throw new Exception("Something went wrong while saving");

            return Ok("Success!");
        }

        [HttpPut("id")]
        public IActionResult UpdateOwner(int id, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                throw new Exception("Field is empty!");

            if (id != updatedOwner.Id)
                throw new Exception("ID mismatch!");

            if (!ownerRepository.OwnerExists(id))
                throw new Exception("Category not found!");

            var ownerMap = mapper.Map<Owner>(updatedOwner);

            if (!ownerRepository.UpdateOwner(ownerMap))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }

        [HttpDelete("id")]
        public IActionResult DeleteOwner(int id)
        {
            if (!ownerRepository.OwnerExists(id))
                throw new Exception("Owner not found!");

            var ownerToDelete = ownerRepository.GetOwnerById(id);

            if (!ownerRepository.DeleteOwner(ownerToDelete))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }
    }
}
