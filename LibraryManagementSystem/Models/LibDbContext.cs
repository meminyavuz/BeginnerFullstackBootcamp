using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    public class LibDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrow> Borrows { get; set; }

        public LibDbContext(DbContextOptions<LibDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.IsEnable)
                .HasDefaultValue(true);
        }
    }
}