using Library.Application.Common.Models;
using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Application.Features.Authors.Commands.DeleteAuthor
{
    public sealed class DeleteAuthorHandler
    (
        IRepository<Author> authors,
        ILogger<DeleteAuthorHandler> logger
    ) : IRequestHandler<DeleteAuthorCommand, Result>
    {
        public async Task<Result> Handle(DeleteAuthorCommand request, CancellationToken ct)
        {
            try
            {
                logger.LogInformation("Eliminando autor con ID: {AuthorId}", request.Id);

                var exists = await authors.ExistsAsync(a => a.Id == request.Id, ct);

                if (!exists)
                {
                    logger.LogWarning("Intento de eliminar autor inexistente: {AuthorId}", request.Id);

                    return Result.Failure("El autor no existe");
                }

                var hasBooks = await authors.HasRelatedAsync(request.Id, a => a.Books, ct);

                if (hasBooks)
                {
                    logger.LogWarning("Intento de eliminar autor con libros asociados: {AuthorId}", request.Id);

                    return Result.Failure("No se puede eliminar el autor porque tiene libros asociados");
                }

                var rowsAffected = await authors.DeleteAsync(request.Id, ct);

                if (rowsAffected is 0)
                {
                    return Result.Failure("No se pudo eliminar el autor");
                }

                logger.LogInformation("Autor eliminado exitosamente: {AuthorId}", request.Id);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error eliminando autor {AuthorId}: {ErrorMessage}", request.Id, ex.Message);

                return Result.Failure("Ocurrió un error al eliminar el autor");
            }
        }
    }
}
