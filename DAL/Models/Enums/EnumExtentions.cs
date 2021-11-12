using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Models.Enums
{
    public static class EnumExtentions
    {
        public static string GetString(this Sport sport)
        {
            return sport switch {
                Sport.Football => "Football",
                Sport.Basketball => "BasketBall",
                Sport.All => "All",
                _ => throw new NotSupportedException(),
            };
        }
        public static IEnumerable<Sport> GetAllSports(this Sport sport)
        {
            return new List<Sport>()
            {
                Sport.All,
                Sport.Basketball,
                Sport.Football,
            };
        }
        public static IEnumerable<SelectListItem> GetOptionsWithAll(this Sport sport)
        {
            var options = sport.GetAllSports();
            return options.Select(o => new SelectListItem(o.GetString(), ((int)o).ToString()));
        }
        public static IEnumerable<SelectListItem> GetOptionsWithoutAll(this Sport sport)
        {
            var options = sport.GetAllSports().Where(o=>o!=Sport.All);
            return options.Select(o => new SelectListItem(o.GetString(),((int)o).ToString()));
        }
    }
}
