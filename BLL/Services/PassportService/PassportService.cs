using BLL.ViewModels;
using DAL;
using DAL.Models;

namespace BLL.Services.PassportService
{
    public class PassportService : ServiceBase
    {
        public PassportService(Database db) : base(db) { }

        public PassportViewModel ConstructView(User user)
        {
            var passportView = new PassportViewModel();

            if(user.Passport != null)
            {
                var passport = user.Passport;

                passportView.Serial = DefaultIfNull(passport.Serial);
                passportView.Number = DefaultIfNull(passport.Number);
                passportView.Issued = DefaultIfNull(passport.Issued);
            }

            return passportView;
        }

        public bool Update(User user, PassportViewModel passport)
        {
            if (IsEmpty(passport) || passport.Equals(user.Passport))
                return false;

            var newPassport = new Passport();

            newPassport.Serial = DefaultIfNull(passport.Serial);
            newPassport.Number = DefaultIfNull(passport.Number);
            newPassport.Issued = DefaultIfNull(passport.Issued);

            user.Passport = newPassport;

            return true;
        }

        private TIn DefaultIfNull<TIn>(TIn value) =>
            value != null ? value : default;

        private bool IsEmpty(PassportViewModel passport) =>
            passport.Issued == null & passport.Number == null & passport.Serial == null;

    }
}
