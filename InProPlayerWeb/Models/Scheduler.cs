using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace InProPlayerWeb.Models
{
    public class Scheduler
    {
        [Key]
        public int id { get; set; }
		[Required]
		public int GroupID { get; set; }
		public string SchedulerName { get; set; }
		public string StartCron { get; set; }
		public int LoopType { get; set; }
		public int LoopTimes { get; set; }
		public int KeepTimes { get; set; }
		public string Music { get; set; }
		public string Terminal { get; set; }
    }

	public class SchedulerList : Scheduler
    {
		public string? Week { get; set; }
		public string? Time { get; set; }	
		public string? GroupName { get; set; }

    }
}
