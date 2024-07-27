using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.customer)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.customerId); //??
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
}