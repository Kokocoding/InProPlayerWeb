using System.ComponentModel.DataAnnotations;

namespace InProPlayerWeb.Models
{
    public class SchedulerNPDay
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string? DayName { get; set; }
        [Required]
        public string? NPDay { get; set; }
    }
}
