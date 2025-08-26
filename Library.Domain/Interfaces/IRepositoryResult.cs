using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IRepositoryResult<T> where T : BaseEntity
    {
        /// <summary>
        /// Limpia el change tracker y retorna la entidad
        /// </summary>
        /// <returns>La entidad procesada</returns>
        IRepositoryResult<T> Clear();

        /// <summary>
        /// Obtiene la entidad resultante
        /// </summary>
        T Entity { get; }
    }
}
