#nullable disable
namespace EDACustomer.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
}
