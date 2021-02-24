using HomeSurveillanceApp.Models.Auth;
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
using Microsoft.AspNetCore.Authorization;


namespace HomeSurveillanceApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public LoginController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Username,Password")] Authentication authModel)
        {
            var user = await _userManager.FindByNameAsync(authModel.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, authModel.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

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
                    expires: DateTime.Now.AddMinutes(minutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                string key = new JwtSecurityTokenHandler().WriteToken(token);
                SetCookie(_configuration["JwtConfig:CookieName"], key, Convert.ToInt32(minutes));
                return Redirect("/");
            }
            return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username, UserRole, Password")] Registration registerModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerModel.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.Username
            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                var role = _roleManager.FindByNameAsync(registerModel.UserRole).Result;
                await _userManager.AddToRoleAsync(user, role.Name);
                ViewBag.Message = "Succesfully created user";
                ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
                return View();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Password must have atleast 1 Uppercase and 1 special character" });
        }

        public IActionResult Logout()
        {
            RemoveCookie(_configuration["JwtConfig:CookieName"]);
            return RedirectToAction("Index");
        }

        void SetCookie(string key, string value, int? expiration)
        {
            //set http only to remove access from client side javascript
            CookieOptions option = new CookieOptions { HttpOnly = true};
            if (expiration.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expiration.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }
        void RemoveCookie(string key)
        {
            if (Request.Cookies.ContainsKey(key))
                Response.Cookies.Delete(key);
        }
    }
}
