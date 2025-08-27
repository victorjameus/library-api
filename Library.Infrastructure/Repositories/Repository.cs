using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Infrastructure.Data;
using System.Linq.Expressions;

namespace Library.Infrastructure.Repositories
{
    public sealed class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public IQueryable<T> AsQuery() => _dbSet;

        public async Task<T?> FindAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync([id], ct);
        }

        public async Task<bool> HasRelatedAsync<TRelated>(int id, Expression<Func<T, ICollection<TRelated>>> relations, CancellationToken ct = default)
        {
            var expression = Expression.Lambda<Func<T, bool>>
            (
                Expression.Call(typeof(Enumerable), nameof(Enumerable.Any), [typeof(TRelated)], relations.Body),
                relations.Parameters
            );

            return await _dbSet
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync(expression, ct);
        }

        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            return await AddAsync(entity, Tracking.Track, ct);
        }

        public async Task<T> AddAsync(T entity, Tracking trackingBehavior, CancellationToken ct = default)
        {
            var entry = await _dbSet.AddAsync(entity, ct);

            await context.SaveChangesAsync(ct);

            if (trackingBehavior == Tracking.Clear)
            {
                context.ChangeTracker.Clear();
            }

            return entry.Entity;
        }

        public async Task<int> UpdateAsync(int id, Action<T> updateAction, CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync([id], ct);

            if (entity == null)
            {
                return 0;
            }

            updateAction(entity);

            entity.UpdatedAt = DateTime.UtcNow;

            return await context.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(e => e.Id == id)
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(e => e.IsDeleted, true)
                    .SetProperty(e => e.DeletedAt, DateTime.UtcNow)
                    .SetProperty(e => e.UpdatedAt, DateTime.UtcNow), ct);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.AnyAsync(predicate, ct);
        }
    }
}
