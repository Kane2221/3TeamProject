using _3TeamProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace _3TeamProject.Services
{
    public class AddClaims
    {
        public AddClaims(string name, string sid, string role)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Sid, sid),
                new Claim(ClaimTypes.Role, role)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPricipal = new ClaimsPrincipal(claimsIdentity);

        }
    }
}
