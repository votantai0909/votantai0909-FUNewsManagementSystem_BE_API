using FUNewsManagementSystem.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> Get(int index, int pageSize);
        IQueryable<T> Entities { get; }
        IQueryable<T> GetQueryable();
        T GetById(object id);
        void Insert(T obj);
        void InsertRange(List<T> obj);
        Task InsertCollection(ICollection<T> collection);

        void Update(T obj);
        void Delete(object id);
        void Save();
        Task<IEnumerable<T>> GetAsync(int index, int pageSize);
        Task<T> GetByIdAsync(object id);
        List<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        Task InsertAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(object id);
        Task DeleteAsync(T entity);
        Task SaveAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> GetAllQueryableAsync();
        Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetAsyncByStatus(int index, int pageSize, Expression<Func<T, bool>> predicate);
        Task<IEnumerable<NewsArticle>> GetAllArticlesAsync(int index, int pageSize);
        Task<IQueryable<T>> FindNewAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<T> GetArticleByIdIncludeTagAsync(object id, params Expression<Func<T, object>>[] includes);
    }
}
