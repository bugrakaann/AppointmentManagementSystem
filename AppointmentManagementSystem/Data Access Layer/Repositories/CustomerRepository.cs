using AppointmentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.Repositories;

public class CustomerRepository :  Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }
    public IEnumerable<Customer> GetAll()
    {
        return _context.Customers.Include(c=> c.Appointments).ToList(); 
    }

    public Customer GetById(int id)
    {
        return _context.Customers.Include(c=> c.Appointments).FirstOrDefault(c => c.id == id);
    }

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    public void Delete(Customer customer)
    {
        _context.Customers.Remove(customer);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        Customer customer = _context.Customers.FirstOrDefault(a=>a.id == id);
        if(customer == null) return;
        _context.Customers.Remove(customer);
        _context.SaveChanges();
    }
}