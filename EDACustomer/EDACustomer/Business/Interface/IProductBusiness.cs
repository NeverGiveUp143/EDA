using EDACustomer.Models;

namespace EDACustomer.Business.Interface
{
    public interface IProductBusiness
    {
        Task<List<ProductModel>> GetProductsList();
    }
}
