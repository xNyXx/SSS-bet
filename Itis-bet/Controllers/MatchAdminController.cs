using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MatchAdminController : Controller
    {
        private Database _db;
        private readonly INotificator<bool> _notify;

        public MatchAdminController(Database db, INotificator<bool> notify)
        {
            _db = db;
            _notify = notify;
        }

        // GET
        public IActionResult Index(int page = 1, string sortOrder = "", string search = "")
        {
            var pageSize = 20;

            var matches = from m in _db.Matches select m;

            switch (sortOrder)
            {
                case "title":
                    matches = matches.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    matches = matches.OrderByDescending(s => s.Title);
                    break;
                default:
                    matches = matches.OrderBy(s => s.Id);
                    break;
            }

            ViewBag.TitleSortParam = sortOrder == "title" ? "title_desc" : "title";

            if (!String.IsNullOrEmpty(search))
            {
                matches = matches.Where(a => a.Title.ToLower().Contains(search.ToLower()));
            }


            ViewData["s"] = search;
            ViewData["sortOrder"] = sortOrder;
            ViewData["page"] = page;
            ViewData["pageSize"] = pageSize;
            ViewData["pageCount"] = Math.Floor((double) (matches.Count() / pageSize)) + 1;

            matches = matches.Skip((page - 1) * pageSize).Take(pageSize);


            return View(matches.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit", (new Matches()));
        }

        [HttpPost]
        public IActionResult Create(Matches model)
        {
            if (ModelState.IsValid)
            {
                _db.Matches.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Edit", (new Matches()));
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var model = _db.Matches
                .SingleOrDefault(a => a.Id.Equals(id));

            if (model == null)
                return RedirectToAction("Index");

            return View("Edit", model);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, Matches model)
        {
            if (ModelState.IsValid)
            {
                _db.Matches.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Edit", model);
        }

        [HttpGet]
        public IActionResult Remove(Guid id)
        {
            var model = _db.Matches.FirstOrDefault(r => r.Id.Equals(id));

            if (model == null)
                return RedirectToAction("Index");

            _db.Matches.Remove(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Confirm(Guid id)
        {
            var model = _db.Matches.FirstOrDefault(r => r.Id.Equals(id));

            if (model == null || model.Status.Equals(MatchStatus.Finished))
                return RedirectToAction("Index");

            var usersBets = _db.UsersBets
                .Include(b => b.Bet)
                .Include(b => b.Bet.Match)
                .Where(b => b.Bet.MatchId.Equals(id)).ToList();

            var result = model.Result.Split(':');
            var team1 = result[0];
            var team2 = result[1];

            bool win = false;

            foreach (var userBet in usersBets)
            {
                var user = _db.Users.Single(u => u.Id.Equals(userBet.UserId));
                var transaction = new Transactions();
                transaction.UserId = user.Id;
                transaction.User_Bet_Id = userBet.Id;

                switch (userBet.Bet.Description)
                {
                    case MatchResult.Draw:
                        if (Convert.ToInt32(team1) == Convert.ToInt32(team2))
                            win = true;
                        break;
                    case MatchResult.W1:
                        if (Convert.ToInt32(team1) > Convert.ToInt32(team2)) win = true;
                        break;
                    case MatchResult.W2:
                        if (Convert.ToInt32(team1) < Convert.ToInt32(team2)) win = true;
                        break;
                }

                if (win)
                {
                    userBet.Result = UserBetResult.Won;
                    user.Money += (decimal) userBet.Coef * userBet.Money;


                    transaction.Date = DateTime.Now;
                    transaction.Money = (decimal) userBet.Coef * userBet.Money;
                    transaction.Type = TransactionType.Replenishment;

                    _notify.AboutBetAsync(BetReason.Winned, user.Email, userBet);
                }
                else
                {
                    transaction.Date = userBet.Time;
                    transaction.Money = userBet.Money;
                    transaction.Type = TransactionType.Withdrawal;

                    userBet.Result = UserBetResult.Losed;
                    _notify.AboutBetAsync(BetReason.Loosed, user.Email, userBet);
                }

                userBet.Status = UserBetPaymentStatus.Finished;
                userBet.Bet.Match.Status = MatchStatus.Finished;


                _db.UsersBets.Update(userBet);
                _db.Users.Update(user);
                _db.Matches.Update(userBet.Bet.Match);
                _db.Transactions.Add(transaction);

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult FillData()
        {
            var random = new Random();
            var teams = new List<string>()
            {
                "Челси", "Мадрид", "Сидней", "Перт", "Флорида", "США", "Канада", "Йемен"
            };

            var tours = new List<string>() {"Чемпионат мира", "Чемпионат вселенной", "Чемпионат итиса"};

            foreach (var tour in tours)
            {
                for (int j = 1; j < 8; j++)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        var model = new Matches();
                        model.Tournament = tour;
                        model.Team1 = teams[random.Next(teams.Count)];
                        model.Team2 = teams[random.Next(teams.Count)];
                        model.Sport = (Sport) j;
                        model.Date = RandomDay();
                        model.Result = random.Next(4).ToString() + ":" + random.Next(4).ToString();
                        model.Status = (MatchStatus) (i % 3);
                        model.Title = $"{model.Team1} vs {model.Team2}";

                        _db.Matches.Add(model);
                        _db.SaveChanges();

                        for (int k = 0; k < 3; k++)
                        {
                            var min = 1.1;
                            var max = 3.3;
                            var rand = new Random();
                            var result = Math.Round(rand.NextDouble() * (max - min) + min, 2);

                            var bet = new Bets();
                            bet.MatchId = model.Id;
                            bet.Coef = result;
                            bet.Description = (MatchResult) k;
                            _db.Bets.Add(bet);
                        }
                        _db.SaveChanges();
                    }
                }
            }

            return Redirect("/");
        }

        private Random gen = new Random();

        DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
