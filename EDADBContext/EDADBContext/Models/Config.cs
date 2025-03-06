#nullable disable
using System.ComponentModel.DataAnnotations;

namespace EDADBContext.Models
{
    public class Config
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
