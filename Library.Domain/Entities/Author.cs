namespace Library.Domain.Entities
{
    public sealed class Author : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? Biography { get; set; }

        public ICollection<Book> Books { get; set; } = [];

        public string GetFullName() => $"{FirstName} {LastName}";
        public int GetAge() => DateOfBirth.HasValue ? DateTime.Now.Year - DateOfBirth.Value.Year : 0;
    }
}
