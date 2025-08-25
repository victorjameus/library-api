using Library.Domain.Entities;

namespace Library.Infrastructure.Data.Configurations
{
    public sealed class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .ToTable("Books")
                .HasKey(b => b.Id);

            builder
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(13);

            builder
                .Property(b => b.Genre)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(b => b.Language)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Español");

            builder
                .Property(b => b.Publisher)
                .HasMaxLength(100);

            builder
                .Property(b => b.Description)
                .HasMaxLength(2000);

            builder
                .Property(b => b.IsAvailable)
                .HasDefaultValue(true);

            builder
                .HasIndex(b => b.ISBN)
                .IsUnique()
                .HasDatabaseName("IX_Books_ISBN");

            builder
                .HasIndex(b => new { b.Title, b.AuthorId })
                .HasDatabaseName("IX_Books_Title_Author");

            builder
                .HasIndex(b => b.Genre)
                .HasDatabaseName("IX_Books_Genre");

            builder
                .HasIndex(b => b.IsAvailable)
                .HasDatabaseName("IX_Books_IsAvailable");

            builder
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(b => b.Loans)
                .WithOne(l => l.Book)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
