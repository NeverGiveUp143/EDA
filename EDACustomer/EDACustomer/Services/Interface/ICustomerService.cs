using EDACustomer.Models;

namespace EDACustomer.Services.Interface
{
    public interface ICustomerService
    {
        Task UpdateStockAndNotify(Guid productId, int newStock);
        Task<string> UpdatedProduct(Guid productId, int checkedOutQuantity);
    }
}
