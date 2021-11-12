using DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class ProfileViewModel
    {
        [DataType(DataType.Text)]
        [MinLength(1, ErrorMessage = "Таких коротких имен не бывает.")]
        [MaxLength(30, ErrorMessage = "Таких длинных имен не бывает.")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [MinLength(1, ErrorMessage = "Таких коротких фамилий не бывает.")]
        [MaxLength(40, ErrorMessage = "Таких длинных фамилий не бывает.")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(3, ErrorMessage = "Минимальная длинна email 3.")] // Thank google.
        [MaxLength(320, ErrorMessage = "Максимальная длинна email 320.")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [MinLength(11, ErrorMessage = "Длинна номера телефона 11.")]
        [MaxLength(11, ErrorMessage = "Длинна номера телефона 11.")]
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        public ProfileViewModel() {}

        public override bool Equals(object obj) =>
            obj is UserProfile model &&
               Name == model.Name &&
               LastName == model.LastName;

        public override int GetHashCode() =>
            HashCode.Combine(Name, LastName, Email, Phone);
    }
}
