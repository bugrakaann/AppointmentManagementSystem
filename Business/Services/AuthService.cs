using Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Models.Models;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInResult> PasswordSignIn(string username, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, false);

        if (!result.Succeeded)
        {
            throw new ArgumentException("Giriş başarısız!");
        }

        return result;
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
    }
}