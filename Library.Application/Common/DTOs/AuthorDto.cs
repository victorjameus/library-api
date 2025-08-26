using Library.Domain.Entities;

namespace Library.Application.Common.DTOs
{
    public sealed class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string? Nationality { get; set; }
        public string? Biography { get; set; }
        public int BookCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static AuthorDto FromEntity(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                FullName = author.GetFullName(),
                DateOfBirth = author.DateOfBirth,
                Age = author.GetAge(),
                Nationality = author.Nationality,
                Biography = author.Biography,
                BookCount = author.Books?.Count ?? 0,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt
            };
        }

        public static IEnumerable<AuthorDto> FromEntities(IEnumerable<Author> authors)
        {
            return authors.Select(FromEntity);
        }
    }
}
