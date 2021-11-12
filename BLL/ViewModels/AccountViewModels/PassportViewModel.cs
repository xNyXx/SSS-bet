using DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class PassportViewModel
    {
        [Display(Name = "Серия")]
        [MinLength(4, ErrorMessage = "Длинна серии паспорта 4 цифры.")]
        [MaxLength(4, ErrorMessage = "Длинна серии паспорта 4 цифры.")]
        public string Serial { get; set; }

        [Display(Name = "Нормер")]
        [MinLength(6, ErrorMessage = "Длинна номера паспорта 6 цифор.")]
        [MaxLength(6, ErrorMessage = "Длинна номера паспорта 6 цифор.")]
        public string Number { get; set; }

        [Display(Name = "Кем выдан")]
        [MinLength(3, ErrorMessage = "Минимальная длинна поля 3.")]
        [MaxLength(500, ErrorMessage = "Максимальная длинна поля 500.")]
        public string Issued { get; set; }

        public PassportViewModel() { }


        public override int GetHashCode() =>
            HashCode.Combine(Serial, Number, Issued);

        public override bool Equals(object obj) =>
            obj is Passport model &&
                Serial == model.Serial &&
                Number == model.Number &&
                Issued == model.Issued;
    }
}
