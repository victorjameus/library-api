namespace Library.Domain.Entities
{
    public sealed class Member : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }

        public ICollection<Loan> Loans { get; set; } = [];

        public string GetFullName() => $"{FirstName} {LastName}";
        public bool CanBorrowBooks() => IsActive && !IsDeleted;
        public int GetActiveLoanCount() => Loans.Count(l => l.IsActive);
        public bool HasOverdueLoans() => Loans.Any(l => l.IsOverdue());
    }
}
