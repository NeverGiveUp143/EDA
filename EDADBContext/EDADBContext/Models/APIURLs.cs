#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDADBContext.Models
{
    public class APIURLs
    {
        [Key]
        public int Id { get; set; }
        public string PostURL { get; set; }
        public string FetchURL { get; set; }
        public string PutURL { get; set; }
        public string DeleteURL { get; set; }
        [ForeignKey("Screen")]
        public int ScreenId { get; set; }
        public virtual Screen screen { get; set; }
    }
}
