using System.Security.Authentication;
using System.Security.Claims;
using Core.Enities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Shop_App.Extensions;

public static class ClaimPrincipleExtensions
{
    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user) =>
        await userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == user.GetEmail()) ?? throw new AuthenticationException("User not found");
    
    public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user) =>
        await userManager.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Email == user.GetEmail())
             ?? throw new AuthenticationException("User not found");

    public static string GetEmail(this ClaimsPrincipal user) =>
         user.FindFirstValue(ClaimTypes.Email) 
                    ?? throw new AuthenticationException("Email claim not found");
}