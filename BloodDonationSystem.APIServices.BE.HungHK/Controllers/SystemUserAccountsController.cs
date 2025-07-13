using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BloodDonationSystem.Repositories.HungHK.Models;
using BloodDonationSystem.Services.HungHK;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BloodDonationSystem.APIServices.BE.HungHK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserAccountsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISystemUserAccountService _userAccountsService;

        public SystemUserAccountsController(IConfiguration config, ISystemUserAccountService userAccountsService)
        {
            _config = config;
            _userAccountsService = userAccountsService;     //// Add DJ
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _userAccountsService.GetUserAccountAsync(request.UserName, request.Password);

            if (user == null || user.Result == null)
                return Unauthorized();

            var token = GenerateJSONWebToken(user.Result);

            return Ok(token);
        }
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<SystemUserAccount>> Get()
        {
            return await _userAccountsService.GetAllAsync();
        }

        private string GenerateJSONWebToken(SystemUserAccount systemUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                    , _config["Jwt:Audience"]
                    , new Claim[]
                    {
                new(ClaimTypes.Name, systemUserAccount.UserName),
                new Claim(ClaimTypes.NameIdentifier, systemUserAccount.UserAccountId.ToString()),
                //new(ClaimTypes.Email, systemUserAccount.Email),
                new(ClaimTypes.Role, systemUserAccount.RoleId.ToString()),
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public sealed record LoginRequest(string UserName, string Password);
    }
}
