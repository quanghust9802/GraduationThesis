using Application.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected AccessControllContext _context;
        protected DbSet<TEntity> _table;
        public BaseRepository(AccessControllContext dbContext)
        {
            _context = dbContext;
            _table = _context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            return await _table.ToListAsync();

        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return await _table.FindAsync(id);
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            _table.Add(entity);
            var result = await _context.SaveChangesAsync();
            return result;

        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result;
        }
        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            _table.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result;
        }
        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

