using LibraryApp.Models;


namespace LibraryApp.Models
{
    public class Borrowing
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } 

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnedDate { get; set; } 
    }
}