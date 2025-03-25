#nullable disable
using System.ComponentModel.DataAnnotations;

namespace EDADBContext.Models
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
