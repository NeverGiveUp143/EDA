using EDACustomer.Models;

namespace EDACustomer.Business.Interface
{
    public interface ICustomerBusiness
    {
        Task<List<CustomerModel>> GetCustomersList();
        Task<string> AddCustomer(CustomerModel customer);
    }
}
