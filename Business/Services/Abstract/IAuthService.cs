using Microsoft.AspNetCore.Identity;
using Models.Models;

namespace Business.Services.Abstract;

public interface IAuthService
{
    Task<SignInResult> PasswordSignIn(string username, string password, bool rememberMe);
    Task SignOut();
    bool IsLoggedIn();

    Task<ApplicationUser> GetLoggedUser();
}