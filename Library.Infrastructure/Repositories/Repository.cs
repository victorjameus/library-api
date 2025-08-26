using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Data;

namespace Library.Infrastructure.Repositories
{
    public sealed class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public IQueryable<T> AsQuery() => _dbSet;

        public async Task<IRepositoryResult<T>> AddAsync(T entity, CancellationToken ct = default)
        {
            var entry = await _dbSet.AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);

            return new RepositoryResult<T>(entry.Entity, this);
        }

        public void Clear()
        {
            context.ChangeTracker.Clear();
        }
    }
}
