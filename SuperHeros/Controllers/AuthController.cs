using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SuperHeros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto Request)
        {

            CreatePasswordHash(Request.Password, out byte[] PasswordHash, out byte[] PasswordSalt);

            user.UserName = Request.UserName;
            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;

            return Ok(user);

        }

        [HttpPost("Login")]

        public async Task<ActionResult<String>> Login(UserDto Request)
        {
            if(user.UserName != Request.UserName)
            {
                return BadRequest("User not Found");
            }
            if(!VerifyPassword(Request.Password, user.PasswordHash, user.PasswordHash))
            {
                return BadRequest("Worng Password");
            }

            string Token = CreatToken(user);
            return Ok(Token);
            
        }

        private string CreatToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSetting:Token").Value));

            var cerds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cerds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        } 

        private void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using(var hmac= new HMACSHA512())
            {
                PasswordSalt = hmac.Key; 
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using(var hmac= new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes((string)password));
                return computedHash.SequenceEqual(PasswordHash);
            }

        }
    }
}
