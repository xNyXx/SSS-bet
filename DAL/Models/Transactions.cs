using DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Transactions
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        public Guid UserId { get; set; }
        public User User { get; set; }


        [Required]
        public TransactionType Type { get; set; } 


        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public decimal Money { get; set; }

        public Guid? User_Bet_Id { get; set; }

        public UsersBets UserBet { get; set; }
    } 
}
