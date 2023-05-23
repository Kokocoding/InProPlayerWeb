using System.ComponentModel.DataAnnotations;

namespace InProPlayerWeb.Models
{
    public class Configuration
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string? PortName { get; set; }
        [Required]
        public int BaudRate { get; set; }
    }
}
