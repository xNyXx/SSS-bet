using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL;
using DAL.Models;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITIS_Bet.Controllers
{
    public class HomeController : Controller
    {
        private readonly Database _db;

        private readonly MongoService _service;
        public HomeController(Database db, MongoService service)
        {
            _db = db;
            _service = service;
        }

        public IActionResult Index(string sport = null, string search = null)
        {
            var matches = _db.Matches.Where(a =>
                a.Status.Equals(MatchStatus.Active) || a.Status.Equals(MatchStatus.Waiting));

            if (sport != null) matches = matches.Where(a => a.Sport.Equals((Sport) Enum.Parse(typeof(Sport), sport)));
            if (search != null) matches = matches.Where(a => a.Title.ToLower().Contains(search.ToLower()));

            ViewData["search"] = search;

            matches = matches.Include(a=>a.Bets.OrderBy(b=>b.Description));


            var map = new Dictionary<Sport, Dictionary<string, List<Matches>>>();
            foreach (var match in matches)
            {
                if (!map.ContainsKey(match.Sport))
                    map.Add(match.Sport, new Dictionary<string, List<Matches>>());

                if (!map[match.Sport].ContainsKey(match.Tournament))
                    map[match.Sport].Add(match.Tournament, new List<Matches>());

                map[match.Sport][match.Tournament].Add(match);
            }

            if (sport == null) ViewData["sport"] = "";
            else
                ViewData["sport"] = sport;

            ViewBag.Banners = _service.Get();

            return View(map);
        }

        public IActionResult Privacy() => View();
    }
}
