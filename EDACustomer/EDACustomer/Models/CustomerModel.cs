#nullable disable
namespace EDACustomer.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public int ItemInCart { get; set; }
    }
}
