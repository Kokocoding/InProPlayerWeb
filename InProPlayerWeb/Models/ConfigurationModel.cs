using System.ComponentModel.DataAnnotations;

namespace InProPlayerWeb.Models
{
    public class Configuration
    {
        [Key]
        public int id { get; set; }
        public int SchedulerGroupID { get; set; }
        [Required]
        public string PortName { get; set; }
        [Required]
        public int BaudRate { get; set; }
        public string Frequency { get; set; }
        public string Channel { get; set; }
        public int PreAmpPower { get; set; }
        [Required]
        public string Lang { get; set; }
        [Required]
        public string Account { get; set; } 
        [Required]
        public string PassWord { get; set; }
        [Required]
        public bool IsAccountMemory { get; set; }

    }
}
