using EDACustomer.Repository.Interface;
using EDADBContext;
using EDADBContext.Models;
using Microsoft.EntityFrameworkCore;

namespace EDACustomer.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DBContext _dBContext;
        public CustomerRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Customer>> GetCustomersList()
        {
           return await _dBContext.Customer.ToListAsync(); 
        }

        public async Task<string> AddCustomer(Customer customer)
        {
            try
            {
                if (customer != null)
                {                   
                    _dBContext.Customer.Add(customer);
                    var result =  await _dBContext.SaveChangesAsync();
                    return result > 0  ? string.Empty : Constants.DBInsertFailureMessage ;
                }
                return Constants.CustomerDataNullErrorMessage;
            }
            catch (Exception ex)
            {
              return Constants.ExceptionWhileInsertingData+ex.Message;
            }
           
        }

    }
}
