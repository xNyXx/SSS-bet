using System.ComponentModel.DataAnnotations;


namespace BLL.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(3, ErrorMessage = "Минимальная длинна email 3.")] // Thank google.
        [MaxLength(320, ErrorMessage = "Максимальная длинна email 320.")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Минимальная длинна пароля 5.")]
        [MaxLength(10, ErrorMessage = "Максимальная длинна пароля 10.")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public bool Remember { get; set; }
    }
}
