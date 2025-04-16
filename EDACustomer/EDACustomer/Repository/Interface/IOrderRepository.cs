using OrderService.Models;
using EDADBContext.Models;

namespace OrderService.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<List<OrderModel>> GetOrdersCheckOutList();
        Task<string> AddCheckoutOrder(Customer customer);
    }
}
