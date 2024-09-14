using Microsoft.EntityFrameworkCore;
using LibraryApp.Models;

namespace LibraryApp.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) {}

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Borrowing>()
                .HasOne(b => b.Book)
                .WithMany()
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<Borrowing>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);
        }
    }
}
