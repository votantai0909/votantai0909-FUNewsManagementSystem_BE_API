using AutoMapper;
using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Models.ArticleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NewsArticleController : ControllerBase
    {
        private readonly INewsService _newsArticleService;
        private readonly IMapper _mapper;

        public NewsArticleController(INewsService service, IMapper mapper)
        {
            _newsArticleService = service;
            _mapper = mapper;
        }

        [Authorize(Roles = "2")]
        [HttpGet("paged")]
        public async Task<IActionResult> GetAll(int index = 0, int pageSize = 10)
        {
            var articles = await _newsArticleService.GetNewsArticles(index, pageSize);
            var dtos = _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var article = await _newsArticleService.GetNewsArticleById(id);
            if (article == null) return NotFound();
            var dto = _mapper.Map<NewsArticleDto>(article);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNewsArticleDto dto)
        {
            var article = _mapper.Map<NewsArticle>(dto);
            var result = await _newsArticleService.CreateNewsArticle(article, dto.TagIds.ToArray());
            return result ? Ok("Created successfully") : BadRequest("Creation failed");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateNewsArticleDto dto)
        {
            var article = _mapper.Map<NewsArticle>(dto);
            var result = await _newsArticleService.UpdateNewsArticle(article, dto.TagIds.ToArray());
            return result ? Ok("Updated successfully") : BadRequest("Update failed");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _newsArticleService.DeleteNewsArticleAsync(id);
            return Ok("Deleted (soft) successfully");
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetByStatus(string status, int index = 0, int pageSize = 10)
        {
            var articles = await _newsArticleService.GetNewsArticlesByStatus(index, pageSize, status);
            var dtos = _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
            return Ok(dtos);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(short userId)
        {
            var articles = await _newsArticleService.GetNewsArticlesByUser(userId);
            var dtos = _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
            return Ok(dtos);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stats = await _newsArticleService.GetNewsStatistics(startDate, endDate);
            return Ok(stats);
        }
    }
}
