using EDACustomer.Models;

namespace EDACustomer.Services.Interface
{
    public interface ICustomerService
    {
        Task UpdateStockAndNotify(Guid productId, int newStock);
        Task<ProductModel> UpdatedProduct(Guid productId, int checkedOutQuantity);
    }
}
