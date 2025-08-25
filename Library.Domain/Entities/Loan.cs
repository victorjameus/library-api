namespace Library.Domain.Entities
{
    public sealed class Loan : BaseEntity
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime LoanDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public string? ReturnCondition { get; set; }
        public decimal? FineAmount { get; set; }
        public bool FinePaid { get; set; } = false;

        public Book Book { get; set; } = null!;
        public Member Member { get; set; } = null!;

        public bool IsOverdue() => IsActive && DateTime.UtcNow > DueDate;
        public int GetDaysOverdue() => IsOverdue() ? (DateTime.UtcNow - DueDate).Days : 0;

        public void ReturnBook(string? condition = null)
        {
            ReturnDate = DateTime.UtcNow;
            IsActive = false;
            ReturnCondition = condition;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RenewLoan(int additionalDays)
        {
            if (IsActive && !IsOverdue())
            {
                DueDate = DueDate.AddDays(additionalDays);
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
