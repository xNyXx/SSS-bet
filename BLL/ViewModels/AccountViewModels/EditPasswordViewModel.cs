using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class EditPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Минимальная длинна пароля 5.")]
        [MaxLength(10, ErrorMessage = "Максимальная длинна пароля 10.")]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Минимальная длинна пароля 5.")]
        [MaxLength(10, ErrorMessage = "Максимальная длинна пароля 10.")]
        [Display(Name = "Пароль")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
