using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        [MinLength(3, ErrorMessage = "Минимальная длинна логина 3.")] 
        [MaxLength(30, ErrorMessage = "Максимальная длинна логина 30.")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(3, ErrorMessage = "Минимальная длинна email 3.")] // Thank google.
        [MaxLength(320, ErrorMessage = "Максимальная длинна email 320.")] 
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MinLength(11, ErrorMessage = "Длинна номера телефона 11.")] 
        [MaxLength(11, ErrorMessage = "Длинна номера телефона 11.")]
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Минимальная длинна пароля 5.")]
        [MaxLength(10, ErrorMessage = "Максимальная длинна пароля 10.")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
