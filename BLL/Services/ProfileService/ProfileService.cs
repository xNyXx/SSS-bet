using BLL.ViewModels;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class ProfileService : ServiceBase
    {
        public ProfileService(Database db) : base(db) { }
        
        public ProfileViewModel ConstructView(User user)
        {
            var profleView = new ProfileViewModel();

            if (user.Profile != null)
            {
                var profile = user.Profile;

                profleView.Name = DefaultIfNull(profile.Name);
                profleView.LastName = DefaultIfNull(profile.LastName);
            }

            // Always configured.
            profleView.Email = user.Email;
            profleView.Phone = user.PhoneNumber;

            return profleView;
        }

        public bool Update(User user, ProfileViewModel profile)
        {
            if (IsEmpty(profile) || IsEquals(user, profile))
                return false;

            var newProfile = new UserProfile();

            newProfile.Name = DefaultIfNull(profile.Name);
            newProfile.LastName = DefaultIfNull(profile.LastName);

            user.Email = DefaultIfNull(profile.Email);
            user.PhoneNumber = DefaultIfNull(profile.Phone);

            user.Profile = newProfile;

            return true;
        }

        private TIn DefaultIfNull<TIn>(TIn value) => 
            value != null ? value : default;

        private bool IsEmpty(ProfileViewModel profile) =>
            profile.Name == null & profile.LastName == null &
            profile.Email == null & profile.Phone == null;

        private bool IsEquals(User user, ProfileViewModel profile) =>
            profile.Equals(user.Profile) &&
            user.Email.Equals(profile.Email) &&
            user.PhoneNumber.Equals(profile.Phone);
    }
}
