namespace EDACustomer.Services.Interface
{
    public interface ICustomerService
    {
        Task UpdateStockAndNotify(Guid productId, int newStock);
    }
}
