using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Infrastructure.Repositories
{
    public sealed class RepositoryResult<T>(T entity, IRepository<T> repository) : IRepositoryResult<T> where T : BaseEntity
    {
        private readonly T _entity = entity;
        private readonly IRepository<T> _repository = repository;

        public IRepositoryResult<T> Clear()
        {
            _repository.Clear();

            return this;
        }

        public T Entity => _entity;

        public static implicit operator T(RepositoryResult<T> result) => result._entity;
    }
}
