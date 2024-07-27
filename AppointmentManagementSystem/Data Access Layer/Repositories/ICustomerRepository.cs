using AppointmentManagementSystem.Models;

namespace AppointmentManagementSystem.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    IEnumerable<Customer> GetAll();
    Customer GetById(int id);
    void Add(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
    void Delete(int id);
}