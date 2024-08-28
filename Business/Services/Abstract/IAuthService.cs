using Microsoft.AspNetCore.Identity;

namespace Business.Services.Abstract;

public interface IAuthService
{
    Task<SignInResult> PasswordSignIn(string username, string password, bool rememberMe);
    Task SignOut();
}