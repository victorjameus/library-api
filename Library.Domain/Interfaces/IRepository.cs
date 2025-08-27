using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> AsQuery();
        Task<T?> FindAsync(int id, CancellationToken ct = default);
        Task<bool> HasRelatedAsync<TRelated>(int id, Expression<Func<T, ICollection<TRelated>>> relations, CancellationToken ct = default);
        Task<T> AddAsync(T entity, CancellationToken ct = default);
        Task<T> AddAsync(T entity, Tracking trackingBehavior, CancellationToken ct = default);
        Task<int> UpdateAsync(int id, Action<T> updateAction, CancellationToken ct = default);
        Task<int> DeleteAsync(int id, CancellationToken ct = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    }
}
