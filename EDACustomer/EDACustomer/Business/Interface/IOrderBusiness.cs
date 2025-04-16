using OrderService.Models;

namespace OrderService.Business.Interface
{
    public interface IOrderBusiness
    {
        Task<List<OrderModel>> GetOrdersCheckOutList();
        Task<string> AddCheckoutOrder(OrderModel customer);
    }
}
