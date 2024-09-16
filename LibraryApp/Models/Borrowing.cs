namespace LibraryApp.Models
{
    public class Borrowing
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
    }
}
