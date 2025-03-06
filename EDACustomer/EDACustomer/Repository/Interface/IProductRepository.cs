using EDACustomer.Models;

namespace EDACustomer.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetProductsList();
    }
}
