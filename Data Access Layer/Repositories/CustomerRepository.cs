using Data_Access_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public class CustomerRepository :  Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }
}