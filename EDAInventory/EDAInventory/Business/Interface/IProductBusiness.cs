using EDAInventory.Models;

namespace EDAInventory.Business.Interface
{
    public interface IProductBusiness
    {
        Task<List<ProductModel>> GetProductsList();
    }
}
