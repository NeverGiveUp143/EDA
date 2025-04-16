using EDADBContext.Models;
using EDAInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDAInventory.Business.Interface
{
    public interface IInventoryBusiness
    {
        Task<List<ProductModel>> GetProductsList();
        Task<string> UpsertProduct(ProductModel? product, bool IsUpdate = false);
        Task<string> ModifyStock(Guid productId, int quantity);
        Task<string> GetProductById(Guid productId);

    }
}
