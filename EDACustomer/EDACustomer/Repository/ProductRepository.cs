using EDACustomer.Models;
using EDACustomer.Repository.Interface;
using EDADBContext;
using Microsoft.EntityFrameworkCore;

namespace EDACustomer.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DBContext _dBContext;
        public ProductRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<ProductModel>> GetProductsList()
        {
            return await _dBContext.Products.Select(x =>  new ProductModel
            {
                Id = x.Id,
                Name = x.Name,
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToListAsync();
        }
    }
}
