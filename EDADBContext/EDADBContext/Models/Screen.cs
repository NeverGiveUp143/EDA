using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDADBContext.Models
{
    public class Screen
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? ScreenLabel { get; set; }

        [ForeignKey("Module")]
        public int ModuleId { get; set; }

        [Required]
        public virtual Module? Module { get; set; }
    }
}
