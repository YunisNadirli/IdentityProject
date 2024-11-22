using IdentityApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Runtime.CompilerServices;

namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountsController> _logger;
        public AccountsController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            ILogger<AccountsController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
            _logger = logger;

        }
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LogInDto dto)
        {
            string username = dto.Username;
            string password = dto.Password;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return Unauthorized();

            //var result = await _signInManager.PasswordSignInAsync(user, password,false,false);
            if (user.PasswordHash != password) return Unauthorized();
            await _signInManager.SignInAsync(user,true);
            string role = "user";
            if (username == "root")
            {
                role = "admin";
                _logger.LogInformation($"{role} sisteme giris etdi");
            } 
            var token = GenerateJwtToken(
                username,username == "root"?"admin":"user",10);

            return Ok(new {token.AccessToken});
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Çıxış edildi.");
        }

        private Token GenerateJwtToken(string username, string role,int minute)
        {
            Token token = new();
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.Role, role)
            };
            SymmetricSecurityKey securityKey = 
                new(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            SigningCredentials signingCredentials = 
                new(securityKey, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.UtcNow.AddMinutes(minute);

            
            JwtSecurityToken securityToken = new(
                audience: _config["Token:Audience"],
                issuer: _config["Token:Issuer"],
                claims: claims,
                expires: token.Expiration,
                notBefore:DateTime.UtcNow,
                signingCredentials:signingCredentials
                );
            JwtSecurityTokenHandler handler = new();
            token.AccessToken = handler.WriteToken(securityToken);  
            return token;
        }

       
    }
}
