using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace AppointmentManagementSystem.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _authService.PasswordSignIn(model.UserName, model.Password, model.RememberMe);
            return RedirectToAction("Index", "Home");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.SignOut();
        return RedirectToAction("Index", "Home");
    }

}