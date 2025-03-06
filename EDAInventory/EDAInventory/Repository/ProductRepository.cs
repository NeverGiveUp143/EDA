using EDADBContext;
using EDADBContext.Models;
using EDAInventory.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EDAInventory.Repository
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
    }
}
