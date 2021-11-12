using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models.Enums;
namespace BLL.ViewModels.AdminModels
{
    public class BlogsTableItemVM
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public string Sport { get; set; }
        public string Header { get; set; }
    }
}
