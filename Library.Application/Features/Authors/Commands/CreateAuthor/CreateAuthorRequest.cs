namespace Library.Application.Features.Authors.Commands.CreateAuthor
{
    public sealed class CreateAuthorRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? Biography { get; set; }
    }
}
