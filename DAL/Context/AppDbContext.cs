using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Context
{

    public class AppDbContext : DbContext
    {
        public IConfiguration _config { get; set; }


        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultDatabaseConnection"));
        }
        
        public DbSet<AppointmentEntity> Appointments { get; set; }

    }
}
