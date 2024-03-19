using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Api.DTO;
using Api.Entities;
using Api.Interfaces;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterNewUser()
        {
            var username = GenerateName();
            while (await UserExists(username))
            {
                username = GenerateName();
            }
            var user = new AppUser() { UserName = username };
            var result =await  _userManager.CreateAsync(user, username);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LogIn(string login)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == login.ToUpper());
            if(user == null)
            {
                return Unauthorized("Invalid promocode");
            }
            var result = await  _signInManager.CheckPasswordSignInAsync(user, user.UserName, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid promocode");
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        private static string GenerateName(int n = 6)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var sb = new StringBuilder();
            var rnd = new Random();
            while(sb.Length < n)
            {
                sb.Append(chars[rnd.Next(0, chars.Length)]);
            }
            return sb.ToString();
        }

        async private Task<bool> UserExists(string userName)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == userName);
        }
    }
}
