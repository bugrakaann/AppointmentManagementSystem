using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Data_Access_Layer.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public ApplicationDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.customer)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.customerId); //??
        
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
}