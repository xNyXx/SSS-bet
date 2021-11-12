using System;
using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Models.Enums;

namespace BLL.ViewModels.AdminModels
{
    public class FinanceVm
    {
        [Required] public Guid UserId { get; set; }
        public User User { get; set; }

        [Required] public TransactionType Type { get; set; }

        [Required] public decimal Money { get; set; }
    }
}
