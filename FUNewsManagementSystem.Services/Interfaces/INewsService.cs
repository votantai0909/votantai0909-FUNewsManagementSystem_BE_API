using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Services.Models.ArticleDTOs;
using FUNewsManagementSystem.Services.Models.LoginDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetNewsArticles(int index, int pageSize);
        Task<NewsArticle> GetNewsArticleById(string id);
        Task<bool> CreateNewsArticle(NewsArticle newsArticle, int[] selectedTags);
        Task<string> GenerateNewsArticleIdAsync();
        Task<bool> UpdateNewsArticle(NewsArticle newsArticle, int[] selectedTags);
        Task DeleteNewsArticleAsync(string newsArticleId);
        Task<IEnumerable<NewsArticle>> GetNewsArticlesByStatus(int index, int pageSize, string status);
        Task<IEnumerable<NewsArticle>> GetNewsArticlesByUser(short userId);
        Task<IEnumerable<NewsStatistic>> GetNewsStatistics(DateTime startDate, DateTime endDate);
    }
}
