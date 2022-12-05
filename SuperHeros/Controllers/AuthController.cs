using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SuperHeros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

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
            else
            {
                return Ok("MY CRAZY TOKEN");
            }
        }

        private void CreatePasswordHash(string password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using(var hmac= new HMACSHA512())
            {
                PasswordSalt = hmac.Key; 
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
