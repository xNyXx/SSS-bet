using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class UserManagerExtensions
    {
        public static User WithProfileAndPassport(this UserManager<User> manager, string name) =>
            manager.Users
                .Where(u => u.UserName.Equals(name))
                .Include(p => p.Passport)
                .Include(p => p.Profile)
                .Single();

        public static User WithProfile(this UserManager<User> manager, string name) =>
            manager.Users
                .Where(u => u.UserName.Equals(name))
                .Include(p => p.Profile)
                .Single();

        public static User WithPassport(this UserManager<User> manager, string name) =>
            manager.Users
               .Where(u => u.UserName.Equals(name))
               .Include(p => p.Passport)
               .Single();
    }
}
