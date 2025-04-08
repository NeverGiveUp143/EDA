using EDACustomer.Models;
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

        public async Task<List<CustomerModel>> GetCustomersList()
        {
            return await (from customer in _dBContext.Customer
                          join product in _dBContext.Products
                          on customer.ProductId equals product.ProductId
                          select new CustomerModel
                          {
                              Name = customer.Name,
                              Product = product.Name,
                              Email = customer.Email,
                              Id = customer.Id,
                              ItemInCart = customer.ItemInCart
                          }).ToListAsync();

        }

        public async Task<string> AddCustomer(Customer customer)
        {
            string tableName = Helper.Utlity.GetClassName<Customer>(customer);
            try
            {
                if (customer != null)
                {
                    _dBContext.Customer.Add(customer);
                    var result = await _dBContext.SaveChangesAsync();
                    return result > 0 ? customer.Id.ToString() : String.Format(Constants.DBInsertFailureMessage, tableName);
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
