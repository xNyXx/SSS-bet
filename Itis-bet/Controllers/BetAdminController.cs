using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ViewModels;
using DAL;
using DAL.Models;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itis_bet.Controllers
{
    [Authorize(Policy = "HasAccessToAdminPanel")]
    public class BetAdminController : Controller
    {
        private Database _db;

        public BetAdminController(Database db) =>
            _db = db;

        // GET
        public IActionResult Index(int page = 1)
        {
            int pageSize = 50;

            var bets = from b in _db.Bets select b;
            bets = bets.Include(b => b.Match);
            bets = bets.OrderByDescending(b => b.Match.Date);
            bets = bets.Skip((page - 1) * pageSize).Take(pageSize);


            return View(bets.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Matches = GetMatches();
            return View("Edit", (new Bets()));
        }

        [HttpPost]
        public IActionResult Create(Bets model)
        {
            ViewBag.Matches = GetMatches();
            if (ModelState.IsValid)
            {
                _db.Bets.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Edit", (new Bets()));
        }

        public IEnumerable<Matches> GetMatches()
        {
            return _db.Matches.Where(a => a.Status.Equals(MatchStatus.Waiting));
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            ViewBag.Matches = GetMatches();
            var model = _db.Bets
                .SingleOrDefault(a => a.Id.Equals(id));

            if (model == null)
                return RedirectToAction("Index");

            return View("Edit", model);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, Bets model)
        {
            ViewBag.Matches = GetMatches();
            if (ModelState.IsValid)
            {
                _db.Bets.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Edit", model);
        }

        [HttpGet]
        public IActionResult Remove(Guid id)
        {
            var model = _db.Bets.FirstOrDefault(r => r.Id.Equals(id));

            if (model == null)
                return RedirectToAction("Index");

            _db.Bets.Remove(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UserBets()
        {
            var model = _db.UsersBets.Include(b => b.User).Include(b=>b.Bet).Include(b=>b.Bet.Match).ToList();


            return View(model);
        }
    }
}
