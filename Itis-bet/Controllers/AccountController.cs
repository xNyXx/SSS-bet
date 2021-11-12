using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BLL.Services;
using BLL.ViewModels;
using DAL;
using DAL.Models;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL.Extensions;
using System.Threading.Tasks;
using BLL.Services.PassportService;
using Infrastructure.Notifications;

namespace Itis_bet.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly Database _db;
        private readonly UserManager<User> _user;
        private readonly ProfileService _profile;
        private readonly PassportService _passport;
        private readonly INotificator<bool> _notify;

        public AccountController(Database db, UserManager<User> userManager,
            ProfileService profile, PassportService passport, INotificator<bool> notify)
        {
            _db = db;
            _user = userManager;
            _profile = profile;
            _passport = passport;
            _notify = notify;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = _user.WithProfileAndPassport(User.Identity.Name);

            return View(new Tuple<ProfileViewModel, PassportViewModel>(
                _profile.ConstructView(user), _passport.ConstructView(user)));
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel newProfile)
        {
            if (ModelState.IsValid)
            {
                var user = _user.WithProfile(User.Identity.Name);

                var someChanges = _profile.Update(user, newProfile);

                if (someChanges)
                {
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View("Index", new Tuple<ProfileViewModel, PassportViewModel>(
                newProfile, _passport.ConstructView(_user.WithPassport(User.Identity.Name))));
        }

        [HttpPost]
        public async Task<IActionResult> EditPassport(PassportViewModel newPassport)
        {
            if (ModelState.IsValid)
            {
                var user = _user.WithPassport(User.Identity.Name);

                var someChanges = _passport.Update(user, newPassport);

                if (someChanges)
                {
                    user.CanBet = true;
                    await _db.SaveChangesAsync();
                    // await _notify.AboutSecurityAsync(SecurityReason.PassportUpdated, user.Email);
                }

                return RedirectToAction("Index");
            }
            return View("Index", new Tuple<ProfileViewModel, PassportViewModel>(
                _profile.ConstructView(_user.WithProfile(User.Identity.Name)), newPassport));
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel newPasswrd)
        {
            if (ModelState.IsValid)
            {
                var user = await _user.FindByNameAsync(User.Identity.Name);

                var passwordsMatch = await _user.CheckPasswordAsync(user, newPasswrd.OldPassword);

                if (passwordsMatch)
                {
                    var passwordHasher = HttpContext.RequestServices
                        .GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    user.PasswordHash = passwordHasher.HashPassword(user, newPasswrd.NewPassword);

                    var res = await _user.UpdateAsync(user);

                    if (res.Succeeded)
                    {
                        await _notify.AboutSecurityAsync(SecurityReason.PasswordUpdated, user.Email);

                        return View("Index", new Tuple<ProfileViewModel, PassportViewModel>(
                            _profile.ConstructView(user), _passport.ConstructView(user)));
                    }
                }
                ModelState.AddModelError(string.Empty, "Incorrect current password!");
            }
            return View("Index", new Tuple<ProfileViewModel, PassportViewModel>(
                _profile.ConstructView(_user.WithProfile(User.Identity.Name)),
                _passport.ConstructView(_user.WithPassport(User.Identity.Name))));
        }

        [HttpGet]
        public IActionResult GetPersonalMenu() =>
            PartialView("_GetPersonalMenu");

        [HttpGet]
        public IActionResult GetBlogPostMenu() =>
            PartialView("_GetBlogPostMenu");


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Bets()
        {
            var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(_db.UsersBets.OrderByDescending(b=>b.Time).Include(b=>b.Bet).Include(b=>b.Bet.Match).Where(b=>b.UserId.Equals(userId)).ToList());
        }
        [HttpGet]
        public IActionResult Balance()
        {
            var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = _db.Users.Find(userId);
            ViewBag.Balance = user.Money;

            return View(_db.Transactions
                .OrderByDescending(b=>b.Date)
                .Where(b=>b.UserId.Equals(userId))
                .ToList()
            );
        }
    }
}
