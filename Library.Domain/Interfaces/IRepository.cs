using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Obtiene un IQueryable para consultas LINQ
        /// </summary>
        /// <returns>IQueryable para construir consultas</returns>
        IQueryable<T> AsQuery();

        /// <summary>
        /// Agrega una entidad y la persiste inmediatamente
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        /// <param name="ct">Token de cancelación</param>
        /// <returns>Resultado que permite encadenar operaciones</returns>
        Task<IRepositoryResult<T>> AddAsync(T entity, CancellationToken ct = default);

        /// <summary>
        /// Limpia el change tracker para liberar memoria
        /// </summary>
        void Clear();
    }
}
