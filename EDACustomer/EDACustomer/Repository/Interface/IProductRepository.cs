using EDADBContext.Models;

namespace EDACustomer.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsList();
    }
}
