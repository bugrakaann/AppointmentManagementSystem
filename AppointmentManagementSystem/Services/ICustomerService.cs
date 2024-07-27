using System.Collections;
using AppointmentManagementSystem.DTOs;

namespace AppointmentManagementSystem.Services;

public interface ICustomerService
{
    IEnumerable<CustomerDto> GetAll();
    CustomerDto GetById(int id);
    void Add(CustomerDto customer);
    void Update(CustomerDto customer);
    void Delete(int id);
    void Delete(CustomerDto customer);
}