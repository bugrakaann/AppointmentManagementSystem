using System.Security.Claims;
using System.Security.Principal;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Models;

namespace Business.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
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

    public bool IsLoggedIn()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.Identity?.IsAuthenticated ?? false;
    }

    public async Task<ApplicationUser> GetLoggedUser()
    {
        if (!IsLoggedIn())
        {
            throw new ArgumentException("Kullanıcı girişi yapmamış");
        }

        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            throw new ArgumentException("Kullanıcı bilgileri eksik");
        }

        var userId = _userManager.GetUserId(user);
        if (userId == null)
        {
            throw new ArgumentException("Kullanıcı ID alınamadı");
        }

        var appUser = await _userManager.FindByIdAsync(userId);
        if (appUser == null)
        {
            throw new ArgumentException("Kullanıcı bulunamadı");
        }

        return appUser;
    }
}