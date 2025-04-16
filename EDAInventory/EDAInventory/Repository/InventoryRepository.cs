using EDADBContext;
using EDADBContext.Models;
using EDAInventory.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EDAInventory.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DBContext _dBContext;
        public InventoryRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Product>> GetProductsList()
        {
            return await _dBContext.Products.ToListAsync();
        }

        public async Task<string> UpsertProduct(Product? product, bool IsUpdate = false)
        {
            string tableName = Helper.Utlity.GetClassName<Product>(product);
            try
            {
                if (product != null)
                {
                    if (IsUpdate)
                    {
                        _dBContext.Products.Update(product);
                    }
                    else
                    {
                        _dBContext.Products.Add(product);
                    }
                    var result = await _dBContext.SaveChangesAsync();
                    return result > 0 ? string.Empty : String.Format(Constants.DBInsertFailureMessage, tableName);
                }
                return String.Format(Constants.DataNullErrorMessage, tableName);

            }
            catch (Exception ex)
            {
                return String.Format(Constants.ExceptionWhileInsertingorUpdatingData + ex.Message, tableName);
            }
        }

       public async Task<Product?> GetProductById(Guid productId)
        {

            if(productId == Guid.Empty)
            {
                return null; 
            }

            return await _dBContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId) ?? null;
        }
    }
}
