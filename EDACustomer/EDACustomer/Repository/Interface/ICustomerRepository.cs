using EDACustomer.Models;
using EDADBContext.Models;

namespace EDACustomer.Repository.Interface
{
    public interface ICustomerRepository
    {
        Task<List<CustomerModel>> GetCustomersList();
        Task<string> AddCustomer(Customer customer);
    }
}
