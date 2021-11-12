using DAL.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.ViewModels.AdminModels
{
    public class CreateBlogVm
    {
        [Required]
        public Sport Sport { get; set; }
        public IEnumerable<SelectListItem> Sports { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 4,
            ErrorMessage = "Header must be less than 64 and more 4 symbols")]
        public string Header { get; set; }
        [Required(ErrorMessage = "Please set description")]
        [StringLength(255, MinimumLength = 4,
            ErrorMessage = "Description must be less than 255 and more 4 symbols")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Its a consonance, post without content! ")]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public IFormFile Picture { get; set; }
    }
}
