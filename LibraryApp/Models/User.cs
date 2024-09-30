using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models{

    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ім'я є обов'язковим.")]
        [StringLength(50, ErrorMessage = "Ім'я не може перевищувати 50 символів.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Прізвище є обов'язковим.")]
        [StringLength(50, ErrorMessage = "Прізвище не може перевищувати 50 символів.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email є обов'язковим.")]
        [EmailAddress(ErrorMessage = "Невірний формат Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Телефон є обов'язковим.")]
        [Phone(ErrorMessage = "Невірний формат телефону.")]
        public string Phone { get; set; }
    }
}