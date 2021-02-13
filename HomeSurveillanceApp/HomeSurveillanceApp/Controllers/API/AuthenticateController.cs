using HomeSurveillanceApp.Authentication.AuthModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeSurveillanceApp.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateModel authModel)
        {
            var user = await userManager.FindByNameAsync(authModel.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, authModel.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                double minutes = Convert.ToDouble(_configuration["JwtConfig:ExpirationInMinutes"]);
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtConfig:ValidIssuer"],
                    audience: _configuration["JwtConfig:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(minutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var userExists = await userManager.FindByNameAsync(registerModel.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "User already exists!" });

            User user = new User()
            {
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.Username
            };
            var result = await userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "User creation failed! Password must have atleast 1 Uppercase and 1 special character" });
            }
            return Ok(new ResponseModel { Status = "Success", Message = "User created successfully!" });
        }

    }
}