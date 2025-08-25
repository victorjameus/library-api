using Library.Domain.Entities;

namespace Library.Infrastructure.Data.Configurations
{
    public sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder
                .ToTable("Authors")
                .HasKey(a => a.Id);

            builder
                .Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(a => a.Nationality)
                .HasMaxLength(50);

            builder
                .Property(a => a.Biography)
                .HasMaxLength(2000);

            builder
                .Property(a => a.CreatedAt)
                .IsRequired();

            builder
                .HasIndex(a => new { a.FirstName, a.LastName })
                .HasDatabaseName("IX_Authors_FullName");

            builder
                .HasIndex(a => a.CreatedAt)
                .HasDatabaseName("IX_Authors_CreatedAt");

            builder
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
