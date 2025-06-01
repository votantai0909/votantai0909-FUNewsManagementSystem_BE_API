using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repositories.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public NewsArticleRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.NewsStatus == true)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetByIdAsync(string newsArticleId)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == newsArticleId);
        }

        public async Task<IEnumerable<NewsArticle>> GetByCategoryAsync(short categoryId)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.CategoryId == categoryId && n.NewsStatus == true)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> SearchAsync(string searchTerm)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.NewsStatus == true &&
                           (n.NewsTitle!.Contains(searchTerm) ||
                            n.Headline.Contains(searchTerm) ||
                            n.NewsContent!.Contains(searchTerm)))
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<NewsArticle> CreateAsync(NewsArticle newsArticle)
        {
            _context.NewsArticles.Add(newsArticle);
            await _context.SaveChangesAsync();
            return newsArticle;
        }

        public async Task<NewsArticle> UpdateAsync(NewsArticle newsArticle)
        {
            _context.NewsArticles.Update(newsArticle);
            await _context.SaveChangesAsync();
            return newsArticle;
        }

        public async Task<bool> DeleteAsync(string newsArticleId)
        {
            var article = await GetByIdAsync(newsArticleId);
            if (article == null) return false;

            _context.NewsArticles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
