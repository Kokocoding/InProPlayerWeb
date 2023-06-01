using Microsoft.EntityFrameworkCore;
using InProPlayerWeb.Models;

namespace InProPlayerWeb.Helper
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<SchedulerNPDay> SchedulerNPDay { get; set; } 
    }
}
