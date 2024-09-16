using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва книги обов'язкова")]
        [StringLength(255, ErrorMessage = "Назва книги не може перевищувати 255 символів")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Автор обов'язковий")]
        [StringLength(255, ErrorMessage = "Ім'я автора не може перевищувати 255 символів")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Жанр обов'язковий")]
        [StringLength(100, ErrorMessage = "Жанр не може перевищувати 100 символів")]
        public string Genre { get; set; }

        [Range(1000, 2100, ErrorMessage = "Введіть коректний рік видання (1000-2100)")]
        public int Year { get; set; }

        [Range(1, 1000, ErrorMessage = "Кількість примірників повинна бути в межах від 1 до 1000")]
        public int CopiesAvailable { get; set; }
    }
}
