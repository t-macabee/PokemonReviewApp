using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemomReviewApp.Controllers;
using PokemomReviewApp.Models;
using PokemonReviewApp.DTOs;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Controllers
{
    public class CategoryController : BaseAPIController
    {
        private readonly ICategoryRepository categoryRepository;
        private IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var result = mapper.Map<List<CategoryDto>>(categoryRepository.GetCategories());
            return Ok(result);
        }

        [HttpGet("id")]
        public IActionResult GetCategoryById(int id)
        {
            var result = mapper.Map<CategoryDto>(categoryRepository.GetCategoryById(id));
            return Ok(result);
        }

        [HttpGet("categoryId")]
        public IActionResult GetPokemonByCategory(int id)
        {
            var result = mapper.Map<List<PokemonDto>>(categoryRepository.GetPokemonByCategory(id));
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody]CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                throw new Exception("Field is empty!");

            var category = categoryRepository.GetCategories()
                .Where(x => x.Name.ToLower() == categoryCreate.Name.ToLower())
                .FirstOrDefault();
            if(category != null)
                throw new Exception("Category already exists!");

            var categoryMap = mapper.Map<Category>(categoryCreate);
            if (!categoryRepository.CreateCategory(categoryMap))
                throw new Exception("Something went wrong while saving");
            
            return Ok("Success!");
        }

        [HttpPut("id")]
        public IActionResult UpdateCategory(int id, [FromBody]CategoryDto updatedCategory) 
        {
            if (updatedCategory == null)
                throw new Exception("Field is empty!");

            if (id != updatedCategory.Id)
                throw new Exception("ID mismatch!");

            if(!categoryRepository.CategoryExists(id))
                throw new Exception("Category not found!");

            var categoryMap = mapper.Map<Category>(updatedCategory);

            if (!categoryRepository.UpdateCategory(categoryMap))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }

        [HttpDelete("id")]
        public IActionResult DeleteCategory(int id)
        {
            if (!categoryRepository.CategoryExists(id))
                throw new Exception("Category not found!");

            var categoryToDelete = categoryRepository.GetCategoryById(id);

            if(!categoryRepository.DeleteCategory(categoryToDelete))
                throw new Exception("Something went wrong while saving");

            return NoContent();
        }
    }
}
