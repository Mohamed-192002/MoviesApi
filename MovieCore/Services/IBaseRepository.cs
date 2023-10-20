using System.Linq.Expressions;

namespace MoviesCore.Services
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> FindAllAsync(string[] includes = null, Expression<Func<T, object>> orderBy = null, string orderByDirection = "ASC");
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);

    }
}
