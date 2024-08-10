using AutoMapper;
using Data_Access_Layer.Repositories;
using DTOs.DTOs;
using Models.Models;

namespace Services.Services;

public class CustomerService : ICustomerService
{
    public readonly IMapper _mapper;

    ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
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
        customer.id = _customer.id;
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