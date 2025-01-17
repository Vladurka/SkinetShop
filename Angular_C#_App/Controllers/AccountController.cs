using Core.Enities;
using Core.Interfaces.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Shop_App.Controllers
{
    public class AccountController(SignInManager<AppUser> signInManager) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser(registerDto.FirstName, registerDto.LastName,
                registerDto.UserName, registerDto.Email);

            var result = await signInManager.UserManager.CreateAsync(user, registerDto.Pass);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout(RegisterDto registerDto)
        {
            await signInManager.SignOutAsync();

            return NoContent();
        }

        [Authorize]
        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo(RegisterDto registerDto)
        {
            if (User.Identity?.IsAuthenticated == false)
                return NoContent();

            var user = await signInManager.UserManager.Users
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
            });
        }

        [HttpGet]
        public ActionResult GetAuthState() =>
            Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
    }
}
