using Library.Application.Common.DTOs;
using Library.Application.Common.Models;

namespace Library.Application.Features.Authors.Commands.CreateAuthor
{
    public sealed record CreateAuthorCommand
    (
        string FirstName,
        string LastName,
        DateTime? DateOfBirth,
        string? Nationality,
        string? Biography
    ) : IRequest<Result<AuthorDto>>;
}
