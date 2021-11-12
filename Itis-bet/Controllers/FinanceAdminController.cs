using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.ViewModels.AdminModels;
using DAL;
using DAL.Models;
using DAL.Models.Enums;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itis_bet.Controllers
{
    [Authorize(Policy = "HasAccessToAdminPanel")]
    public class FinanceAdminController : Controller
    {
        private readonly Database _db;
        private readonly INotificator<bool> _notify;

        public FinanceAdminController(Database db, INotificator<bool> notify)
        {
            _db = db;
            _notify = notify;
        }

        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            var pageSize = 50;

            var list = from t in _db.Transactions select t;

            ViewData["page"] = page;
            ViewData["pageSize"] = pageSize;
            ViewData["pageCount"] = Math.Floor((double) (list.Count() / pageSize)) + 1;

            list = list.OrderByDescending(t => t.Date).Skip((page - 1) * pageSize).Take(pageSize);
            list = list.Include(t => t.User);
            list = list.Include(t => t.UserBet.Bet);


            return View(list.ToList());
        }


        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Users = _db.Users.ToList();
            return View(new FinanceVm());
        }

        [HttpPost]
        public async Task<IActionResult> Add(FinanceVm model)
        {
            ViewBag.Users = _db.Users.ToList();

            var user = _db.Users.SingleOrDefault(u => u.Id.Equals(model.UserId));
            if (user == null) ModelState.AddModelError("UserId", "Not found");
            if (model.Money <= 0) ModelState.AddModelError("Money", "Only positive values");


            if (ModelState.IsValid)
            {
                var transaction = new Transactions();
                transaction.Date = DateTime.Now;
                transaction.UserId = model.UserId;
                transaction.Money = model.Money;
                transaction.Type = model.Type;
                _db.Transactions.Add(transaction);

                switch (transaction.Type)
                {
                    case TransactionType.Replenishment:
                        user.Money += model.Money;
                        break;
                    case TransactionType.Withdrawal:
                        user.Money -= model.Money;
                        break;
                }

                _db.Users.Update(user);

                _db.SaveChanges();

                await _notify.AboutTransactionAsync(TransactionReason.Passed, user.Email, transaction);

                return RedirectToAction("Index");
            }

            return View(new FinanceVm());
        }
    }
}
