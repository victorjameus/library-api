using Library.Application.Common.Models;

namespace Library.Application.Features.Authors.Commands.DeleteAuthor
{
    public sealed record DeleteAuthorCommand(int Id) : IRequest<Result>;
}
