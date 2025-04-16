using OrderService.Models;
using OrderService.Repository.Interface;
using EDADBContext;
using EDADBContext.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DBContext _dBContext;
        public OrderRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<OrderModel>> GetOrdersCheckOutList()
        {
            return await (from customer in _dBContext.Customer
                          join product in _dBContext.Products
                          on customer.ProductId equals product.ProductId
                          select new OrderModel
                          {
                              Name = customer.Name,
                              Product = customer.ProductId.ToString(),
                              ProductName = product.Name,
                              Email = customer.Email,
                              Id = customer.Id,
                              ItemInCart = customer.ItemInCart
                          }).ToListAsync();
        }

        public async Task<string> AddCheckoutOrder(Customer customer)
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
