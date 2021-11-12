using DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DAL.Validators;

namespace DAL.Models
{
    [Table("Matches")]
    public class Matches
    {
        [Required]
        [Column(name: "Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column(name: "Tournament", TypeName = "varchar(255)")]
        public string Tournament { get; set; }

        [Required]
        [Column(name: "Title", TypeName = "varchar(255)")]
        public string Title { get; set; }

        [Required]
        [Column(name: "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        [Column(name: "Team1", TypeName = "varchar(255)")]
        public string Team1 { get; set; }

        [Required]
        [Column(name: "Team2", TypeName = "varchar(255)")]
        public string Team2 { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public Sport Sport { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public MatchStatus Status { get; set; }


        [Column(TypeName = "varchar(255)")]
        [MatchResultValidator(ErrorMessage = "Result is incorrect")]
        public string Result { get; set; }

        public ICollection<Bets> Bets { get; set; }
    }
}
