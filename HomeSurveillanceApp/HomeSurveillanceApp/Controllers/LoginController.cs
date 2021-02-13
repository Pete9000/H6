using HomeSurveillanceApp.Authentication.AuthModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeSurveillanceApp.Controllers
{
    public class LoginController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly IConfiguration _configuration;

        public LoginController(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Username,Password")] AuthenticateModel authModel)
        {
            string currentUserId = User.Identity.Name;
            //userManager.ChangePasswordAsync()
            var user = await userManager.FindByNameAsync(authModel.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, authModel.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
                double minutes = Convert.ToDouble(_configuration["JwtConfig:ExpirationInMinutes"]);
                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtConfig:ValidIssuer"],
                    audience: _configuration["JwtConfig:ValidAudience"],
                    expires: DateTime.Now.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                string key = new JwtSecurityTokenHandler().WriteToken(token);
                SetCookie(_configuration["JwtConfig:CookieName"], key, 20);
                return Redirect("/Home");
            }
            return Unauthorized();
        }

        public IActionResult Logout()
        {
            RemoveCookie(_configuration["JwtConfig:CookieName"]);
            return RedirectToAction("Index");
        }

        public void SetCookie(string key, string value, int? expiration)
        {
            CookieOptions option = new CookieOptions();
            if (expiration.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expiration.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
        public void RemoveCookie(string key)
        {
            if(Request.Cookies.ContainsKey(key))
                Response.Cookies.Delete(key);
        }
    }
}
