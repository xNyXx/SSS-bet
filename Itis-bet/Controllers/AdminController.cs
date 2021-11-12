using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL.ViewModels.AdminModels;
using Infrastructure;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace Itis_bet.Controllers
{
    [Authorize(Policy = "HasAccessToAdminPanel")]
    public class AdminController : Controller
    {
        private readonly Database _db;
        private IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminController(Database db, IWebHostEnvironment appEnvironment, SignInManager<User> signInManager, UserManager<User> manager)
        {
            _db = db;
            _appEnvironment = appEnvironment;
            _signInManager = signInManager;
            _userManager = manager;
        }

        [HttpGet]
        public IActionResult Index() =>
            View();

        [Authorize(Policy = "Editor")]
        [HttpGet]
        public IActionResult BlogPosts()
        {
            var sportOptionsList = Sport.All.GetOptionsWithAll();
            var userEmail = User.GetUserEmail();
            var tableItems = _db.Articles.Include(a => a.User).Where(u=>u.User.Email==userEmail).Select(a => new BlogsTableItemVM()
            {
                Author = a.User.UserName,
                Published = a.PublishedAt,
                Header = a.Header,
                Sport = a.Sport.GetString(),
            });
            var model = new BlogPostsVM()
            {
                Sports = sportOptionsList,
                BlogsTableItems = tableItems,
            };
            return View(model);
        }

        public IActionResult GetBlogTableItems(Sport sport)
        {
            var userEmail = User.GetUserEmail();
            var posts = _db.Articles.Where(a=>a.User.Email==userEmail).Select(a=>new BlogsTableItemVM()
            {
                Author = a.User.UserName,
                Header = a.Header,
                Published = a.PublishedAt,
                Sport = a.Sport.GetString(),
            });
            var enPosts = posts.AsEnumerable();
            if (sport != Sport.All)
            {
                enPosts = enPosts.Where(p => p.Sport == sport.GetString());
            }
            return PartialView(enPosts);
        }

        [Authorize(Policy = "Editor")]
        [HttpGet]
        public IActionResult CreateBlogPost()
        {
            var model = new CreateBlogVm()
            {
                Sports = Sport.All.GetOptionsWithoutAll(),
            };
            return View(model);
        }

        [Authorize(Policy = "Editor")]
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost(CreateBlogVm model)
        {
            if (!ModelState.IsValid)
            {
                model.Sports = Sport.All.GetOptionsWithoutAll();
                return View(model);
            }
            if (model.Picture != null)
            {
                // путь к папке Files
                string path = "/images/Articles/" + model.Picture.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await model.Picture.CopyToAsync(fileStream);
                }
                var article = new Articles
                {
                    Description = model.Description,
                    Content = model.Content,
                    Sport = model.Sport,
                    Header = model.Header,
                    Picture = path,
                    PublishedAt = DateTime.Now,
                    UserId = User.GetUserId(),
                };
                _db.Articles.Add(article);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("BlogPosts");
        }

        [Authorize(Policy = "Editor")]
        [HttpGet]
        public IActionResult Comments(string articleId)
        {
            var model = _db.Comments
                .Include(c => c.Article)
                .Include(a => a.User)
                .Select(c => c);
            if(articleId != null)
            {
                Guid guid;
                
                if(Guid.TryParse(articleId, out guid))
                    model = model.Where(c => c.ArticleId == guid);

            }
               
            return View(model.AsEnumerable());
        }
        public IActionResult DeleteComment(Guid articleId, Guid commentId)
        {
            var article = _db.Articles
                .Include(a => a.Comments)
                .FirstOrDefault(a => a.Id == articleId);
            var comment = article.Comments.FirstOrDefault(c => c.Id == commentId);
            _db.Comments.Remove(comment);
            _db.SaveChanges();
            return RedirectToAction("Comments");
 
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult Users(string userId)
        {
            var model = _db.Users
                .Select(c => c);
            if (userId != null)
            {
                Guid guid;

                if (Guid.TryParse(userId, out guid))
                    model = model.Where(c => c.Id == guid);

            }

            return View(model.AsEnumerable());
        }

        [HttpGet]
        public IActionResult ViewUser(Guid userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            return View("User",user);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SetClaim(Guid userId,string value)
        {
            var t = User.Claims;
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            _db.UserClaims.Add(new IdentityUserClaim<Guid>()
            {
                ClaimType = ClaimTypes.Role,
                ClaimValue = value,
                UserId = userId,
            });
            await _db.SaveChangesAsync();
            await _userManager.UpdateSecurityStampAsync(user);
            return RedirectToAction("Users");
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveClaim(Guid userId, string value)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            var claim = _db.UserClaims.Where(x => x.UserId == userId && x.ClaimValue == value).FirstOrDefault();
            if (claim != null)
            {
                _db.UserClaims.Remove(claim);
                await _db.SaveChangesAsync();
            }
            await _userManager.UpdateSecurityStampAsync(user);


            return RedirectToAction("Users");
        }

    }
}
