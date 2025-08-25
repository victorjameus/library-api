using Library.Domain.Entities;

namespace Library.Infrastructure.Data.Configurations
{
    public sealed class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder
                .ToTable("Loans")
                .HasKey(l => l.Id);

            builder
                .Property(l => l.LoanDate)
                .IsRequired();

            builder
                .Property(l => l.DueDate)
                .IsRequired();

            builder
                .Property(l => l.IsActive)
                .HasDefaultValue(true);

            builder
                .Property(l => l.Notes)
                .HasMaxLength(1000);

            builder
                .Property(l => l.ReturnCondition)
                .HasMaxLength(500);

            builder
                .Property(l => l.FineAmount)
                .HasColumnType("decimal(10,2)");

            builder
                .Property(l => l.FinePaid)
                .HasDefaultValue(false);

            builder
                .HasIndex(l => l.IsActive)
                .HasDatabaseName("IX_Loans_IsActive");

            builder
                .HasIndex(l => l.DueDate)
                .HasDatabaseName("IX_Loans_DueDate");

            builder
                .HasIndex(l => l.LoanDate)
                .HasDatabaseName("IX_Loans_LoanDate");

            builder
                .HasIndex(l => new { l.MemberId, l.IsActive })
                .HasDatabaseName("IX_Loans_Member_Active");

            builder
                .HasIndex(l => new { l.BookId, l.IsActive })
                .HasDatabaseName("IX_Loans_Book_Active");

            builder
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
