using OrderService.Models;
using EDADBContext.Models;

namespace OrderService.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsList();
        Task<Product?> GetProductById(Guid productId);
        Task<List<ProductDDModel>> GetProductDDList();
        Task<Product?> GetProductNameById(Guid productId);
    }
}
