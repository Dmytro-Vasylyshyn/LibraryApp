using LibraryApp.Models;


namespace LibraryApp.Models
{
    public class Borrowing
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } // Navigation property

        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // Navigation property

        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; } // Nullable for books not yet returned
    }
}