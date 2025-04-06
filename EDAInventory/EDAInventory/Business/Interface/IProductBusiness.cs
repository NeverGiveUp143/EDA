using EDADBContext.Models;
using EDAInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDAInventory.Business.Interface
{
    public interface IProductBusiness
    {
        Task<List<ProductModel>> GetProductsList();
        Task<string> UpsertProduct(ProductModel? product, bool IsUpdate = false);
        Task<string> DeductStock(Guid productId, int quantity);
        Task<string> GetProductById(Guid productId);

    }
}
