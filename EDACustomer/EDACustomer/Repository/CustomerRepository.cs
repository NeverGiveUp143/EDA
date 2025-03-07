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
            string tableName = Helper.Utlity.GetClassName<Customer>(customer);
            try
            {
                if (customer != null)
                {                   
                    _dBContext.Customer.Add(customer);
                    var result =  await _dBContext.SaveChangesAsync();
                    return result > 0  ? string.Empty : String.Format(Constants.DBInsertFailureMessage, tableName);
                }
                return String.Format(Constants.DataNullErrorMessage, tableName);
            }
            catch (Exception ex)
            {
                return String.Format(Constants.ExceptionWhileInsertingorUpdatingData + ex.Message, tableName);
            }
           
        }

    }
}
