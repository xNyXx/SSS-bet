using System;
using System.Linq;
using System.Security.Claims;

namespace Infrastructure
{
    public static class IdentityExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(c => c.Type.EndsWith("emailaddress"))?.Value;
        }
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            return Guid.Parse(principal.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"))?.Value);
        }
    }
}
