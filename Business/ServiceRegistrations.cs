using Business.Profiles;
using Business.Services;
using Business.Services.Abstract;
using Data_Access_Layer.Data;
using Data_Access_Layer.Repositories;
using Data_Access_Layer.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

public static class ServiceRegistrations
{
    public static void RegisterServices(this IServiceCollection services, string connectionString)
    {

        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        // Register services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddSingleton<IGoogleCalendarService, GoogleCalendarService>();

        // Register repositories
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

    }



}