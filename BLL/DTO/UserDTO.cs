using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class UserDTO
    {

        string passport;
        bool canBetting;
        string email;
        bool verificated;
        string avatar;
        decimal money;
        string name;
        string password;


        public string Passport { get { return passport; } private set { passport = value; } }
        [Required]
        public bool CanBetting { get { return canBetting; } private set { canBetting = value; } }
        public string Email { get { return email; } private set { email = value; } }
        [Required]
        public bool Verificated { get { return verificated; } private set { verificated = value; } }

        public string Avatar { get { return avatar; } private set { avatar = value; } }

        public decimal Money
        {
            get
            {
                return money;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Счёт пользователя не может быть отрицательным");
                }
                else
                {
                    money = value;
                }
            }
        }
        [Required]
        public string Name { get { return name; } private set { name = value; } } 
        [Required]
        public string Password { get { return password; } private set { password = value; } }
        public void AllowBetting(string passport)
        {
            Passport = passport;
            canBetting = true;
        }
    }
}
