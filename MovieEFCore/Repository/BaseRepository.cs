using Microsoft.EntityFrameworkCore;
using Movies;
using MoviesCore.Services;
using System.Linq.Expressions;

namespace MoviesEF.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;

        public BaseRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
            return entity;
        }
        public async Task<T> DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
            return entity;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.SingleOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<T>> FindAllAsync(string[] includes = null, Expression<Func<T, object>> orderBy = null, string orderByDirection = "ASC")
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (orderBy != null)
            {
                if (orderByDirection == "ASC")
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }




    }
}
