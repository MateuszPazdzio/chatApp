using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using chat.Server.Data;

namespace chat.Server.models
{
    public interface IRepository<T> where T : class
    {
        void Delete(T entity);
        void Create(T entity);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? predicate = null, string? includeProperties = null);
        Task<T> GetT(Expression<Func<T, bool>> predicate, string? includeProperties = null);
    }
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ChatDbContext _context;
        private DbSet<T> _dbSet;
        public Repository(ChatDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? predicate = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);

            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetT(Expression<Func<T, bool>> predicate, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            query = query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }
    }
}
