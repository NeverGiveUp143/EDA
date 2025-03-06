using EDACustomer.Models;
using EDADBContext.Models;

namespace EDACustomer.Repository.Interface
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomersList();
        Task<string> AddCustomer(Customer customer);
    }
}
