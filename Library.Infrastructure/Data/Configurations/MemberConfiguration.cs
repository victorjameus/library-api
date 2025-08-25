using Library.Domain.Entities;

namespace Library.Infrastructure.Data.Configurations
{
    public sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder
                .ToTable("Members")
                .HasKey(m => m.Id);

            builder
                .Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(m => m.Phone)
                .HasMaxLength(20);

            builder
                .Property(m => m.Address)
                .HasMaxLength(500);

            builder
                .Property(m => m.IsActive)
                .HasDefaultValue(true);

            builder
                .HasIndex(m => m.Email)
                .IsUnique()
                .HasDatabaseName("IX_Members_Email");

            builder
                .HasIndex(m => new { m.FirstName, m.LastName })
                .HasDatabaseName("IX_Members_FullName");

            builder
                .HasIndex(m => m.IsActive)
                .HasDatabaseName("IX_Members_IsActive");

            builder
                .HasIndex(m => m.MembershipDate)
                .HasDatabaseName("IX_Members_MembershipDate");

            builder
                .HasMany(m => m.Loans)
                .WithOne(l => l.Member)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}