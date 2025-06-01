using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Models.CategoriesDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int index = 0, [FromQuery] int pageSize = 10)
        {
            var result = await _categoryService.GetCategoriesAsync(index, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var success = await _categoryService.CreateCategoryAsync(dto);
            return success ? Ok("Created successfully.") : BadRequest("Create failed.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(short id, [FromBody] UpdateCategoryDto dto)
        {
            if (id != dto.CategoryId) return BadRequest("ID mismatch.");
            var success = await _categoryService.UpdateCategoryAsync(dto);
            return success ? Ok("Updated successfully.") : NotFound("Category not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            return success ? Ok("Deleted successfully.") : BadRequest("Cannot delete. Category may not exist or is in use.");
        }
    }
}
