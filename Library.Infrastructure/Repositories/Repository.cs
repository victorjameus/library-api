using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Data;

namespace Library.Infrastructure.Repositories
{
    public sealed class Repository<T>(ApplicationDbContext context)
        : IRepository<T> where T
        : BaseEntity
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        #region Read operations
        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync([id], ct);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet.ToListAsync(ct);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(ct);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.AnyAsync(predicate, ct);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        {
            return predicate is null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(predicate, ct);
        }
        #endregion Read operations

        #region Write operations
        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);

            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        {
            await _dbSet.AddRangeAsync(entities, ct);

            return entities;
        }

        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);

            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);

            if (entity is not null)
            {
                _dbSet.Remove(entity);
            }
        }

        public Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Remove(entity);

            return Task.CompletedTask;
        }

        public async Task SoftDeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);

            if (entity is not null)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                await UpdateAsync(entity, ct);
            }
        }
        #endregion Write operations

        #region Query building
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> QueryNoTracking()
        {
            return _dbSet.AsNoTracking();
        }
        #endregion Query building

        #region Pagination
        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync
        (
            int page,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            CancellationToken ct = default
        )
        {
            var query = _dbSet.AsNoTracking();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            var totalCount = await query.CountAsync(ct);

            if (orderBy is not null)
            {
                query = ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }
            else
            {
                query = query.OrderBy(e => e.Id);
            }

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }
        #endregion Pagination
    }
}
