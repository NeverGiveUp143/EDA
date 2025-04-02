#nullable disable
namespace EDAInventory.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public int ItemInCart { get; set; }
    }
}
