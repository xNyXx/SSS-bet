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
    public class SportController : Controller
    {
        private readonly Database _db;

        public SportController(Database db) =>
            _db = db;

        [HttpGet]
        public IActionResult Index() =>
            View();
    }
}
