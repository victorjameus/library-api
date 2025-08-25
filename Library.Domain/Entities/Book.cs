namespace Library.Domain.Entities
{
    public sealed class Book : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public DateTime PublishDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int Pages { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Language { get; set; } = "Español";
        public string Publisher { get; set; } = string.Empty;

        public Author Author { get; set; } = null!;
        public ICollection<Loan> Loans { get; set; } = [];

        public bool CanBeBorrowed() => IsAvailable && !IsDeleted;
        public void MarkAsUnavailable() => IsAvailable = false;
        public void MarkAsAvailable() => IsAvailable = true;
    }
}
