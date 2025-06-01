using AutoMapper;
using FUNewsManagementSystem.Repositories;
using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Repositories.Interfaces;
using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Models.ArticleDTOs;
using FUNewsManagementSystem.Services.Models.LoginDTOs;
using Microsoft.EntityFrameworkCore;
using static FUNewsManagementSystem.Services.Services.NewsService;

namespace FUNewsManagementSystem.Services.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FUNewsManagementDbContext _context;

        public NewsService(IUnitOfWork unitOfWork, FUNewsManagementDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticles(int index, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<NewsArticle>();
            return await repository.GetAllArticlesAsync(index, pageSize);
        }

        public async Task<NewsArticle> GetNewsArticleById(string id)
        {
            var repository = _unitOfWork.GetRepository<NewsArticle>();
            return await repository.GetArticleByIdIncludeTagAsync(id, x => x.Tags, x => x.Category, x => x.CreatedBy);
        }

        public async Task<bool> CreateNewsArticle(NewsArticle newsArticle, int[] selectedTags)
        {
            var repository = _unitOfWork.GetRepository<NewsArticle>();
            var tagRepository = _unitOfWork.GetRepository<Tag>();

            // Check if a news article with the same ID already exists
            var existingArticle = await repository.FindAsync(na => na.NewsArticleId == newsArticle.NewsArticleId);
            if (existingArticle != null)
            {
                throw new Exception($"A news article with ID {newsArticle.NewsArticleId} already exists.");
            }


            // Get the list of tags from the database based on selectedTags
            var existingTags = await tagRepository.FindListAsync(t => selectedTags.Contains(t.TagId));

            if (existingTags == null || !existingTags.Any())
            {
                throw new Exception("Invalid or non-existent tags.");
            }

            // Assign the tags to the news article
            newsArticle.Tags = existingTags;

            newsArticle.NewsArticleId = await GenerateNewsArticleIdAsync();
            // Insert the news article into the database
            await repository.InsertAsync(newsArticle);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<string> GenerateNewsArticleIdAsync()
        {
            // Retrieve only the NewsArticleId column to optimize the query
            var articles = await _context.NewsArticles
                .Select(n => n.NewsArticleId)
                .ToListAsync(); // Fetch all data into memory

            // Filter out IDs that can be converted to integers
            var numericIds = articles
                .Where(id => int.TryParse(id, out _)) // Check if ID is a valid integer
                .Select(id => Convert.ToInt32(id))   // Convert valid IDs to integers
                .OrderByDescending(id => id)         // Sort in descending order
                .ToList();

            // Determine the next ID: If there are valid IDs, increment the highest one; otherwise, start from 1
            int newId = numericIds.Any() ? numericIds.First() + 1 : 1;

            return newId.ToString(); // Return the new ID as a string
        }

        public async Task<bool> UpdateNewsArticle(NewsArticle newsArticle, int[] selectedTags)
        {
            var repository = _unitOfWork.GetRepository<NewsArticle>();
            var tagRepository = _unitOfWork.GetRepository<Tag>();

            // Lấy bài báo từ DB kèm theo Tags
            var existingArticle = await _context.NewsArticles
                .Include(na => na.Tags)
                .FirstOrDefaultAsync(na => na.NewsArticleId == newsArticle.NewsArticleId);

            if (existingArticle == null)
            {
                throw new Exception("Bài báo không tồn tại.");
            }

            // Cập nhật thông tin bài báo
            existingArticle.NewsTitle = newsArticle.NewsTitle;
            existingArticle.Headline = newsArticle.Headline;
            existingArticle.NewsContent = newsArticle.NewsContent;
            existingArticle.NewsSource = newsArticle.NewsSource;
            existingArticle.CategoryId = newsArticle.CategoryId;
            existingArticle.NewsStatus = newsArticle.NewsStatus;
            existingArticle.CreatedById = newsArticle.CreatedById;
            existingArticle.UpdatedById = newsArticle.UpdatedById;
            existingArticle.ModifiedDate = DateTime.UtcNow;

            // **Xóa toàn bộ tag cũ**
            existingArticle.Tags.Clear();

            // **Thêm tag mới**
            var tagsToAdd = await tagRepository.FindListAsync(t => selectedTags.Contains(t.TagId));
            foreach (var tag in tagsToAdd)
            {
                existingArticle.Tags.Add(tag);
            }

            // Cập nhật bài báo
            await repository.UpdateAsync(existingArticle);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task DeleteNewsArticleAsync(string newsArticleId)
        {
            var repository = _unitOfWork.GetRepository<NewsArticle>();
            var newsArticle = await _context.NewsArticles
                .Include(na => na.Tags) // Include Tags để có thể thao tác
                .FirstOrDefaultAsync(na => na.NewsArticleId == newsArticleId);

            if (newsArticle == null)
            {
                throw new Exception("News article does not exist.");
            }

            // **Xóa tất cả tag khỏi bài báo**
            newsArticle.Tags.Clear();

            // **Cập nhật trạng thái NewsStatus (soft delete)**
            newsArticle.NewsStatus = false;

            await repository.UpdateAsync(newsArticle);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByStatus(int index, int pageSize, string status)
        {
            var repository = _unitOfWork.GetRepository<NewsArticle>();

            status = status?.Trim().ToLower();

            var query = repository.GetQueryable()
                                  .Include(na => na.Category)
                                  .Include(na => na.CreatedBy);

            if (status == "active")
            {
                return await query.Where(na => na.NewsStatus == true)
                                  .Skip(index * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            }
            else if (status == "inactive")
            {
                return await query.Where(na => na.NewsStatus == false)
                                  .Skip(index * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            }

            throw new ArgumentException("Invalid status. Only 'active' or 'inactive' are accepted.");
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByUser(short userId)
        {
            var repository = _unitOfWork.GetRepository<NewsArticle>();
            return await repository
                .FindNewAsync(na => na.CreatedById == userId, include: query => query
                    .Include(na => na.Category)
                    .Include(na => na.CreatedBy));
        }

        public async Task<IEnumerable<NewsStatistic>> GetNewsStatistics(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Where(na => na.CreatedDate.HasValue && na.CreatedDate.Value.Date >= startDate.Date && na.CreatedDate.Value.Date <= endDate.Date)
                .GroupBy(na => na.CreatedDate!.Value.Date)
                .Select(g => new NewsStatistic
                {
                    Date = g.Key,
                    TotalNews = g.Count()
                })
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
    }
}
