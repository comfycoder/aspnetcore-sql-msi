using Microsoft.EntityFrameworkCore;

namespace MyWeb2019.Models
{
    public partial class AdventureWorksContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }

        public AdventureWorksContext()
        {
        }

        public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "SalesLT");

                entity.HasIndex(e => e.EmailAddress);

                entity.HasIndex(e => e.Rowguid)
                    .HasName("AK_Customer_rowguid")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.CompanyName).HasMaxLength(128);

                entity.Property(e => e.EmailAddress).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("Name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("Name")
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName)
                    .HasColumnType("Name")
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NameStyle).HasColumnType("NameStyle");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnType("Phone")
                    .HasMaxLength(25);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.SalesPerson).HasMaxLength(256);

                entity.Property(e => e.Suffix).HasMaxLength(10);

                entity.Property(e => e.Title).HasMaxLength(8);
            });
        }
    }
}
