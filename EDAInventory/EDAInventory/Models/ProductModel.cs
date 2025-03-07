#nullable disable
namespace EDAInventory.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
