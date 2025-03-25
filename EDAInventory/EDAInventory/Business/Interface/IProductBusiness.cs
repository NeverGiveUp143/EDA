using EDAInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDAInventory.Business.Interface
{
    public interface IProductBusiness
    {
        Task<List<ProductModel>> GetProductsList();
        Task<string> UpsertProduct(ProductModel? product, bool IsUpdate = false);
    }
}
