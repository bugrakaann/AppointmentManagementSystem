using Data_Access_Layer.Data;
using Microsoft.AspNetCore.Identity;
using Models.Models;
using Business;
using AppointmentManagementSystem.Middlewares;


var builder = WebApplication.CreateBuilder(args);

var conString = builder.Configuration.GetConnectionString("DefaultConnection");


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.RegisterServices(conString);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


var app = builder.Build();

// Initializer
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await Business.Data.SeedData.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/l", () => Results.Redirect("/Account/Login"));
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{code?}");

app.Run();