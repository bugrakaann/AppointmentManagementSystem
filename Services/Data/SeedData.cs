using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models.Models;
using System.Data;

namespace Services.Data;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await InitRoles(roleManager);
        await InitUsers(userManager);
    }


    private static async Task InitRoles(RoleManager<IdentityRole> roleManager)
    {

        string[] roleNames = { "Admin", "User" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public static async Task InitUsers(UserManager<ApplicationUser> userManager)
    {
        var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
        if (!adminUsers.Any())
        {
            var adminPassword = "Admin123.";
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(adminUser, ["Admin", "User"]);
            }
        }
    }
}