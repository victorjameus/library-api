using Library.Application.Common.DTOs;
using Library.Application.Common.Models;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;

namespace Library.Application.Features.Authors.Commands.CreateAuthor
{
    public sealed class CreateAuthorHandler
    (
        IRepository<Author> authors,
        ILogger<CreateAuthorHandler> logger
    ) : IRequestHandler<CreateAuthorCommand, Result<AuthorDto>>
    {
        public async Task<Result<AuthorDto>> Handle(CreateAuthorCommand request, CancellationToken ct)
        {
            try
            {
                logger.LogInformation("Creando nuevo autor: {FirstName} {LastName}", request.FirstName, request.LastName);

                var exists = await authors.ExistsAsync(a => a.FirstName == request.FirstName && a.LastName == request.LastName, ct);

                if (exists)
                {
                    logger.LogWarning("Intento de crear autor duplicado: {FirstName} {LastName}", request.FirstName, request.LastName);
                    return Result<AuthorDto>.Failure($"Ya existe un autor con el nombre {request.FirstName} {request.LastName}");
                }

                var author = new Author
                {
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    DateOfBirth = request.DateOfBirth,
                    Nationality = request.Nationality?.Trim(),
                    Biography = request.Biography?.Trim()
                };

                var created = await authors.AddAsync(author, Tracking.Clear, ct);

                logger.LogInformation("Autor creado exitosamente con ID: {AuthorId} - {FullName}", created.Id, created.GetFullName());
                var authorDto = AuthorDto.FromEntity(created);

                return Result<AuthorDto>.Success(authorDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creando autor: {FirstName} {LastName} - {ErrorMessage}", request.FirstName, request.LastName, ex.Message);
                return Result<AuthorDto>.Failure("Ocurrió un error al crear el autor. Por favor intente nuevamente.");
            }
        }
    }
}
