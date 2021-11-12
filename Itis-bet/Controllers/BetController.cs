using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using DAL.Models.Enums;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Itis_bet.Controllers
{
    public class BetController : Controller
    {
        private readonly Database _db;
        // private readonly INotificator<bool> _notify;

        public BetController(Database db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid id, decimal sum, string bet)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new {status = "Error", message = "Need auth"});


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _db.Users.Find(new Guid(userId));

            if(!user.CanBet)
                return Json(new {status = "Error", message = "Access denied. Please, fill your passport data"});
            if(user.Money < sum)
                return Json(new {status = "Error", message = "Lack of balance"});

            var Match = _db.Matches
                .SingleOrDefault(m => (m.Status.Equals(MatchStatus.Active) ||
                                       m.Status.Equals(MatchStatus.Waiting)) && m.Id.Equals(id));

            if (Match == null) return Json(new {status = "Error", message = "Match not found"});

            var Bet = _db.Bets.SingleOrDefault(b =>
                b.MatchId.Equals(id) && b.Description.Equals((MatchResult) Enum.Parse(typeof(MatchResult), bet)));

            if (Bet == null) return Json(new {status = "Error", message = "Bet not found "});


            user.Money -= sum;
            _db.Users.Update(user);

            var model = new UsersBets();
            model.UserId = new Guid(userId);
            model.BetId = Bet.Id;
            model.Coef = Bet.Coef;
            model.Money = sum;
            model.Time = DateTime.Now;
            model.Status = UserBetPaymentStatus.Waiting;
            _db.UsersBets.Add(model);
            _db.SaveChanges();

            //await _notify.AboutBetAsync(BetReason.Applyed, user.Email, model);


            return Json(new {status = "success", betId = model.Id});
        }
    }
}
