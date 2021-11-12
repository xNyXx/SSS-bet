using DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Bets
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid MatchId { get; set; }
        public Matches Match { get; set; }


        [Required]
        public MatchResult Description { get; set; }

        public double Coef { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public BetStatus Status { get; set; }
    }
}
