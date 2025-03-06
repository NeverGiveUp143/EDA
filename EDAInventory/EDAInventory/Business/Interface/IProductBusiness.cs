using EDADBContext.Models;

namespace EDAInventory.Business.Interface
{
    public interface IProductBusiness
    {
        Task<List<Product>> GetProductsList();
    }
}
