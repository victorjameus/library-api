using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Application.Common.Extensions
{
    public static class TaskRepositoryExtensions
    {
        public static async Task<T> Clear<T>(this Task<IRepositoryResult<T>> task) where T : BaseEntity
        {
            var result = await task;
            var cleared = result.Clear();

            return cleared.Entity;
        }
    }
}
