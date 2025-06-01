using FUNewsManagementSystem.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repositories.Interfaces
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
        Task<NewsArticle?> GetByIdAsync(string newsArticleId);
        Task<IEnumerable<NewsArticle>> GetByCategoryAsync(short categoryId);
        Task<IEnumerable<NewsArticle>> SearchAsync(string searchTerm);
        Task<NewsArticle> CreateAsync(NewsArticle newsArticle);
        Task<NewsArticle> UpdateAsync(NewsArticle newsArticle);
        Task<bool> DeleteAsync(string newsArticleId);
    }
}
