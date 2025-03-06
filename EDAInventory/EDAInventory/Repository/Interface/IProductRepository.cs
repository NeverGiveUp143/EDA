using EDADBContext.Models;

namespace EDAInventory.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsList();
    }
}
