using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemomReviewApp.Controllers;
using PokemomReviewApp.Models;
using PokemonReviewApp.DTOs;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Controllers
{
    public class CountryController : BaseAPIController
    {
        private readonly ICountryRepository countryRepository;
        private IMapper mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;   
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            var result = mapper.Map<List<CountryDto>>(countryRepository.GetCountries());
            return Ok(result);
        }

        [HttpGet("id")]
        public IActionResult GetCountryById(int id)
        {
            var result = mapper.Map<CountryDto>(countryRepository.GetCountryById(id));
            return Ok(result);
        }

        [HttpGet("ownerId")]
        public IActionResult GetCountryByOwner(int id)
        {
            var result = mapper.Map<CountryDto>(countryRepository.GetCountryByOwner(id));
            return Ok(result);
        }

        [HttpGet("ownersfromCountryId")]
        public IActionResult GetOwnersFromACountry(int id)
        {
            var result = mapper.Map<List<OwnerDto>>(countryRepository.GetOwnersFromACountry(id));
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                throw new Exception("Field is empty!");

            var category = countryRepository.GetCountries()
                .Where(x => x.Name.ToLower() == countryCreate.Name.ToLower())
                .FirstOrDefault();
            if (category != null)
                throw new Exception("Country already exists!");

            var categoryMap = mapper.Map<Country>(countryCreate);
            if (!countryRepository.CreateCountry(categoryMap))
                throw new Exception("Something went wrong while saving");

            return Ok("Success!");
        }

        [HttpPut("id")]
        public IActionResult UpdateCountry(int id, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                throw new Exception("Field is empty!");

            if (id != updatedCountry.Id)
                throw new Exception("ID mismatch!");

            if (!countryRepository.CountryExists(id))
                throw new Exception("Country not found!");

            var countryMap = mapper.Map<Country>(updatedCountry);

            if (!countryRepository.UpdateCountry(countryMap))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }

        [HttpDelete("id")]
        public IActionResult DeleteCountry(int id)
        {
            if (!countryRepository.CountryExists(id))
                throw new Exception("Count not found!");

            var countryToDelete = countryRepository.GetCountryById(id);

            if (!countryRepository.DeleteCountry(countryToDelete))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }
    }
}
