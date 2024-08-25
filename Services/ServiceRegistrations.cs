using Data_Access_Layer.Data;
using Data_Access_Layer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Profiles;
using Services.Services;

namespace Services;

public static class ServiceRegistrations  
{
    public static void AddMyLibraryServices(this IServiceCollection services , string connectionString)
    {
        
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        
        
        // Register services
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICalendarService, GoogleCalendarService>();

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

    }
    

    
}