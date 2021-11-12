using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels.AdminModels
{
    public class BlogPostsVM
    {
        public IEnumerable<BlogsTableItemVM> BlogsTableItems { get; set; }
        public string Sport { get; set; }
        public IEnumerable<SelectListItem> Sports { get; set; }
        public string AuthorName { get; set; }
    }
}
