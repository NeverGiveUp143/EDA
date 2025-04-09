using EDACustomer.Models;
using EDACustomer.Repository.Interface;
using EDADBContext;
using EDADBContext.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace EDACustomer.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DBContext _dBContext;
        public ProductRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Product>> GetProductsList()
        {
            return await _dBContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
            if(productId == Guid.Empty)
            {
                return null;
            }

            return await _dBContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<List<ProductDDModel>> GetProductDDList()
        {
            return await _dBContext.Products.Select(x => new ProductDDModel
            {
                label = x.Name,
                value = x.ProductId.ToString()
            }).ToListAsync();
        }

        public async Task<Product?> GetProductNameById(Guid productId)
        {

            if (productId == Guid.Empty)
            {
                return null;
            }

            return await _dBContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId) ?? null;
        }
    }
}
