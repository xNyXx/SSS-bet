using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;
using DAL.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Itis_bet.Controllers
{
    public class ResultController : Controller
    {
        private readonly Database _db;

        public ResultController(Database db) =>
            _db = db;

        // GET
        public IActionResult Index()
        {
            var matches = _db.Matches.Where(a=>a.Status.Equals(MatchStatus.Finished));


            var map = new  Dictionary<string, List<Matches>>();
            foreach (var match in matches)
            {

                if (!map.ContainsKey(match.Tournament))
                    map.Add(match.Tournament, new List<Matches>());

                map[match.Tournament].Add(match);
            }


            return View(map);
        }
    }
}
