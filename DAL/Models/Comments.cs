using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Comments
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }


        public Guid ArticleId { get; set; }
        public Articles Article { get; set; }


        [Required]
        public string Text { get; set; }

    }
}
