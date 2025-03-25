using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDADBContext.Models
{
    public class FormFields
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required] 
        public string? Type { get; set; }
        public string? url { get; set; }
        public string? defaultValue { get;set; }
        public string? style { get; set; }
        [ForeignKey("Screen")]
        public int screenId { get; set; }
        public virtual Screen? screen { get; set; }
    }
}
