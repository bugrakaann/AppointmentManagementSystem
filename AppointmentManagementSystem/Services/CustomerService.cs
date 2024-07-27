using AppointmentManagementSystem.DTOs;
using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.Repositories;
using AutoMapper;

namespace AppointmentManagementSystem.Services;

public class CustomerService : ICustomerService
{
    public readonly IRepository<Customer> _customerRepository;
    public readonly IMapper _mapper;

    public CustomerService(IRepository<Customer> repository, IMapper mapper)
    {
        _customerRepository = repository;
        if(_customerRepository == null)
        {
            throw new ArgumentException("Invalid repository type");
        }
        _mapper = mapper;
    }
    
    public IEnumerable<CustomerDto> GetAll()
    {
        var customers = _customerRepository.GetAll();
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public CustomerDto GetById(int id)
    {
        var customer = _customerRepository.GetById(id);
        return _mapper.Map<CustomerDto>(customer);
    }

    public void Add(CustomerDto customer)
    {
        var _customer = _mapper.Map<Customer>(customer);
        _customerRepository.Add(_customer);
    }

    public void Update(CustomerDto customer)
    {
        var _customer = _mapper.Map<Customer>(customer);
        _customerRepository.Update(_customer);
    }

    public void Delete(int id)
    {
        _customerRepository.Delete(id);
    }

    public void Delete(CustomerDto customer)
    {
        var _customer = _mapper.Map<Customer>(customer);
        _customerRepository.Delete(_customer);
    }
}