using EDACustomer.Models;
using EDADBContext.Models;

namespace EDACustomer.Business.Interface
{
    public interface IProductBusiness
    {
        Task<List<ProductModel>> GetProductsList();
        Task<ProductModel> GetProductById(Guid productId);
        Task<List<ProductDDModel>> GetProductDDList();
    }
}
