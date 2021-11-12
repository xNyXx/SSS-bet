using DAL.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class UsersBets
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }


        public Guid BetId { get; set; }
        public Bets Bet { get; set; }


        /* !!! Auto Generated, please dont configure selfly. !!! */
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Time { get; set; }

        public decimal Money { get; set; }

        public double Coef { get; set; }



        [Required]
        public UserBetResult Result { get; set; }

        [Required]
        public UserBetPaymentStatus Status { get; set; }
    }
}
