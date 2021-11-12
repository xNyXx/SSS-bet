using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DAL.Models
{
    public class User : IdentityUser<Guid>
    {
        [Key]
        public override Guid Id { get; set; }

        public Guid ProfileId { get; set; }
        public UserProfile Profile { get; set; }

        public Guid  PassportId { get; set; }
        public Passport Passport { get; set; }
        public decimal Money { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool CanBet { get; set; }

        public IEnumerable<Comments> Comments { get; set; }

        public IEnumerable<Articles> Articles { get; set; }
    }

    public class UserProfile
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }

    }

    public class Passport
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public string Serial { get; set; }
        public string Number { get; set; }
        public string Issued { get; set; }
    }
}
