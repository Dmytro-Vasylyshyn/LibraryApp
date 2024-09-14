namespace LibraryApp.Models
{
    public class Borrowing
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // Дата повернення може бути null, якщо книга ще не повернена
    }
}
