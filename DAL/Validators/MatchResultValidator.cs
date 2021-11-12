using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Validators
{
    public class MatchResultValidator:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                var res = value.ToString().Split(':');
                Convert.ToInt32(res[0]);
                Convert.ToInt32(res[1]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
