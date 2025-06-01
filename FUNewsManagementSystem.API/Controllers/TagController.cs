using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Models.TagDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1")]

    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetAllPaged(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize <= 0)
                return BadRequest("Page must be >= 1 and pageSize > 0");

            var result = await _tagService.GetAllPagedAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tagService.GetByIdAsync(id);
            if (result == null) return NotFound("Tag not found");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagDto dto)
        {
            var success = await _tagService.CreateAsync(dto);
            return success ? Ok("Tag created successfully") : BadRequest("Failed to create tag");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTagDto dto)
        {
            var success = await _tagService.UpdateAsync(dto);
            return success ? Ok("Tag updated successfully") : NotFound("Tag not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _tagService.DeleteAsync(id);
            return success ? Ok("Tag deleted successfully") : NotFound("Tag not found");
        }
    }
}
