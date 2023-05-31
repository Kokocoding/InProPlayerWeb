using System.ComponentModel.DataAnnotations;

namespace InProPlayerWeb.Models
{
    public class Group
    {
        
        [Key]
        public int id { get; set; }
        [Required]
        public string? title { get; set; }
    }
}
