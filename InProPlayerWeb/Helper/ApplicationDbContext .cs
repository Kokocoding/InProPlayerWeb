using Microsoft.EntityFrameworkCore;
using InProPlayerWeb.Models;
using Microsoft.Extensions.Configuration;

namespace InProPlayerWeb.Helper
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<Configuration> Configuration { get; set; }
    }
}
